﻿using Domain.Entities;
using Domain.Interfaces;
using Domain.Options;
using Domain.Utils;
using Microsoft.Extensions.Options;
using Serilog;

namespace Application.EventHandlers.CreateBillToPayEvent
{
    public class CreateBillToPayEventHandler : ICreateBillToPayEventHandler
    {
        private readonly ILogger _logger;
        private readonly BillToPayOptions _billToPayOptions;
        private readonly IFixedInvoiceRepository _fixedInvoiceRepository;
        private readonly IBillToPayRepository _billToPayRepository;
        private const string FREQUENCIA_MENSAL_RECORRENTE = "Mensal:Recorrente";
        private const string FREQUENCIA_MENSAL = "Mensal";
        private const string FREQUENCIA_LIVRE = "Livre";
        private const int DIA_VENCIMENTO_CARTAO_CREDITO = 9;
        private const string TIPO_REGISTRO_FATURA_FIXA = "Conta/Fatura Fixa";
        private const string TIPO_REGISTRO_COMPRA_LIVRE = "Compra Livre";
        private const int QUANTOS_DIAS_PASSADOS_CONSIDERAR = -1;

        public CreateBillToPayEventHandler(
            ILogger logger,
            IOptions<BillToPayOptions> options,
            IFixedInvoiceRepository fixedInvoiceRepository,
            IBillToPayRepository billToPayRepository)
        {
            _logger = logger;
            _fixedInvoiceRepository = fixedInvoiceRepository;
            _billToPayOptions = options.Value;
            _billToPayRepository = billToPayRepository;
        }

        public async Task Handle(CreateBillToPayEventInput input)
        {
            var participants = await _fixedInvoiceRepository
                .GetOnlyOldRecordsAndParticipants(QUANTOS_DIAS_PASSADOS_CONSIDERAR, TIPO_REGISTRO_FATURA_FIXA);

            foreach (var toProcess in participants)
            {
                try
                {
                    var json = JsonSerializeUtils.Serialize(toProcess);

                    _logger.Information("Objeto FixedInvoice que será processado: {@json}", json);

                    var billsToPay = await _billToPayRepository.GetBillToPayByFixedInvoiceId(toProcess.Id);

                    var lastBillToPay = GetLastRegistrationBillToPay(billsToPay);

                    await StartRegistration(toProcess, lastBillToPay);

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Erro ao efetuar o processo para cada item participante. Erro: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Responsável de verificar se o registro no banco de dados está elegível para cadastro agora.
        /// </summary>
        /// <param name="fixedInvoice"></param>
        /// <returns></returns>
        private async Task StartRegistration(FixedInvoice fixedInvoice, BillToPay? billToPay)
        {
            await LogicRegistration(fixedInvoice, billToPay);
        }

        /// <summary>
        /// Contempla a lógica para cadastro de novas contas a pagar de acordo com o tempo que ela se encontra.
        /// </summary>
        /// <param name="fixedInvoice"></param>
        public async Task LogicRegistration(FixedInvoice fixedInvoice, BillToPay? billToPay)
        {
            if (billToPay != null)
            {
                await LogicByBillToPay(billToPay);
            }
            else
            {
                await LogicByFixedInvoice(fixedInvoice);
            }
        }

        private async Task LogicByFixedInvoice(FixedInvoice fixedInvoice)
        {
            List<BillToPay> listBillToPay = new();

            var totalMonths = DateServiceUtils.GetMonthsByDateTime(
                DateServiceUtils.GetDateTimeByYearMonthBrazilian(fixedInvoice.InitialMonthYear),
                DateServiceUtils.GetDateTimeByYearMonthBrazilian(fixedInvoice.FynallyMonthYear));

            int qtdMonthAdd;

            if (fixedInvoice.FynallyMonthYear == null)
            {
                qtdMonthAdd = GetMonthsAdd(totalMonths, _billToPayOptions.HowManyMonthForward);
            }
            else
            {
                qtdMonthAdd = totalMonths;
            }

            if (qtdMonthAdd <= 0 || totalMonths > _billToPayOptions.HowManyMonthForward)
            {
                return;
            }

            bool addMonthForDueDate = false;

            if (fixedInvoice.Account == Account.CARTAO_CREDITO)
            {
                addMonthForDueDate = true;
            }

            Dictionary<string, DateTime> purchasesDate = new();
            bool considerPurchase = false;

            if (fixedInvoice.Account == Account.CARTAO_CREDITO)
            {
                // Cartão de Crédito

                fixedInvoice.BestPayDay = DIA_VENCIMENTO_CARTAO_CREDITO;

                if (fixedInvoice.Frequence == FREQUENCIA_MENSAL_RECORRENTE)
                {
                    // Crédito Recorrente.
                    considerPurchase = true;
                    purchasesDate = DateServiceUtils
                        .GetNextYearMonthAndDateTime(fixedInvoice.PurchaseDate, qtdMonthAdd, null, true);
                }
            }

            if (fixedInvoice.InitialMonthYear == fixedInvoice.FynallyMonthYear
                && DateServiceUtils.IsCurrentMonth(fixedInvoice.InitialMonthYear, fixedInvoice.FynallyMonthYear))
            {
                qtdMonthAdd = 0;
            }

            var nextMonthYearToRegister = DateServiceUtils
                .GetNextYearMonthAndDateTime(
                null, qtdMonthAdd, fixedInvoice.BestPayDay,
                DateServiceUtils.IsCurrentMonth(fixedInvoice.InitialMonthYear, fixedInvoice.FynallyMonthYear), addMonthForDueDate);

            foreach (var nextMonth in nextMonthYearToRegister!)
            {
                DateTime? purchase;
                if (considerPurchase)
                {
                    purchase = purchasesDate.GetValueOrDefault(nextMonth.Key);

                    if (purchase == DateTime.MinValue)
                    {
                        continue;
                    }
                }
                else
                {
                    purchase = fixedInvoice.PurchaseDate;
                }

                listBillToPay.Add(MapBillToPay(null, fixedInvoice, nextMonth.Value, nextMonth.Key, purchase));
            }

            await DebitFixedAccount(fixedInvoice, qtdMonthAdd);

            EditFixedInvoice(fixedInvoice);

            await _fixedInvoiceRepository.Edit(fixedInvoice);

            await _billToPayRepository.SaveRange(listBillToPay);
        }

        private async Task DebitFixedAccount(FixedInvoice fixedInvoice, int qtdMonthAdd)
        {
            if (IsValidToDebit(fixedInvoice, qtdMonthAdd))
            {
                var descontar = await _billToPayRepository
                    .GetByYearMonthCategoryAndRegistrationType(
                    fixedInvoice.InitialMonthYear!, fixedInvoice.Category!, TIPO_REGISTRO_FATURA_FIXA);

                if (descontar == null)
                {
                    return;
                }

                if (descontar.HasPay)
                {
                    return;
                }

                if (descontar.Frequence == FREQUENCIA_MENSAL)
                {
                    var valueOld = descontar.Value;

                    descontar.Value -= fixedInvoice.Value;
                    descontar.AdditionalMessage += $"Retirado: [R$ {fixedInvoice.Value}] em [{DateTime.Now.Date}] do valor que estava: [R$ {valueOld}] pela seguinte conta: [{fixedInvoice.Name}] | ";

                    var edited = await _billToPayRepository.Edit(descontar);

                    if (edited == 1)
                    {
                        _logger.Information($"A conta relacionada de Id [{descontar.Id}] foi descontado valores com base no gasto da conta de Id [{fixedInvoice.Id}] com a seguinte informação [{descontar.AdditionalMessage}]");
                    }
                }
            }
        }

        private static bool IsValidToDebit(FixedInvoice fixedInvoice, int qtdMonthAdd)
        {
            var isTrue = fixedInvoice.RegistrationType == TIPO_REGISTRO_COMPRA_LIVRE
                      && qtdMonthAdd == 0
                      && fixedInvoice.Frequence == FREQUENCIA_LIVRE;

            return isTrue;
        }

        public static bool EnterPaid(FixedInvoice? fixedInvoice)
        {
            bool considerPaid = false;

            if (fixedInvoice == null)
            {
                return considerPaid;
            }

            switch (fixedInvoice.Account)
            {
                case Account.CARTAO_VALE_ALIMENTACAO:
                case Account.CARTAO_VALE_REFEICAO:
                case Account.CARTAO_DEBITO:
                    if (fixedInvoice.RegistrationType != TIPO_REGISTRO_FATURA_FIXA)
                    {
                        considerPaid = true;
                    }
                    break;
            }

            return considerPaid;
        }

        private async Task LogicByBillToPay(BillToPay billToPay)
        {
            var result = await _fixedInvoiceRepository.GetById(billToPay.IdFixedInvoice);

            if (result?.FynallyMonthYear?.Length > 0)
            {
                await EditFixedInvoiceNow(billToPay);
                return;
            }

            List<BillToPay> listBillToPay = new();

            var totalMonths = DateServiceUtils.GetMonthsByDateTime(billToPay.DueDate, null);

            var qtdMonthAdd = GetMonthsAdd(totalMonths, _billToPayOptions.HowManyMonthForward);

            if (qtdMonthAdd <= 0 || totalMonths > _billToPayOptions.HowManyMonthForward)
            {
                await EditFixedInvoiceNow(billToPay);
                return;
            }

            var nextMonthYearToRegister = DateServiceUtils
                .GetNextYearMonthAndDateTime(
                billToPay.DueDate, qtdMonthAdd, null, false);

            foreach (var nextMonth in nextMonthYearToRegister!)
            {
                listBillToPay.Add(MapBillToPay(billToPay, null, nextMonth.Value, nextMonth.Key));
            }

            await _billToPayRepository.SaveRange(listBillToPay);
        }

        public static int GetMonthsAdd(int totalMonths, int howManyMonthForward)
        {
            if (totalMonths > howManyMonthForward)
            {
                return 0;
            }

            var result = Math.Abs(totalMonths - howManyMonthForward);

            return result;
        }

        public static BillToPay MapBillToPay(
            BillToPay? billToPay, FixedInvoice? fixedInvoice, DateTime dueDate, string yearMonth, DateTime? purchaseDate = null)
        {
            if (billToPay is not null)
            {
                return new BillToPay()
                {
                    Id = Guid.NewGuid(),
                    IdFixedInvoice = billToPay!.IdFixedInvoice,
                    Account = billToPay.Account,
                    Name = billToPay.Name,
                    Category = billToPay.Category,
                    RegistrationType = billToPay.RegistrationType,
                    Value = billToPay.Value,
                    PurchaseDate = purchaseDate,
                    DueDate = dueDate,
                    YearMonth = yearMonth,
                    Frequence = billToPay.Frequence,
                    PayDay = null,
                    HasPay = false,
                    AdditionalMessage = billToPay.AdditionalMessage,
                    CreationDate = DateTime.Now,
                    LastChangeDate = new DateTime(1753, 01, 01, 12, 0, 0, DateTimeKind.Local)
                };
            }
            else
            {
                var newBillToPay = new BillToPay()
                {
                    Id = Guid.NewGuid(),
                    IdFixedInvoice = fixedInvoice!.Id,
                    Account = fixedInvoice.Account,
                    Name = fixedInvoice.Name,
                    Category = fixedInvoice.Category,
                    RegistrationType = fixedInvoice.RegistrationType,
                    Value = fixedInvoice.Value,
                    PurchaseDate = purchaseDate,
                    DueDate = dueDate,
                    YearMonth = yearMonth,
                    Frequence = fixedInvoice.Frequence,
                    PayDay = null,
                    HasPay = false,
                    AdditionalMessage = fixedInvoice.AdditionalMessage,
                    CreationDate = DateTime.Now,
                    LastChangeDate = new DateTime(1753, 01, 01, 12, 0, 0, DateTimeKind.Local)
                };

                if (EnterPaid(fixedInvoice))
                {
                    newBillToPay.HasPay = true;
                    newBillToPay.PayDay = purchaseDate.ToString();
                    newBillToPay.LastChangeDate = DateTime.Now;
                }

                return newBillToPay;
            }
        }

        public async Task EditFixedInvoiceNow(BillToPay billToPay)
        {
            var fixedInvoiceByDb = await _fixedInvoiceRepository.GetById(billToPay.IdFixedInvoice);

            if (fixedInvoiceByDb == null)
            {
                return;
            }

            var fixedInvoice = new FixedInvoice()
            {
                Id = fixedInvoiceByDb!.Id,
                Name = fixedInvoiceByDb.Name,
                Account = fixedInvoiceByDb.Account,
                Frequence = fixedInvoiceByDb.Frequence,
                RegistrationType = fixedInvoiceByDb.RegistrationType,
                PurchaseDate = fixedInvoiceByDb.PurchaseDate,
                InitialMonthYear = fixedInvoiceByDb.InitialMonthYear,
                FynallyMonthYear = fixedInvoiceByDb.FynallyMonthYear,
                Category = fixedInvoiceByDb.Category,
                Value = fixedInvoiceByDb.Value,
                BestPayDay = fixedInvoiceByDb.BestPayDay,
                AdditionalMessage = fixedInvoiceByDb.AdditionalMessage,
                CreationDate = fixedInvoiceByDb.CreationDate,
                LastChangeDate = DateTime.Now
            };

            await _fixedInvoiceRepository.Edit(fixedInvoice);
        }

        private static void EditFixedInvoice(FixedInvoice fixedInvoice)
        {
            fixedInvoice.LastChangeDate = DateTime.Now;
        }

        private static BillToPay? GetLastRegistrationBillToPay(IList<BillToPay> billsToPay)
        {
            if (billsToPay == null)
            {
                return null;
            }

            var result = billsToPay
                .OrderByDescending(billToPay => billToPay.DueDate)
                .FirstOrDefault()!;

            return result;
        }
    }
}
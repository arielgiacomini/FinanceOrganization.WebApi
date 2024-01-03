using Domain.Entities;
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
        private readonly IBillToPayRepository _walletToPayRepository;
        private const string CARTAO_CREDITO = "Cartão de Crédito";
        private const string CARTAO_VALE_ALIMENTACAO = "Cartão VA";
        private const string CARTAO_VALE_REFEICAO = "Cartão VR";
        private const string FREQUENCIA_MENSAL_RECORRENTE = "Mensal:Recorrente";
        private const int DIA_MAXIMO_CONTA_MES_SUBSEQUENTE = 24;
        private const int DIA_VENCIMENTO_CARTAO_CREDITO = 9;

        public CreateBillToPayEventHandler(
            ILogger logger,
            IOptions<BillToPayOptions> options,
            IFixedInvoiceRepository fixedInvoiceRepository,
            IBillToPayRepository walletToPayRepository)
        {
            _logger = logger;
            _fixedInvoiceRepository = fixedInvoiceRepository;
            _billToPayOptions = options.Value;
            _walletToPayRepository = walletToPayRepository;
        }

        public async Task Handle(CreateBillToPayEventInput input)
        {
            var fixedInvoices = await _fixedInvoiceRepository.GetByAll();

            foreach (var fixedInvoice in fixedInvoices)
            {
                var json = JsonSerializeUtils.Serialize(fixedInvoice);

                _logger.Information("Objeto FixedInvoice que será processado: {@json}", json);

                var billsToPay = await _walletToPayRepository.GetBillToPayByFixedInvoiceId(fixedInvoice.Id);

                var lastBillToPay = GetLastRegistrationBillToPay(billsToPay);

                StartRegistration(fixedInvoice, lastBillToPay);

                Thread.Sleep(1000);
            }
        }

        /// <summary>
        /// Responsável de verificar se o registro no banco de dados está elegível para cadastro agora.
        /// </summary>
        /// <param name="fixedInvoice"></param>
        /// <returns></returns>
        private void StartRegistration(FixedInvoice fixedInvoice, BillToPay? billToPay)
        {
            _ = LogicRegistration(fixedInvoice, billToPay);
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

            if (fixedInvoice.Account == CARTAO_CREDITO || (fixedInvoice.BestPayDay <= DIA_MAXIMO_CONTA_MES_SUBSEQUENTE
                && (fixedInvoice.Account != CARTAO_CREDITO && fixedInvoice.Account != CARTAO_VALE_REFEICAO && fixedInvoice.Account != CARTAO_VALE_ALIMENTACAO)))
            {
                addMonthForDueDate = true;
            }

            Dictionary<string, DateTime> purchasesDate = new();
            bool considerPurchase = false;

            if (fixedInvoice.Account == CARTAO_CREDITO)
            {
                // Cartão de Crédito

                fixedInvoice.BestPayDay = DIA_VENCIMENTO_CARTAO_CREDITO;

                if (fixedInvoice.Frequence == FREQUENCIA_MENSAL_RECORRENTE)
                {
                    // Crédito Recorrente.
                    considerPurchase = true;
                    purchasesDate = DateServiceUtils.GetNextYearMonthAndDateTime(fixedInvoice.PurchaseDate, qtdMonthAdd, null, true);
                }
            }

            if (fixedInvoice.InitialMonthYear == fixedInvoice.FynallyMonthYear)
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
                }
                else
                {
                    purchase = fixedInvoice.PurchaseDate;
                }

                listBillToPay.Add(MapBillToPay(null, fixedInvoice, nextMonth.Value, nextMonth.Key, purchase));
            }

            await _walletToPayRepository.Save(listBillToPay);
        }

        private async Task LogicByBillToPay(BillToPay billToPay)
        {
            var result = await _fixedInvoiceRepository.GetById(billToPay.IdFixedInvoice);

            if (result?.FynallyMonthYear?.Length > 0)
            {
                return;
            }

            List<BillToPay> listBillToPay = new();

            var totalMonths = DateServiceUtils.GetMonthsByDateTime(billToPay.DueDate, null);

            var qtdMonthAdd = GetMonthsAdd(totalMonths, _billToPayOptions.HowManyMonthForward);

            if (qtdMonthAdd <= 0 || totalMonths > _billToPayOptions.HowManyMonthForward)
            {
                return;
            }

            var nextMonthYearToRegister = DateServiceUtils
                .GetNextYearMonthAndDateTime(
                billToPay.DueDate, qtdMonthAdd, null, false);

            foreach (var nextMonth in nextMonthYearToRegister!)
            {
                listBillToPay.Add(MapBillToPay(billToPay, null, nextMonth.Value, nextMonth.Key));
            }

            await _walletToPayRepository.Save(listBillToPay);
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
                    Value = billToPay.Value,
                    PurchaseDate = purchaseDate,
                    DueDate = dueDate,
                    YearMonth = yearMonth,
                    Frequence = billToPay.Frequence,
                    PayDay = null,
                    HasPay = false,
                    CreationDate = DateTime.Now,
                    LastChangeDate = null
                };
            }
            else
            {
                return new BillToPay()
                {
                    Id = Guid.NewGuid(),
                    IdFixedInvoice = fixedInvoice!.Id,
                    Account = fixedInvoice.Account,
                    Name = fixedInvoice.Name,
                    Category = fixedInvoice.Category,
                    Value = fixedInvoice.Value,
                    PurchaseDate = purchaseDate,
                    DueDate = dueDate,
                    YearMonth = yearMonth,
                    Frequence = fixedInvoice.Frequence,
                    PayDay = null,
                    HasPay = false,
                    CreationDate = DateTime.Now,
                    LastChangeDate = null
                };
            }
        }

        public BillToPay? GetLastRegistrationBillToPay(IList<BillToPay> billsToPay)
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
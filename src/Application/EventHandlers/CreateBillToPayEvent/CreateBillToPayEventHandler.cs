using Application.Feature.Payment.AdjustPayament;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Options;
using Domain.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.EventHandlers.CreateBillToPayEvent
{
    public class CreateBillToPayEventHandler : ICreateBillToPayEventHandler
    {
        private readonly ILogger<CreateBillToPayEventHandler> _logger;
        private readonly BillToPayOptions _billToPayOptions;
        private readonly IBillToPayRegistrationRepository _billToPayRegistrationRepository;
        private readonly IBillToPayRepository _billToPayRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IPaymentAdjustmentHandler _paymentAdjustmentHandler;


        private const string FREQUENCIA_MENSAL_RECORRENTE = "Mensal:Recorrente";
        private const string FREQUENCIA_LIVRE = "Livre";
        private const string TIPO_REGISTRO_FATURA_FIXA = "Conta/Fatura Fixa";
        private const string TIPO_REGISTRO_COMPRA_LIVRE = "Compra Livre";
        private const int QUANTOS_DIAS_PASSADOS_CONSIDERAR = -1;

        public CreateBillToPayEventHandler(
            ILogger<CreateBillToPayEventHandler> logger,
            IOptions<BillToPayOptions> options,
            IBillToPayRegistrationRepository billToPayRegistrationRepository,
            IBillToPayRepository billToPayRepository,
            IAccountRepository accountRepository,
            IPaymentAdjustmentHandler paymentAdjustmentHandler)
        {
            _logger = logger;
            _billToPayRegistrationRepository = billToPayRegistrationRepository;
            _billToPayOptions = options.Value;
            _billToPayRepository = billToPayRepository;
            _accountRepository = accountRepository;
            _paymentAdjustmentHandler = paymentAdjustmentHandler;
        }

        public async Task Handle(CreateBillToPayEventInput input)
        {
            var participants = await _billToPayRegistrationRepository
                .GetOnlyOldRecordsAndParticipants(QUANTOS_DIAS_PASSADOS_CONSIDERAR, TIPO_REGISTRO_FATURA_FIXA);

            foreach (var toProcess in participants)
            {
                try
                {
                    var json = JsonSerializeUtils.Serialize(toProcess);

                    _logger.LogInformation("Rotina que cria novas contas a pagar. Conta a ser processada: {@json}", json);

                    var billsToPay = await _billToPayRepository.GetBillToPayByBillToPayRegistrationId(toProcess.Id);

                    var lastBillToPay = GetLastRegistrationBillToPay(billsToPay);

                    await StartRegistration(toProcess, lastBillToPay);

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro na rotina que cria novas contas a pagar. Erro: {Message}", ex.Message);
                }
            }
        }

        /// <summary>
        /// Responsável de verificar se o registro no banco de dados está elegível para cadastro agora.
        /// </summary>
        /// <param name="billToPayRegistration"></param>
        /// <returns></returns>
        private async Task StartRegistration(BillToPayRegistration billToPayRegistration, BillToPay? billToPay)
        {
            await LogicRegistration(billToPayRegistration, billToPay);
        }

        /// <summary>
        /// Contempla a lógica para cadastro de novas contas a pagar de acordo com o tempo que ela se encontra.
        /// </summary>
        /// <param name="billToPayRegistration"></param>
        public async Task LogicRegistration(BillToPayRegistration billToPayRegistration, BillToPay? billToPay)
        {
            if (billToPay != null)
            {
                await LogicByBillToPay(billToPay);
            }
            else
            {
                await LogicByBillToPayRegistration(billToPayRegistration);
            }
        }

        /// <summary>
        /// Caso seja um cadastro efetuado recente, ao chegar no time para cadastro ele seguirá por aqui.
        /// </summary>
        /// <param name="billToPayRegistration"></param>
        /// <returns></returns>
        private async Task LogicByBillToPayRegistration(BillToPayRegistration billToPayRegistration)
        {
            if (string.IsNullOrEmpty(billToPayRegistration.Account))
            {
                _logger.LogError("Conta vazia ou null. {@json}", billToPayRegistration);
                return;
            }

            List<BillToPay> listBillToPay = new();

            var totalMonths = DateServiceUtils.GetMonthsByDateTime(
                DateServiceUtils.GetDateTimeByYearMonthBrazilian(billToPayRegistration.InitialMonthYear),
                DateServiceUtils.GetDateTimeByYearMonthBrazilian(billToPayRegistration.FynallyMonthYear));

            int qtdMonthAdd;

            if (billToPayRegistration.FynallyMonthYear == null)
            {
                qtdMonthAdd = GetMonthsAdd(totalMonths, _billToPayOptions.HowManyMonthForward);
            }
            else
            {
                qtdMonthAdd = totalMonths;
            }

            if (qtdMonthAdd <= 0 || CheckConsiderParamHowManyMonthForward(billToPayRegistration, totalMonths))
            {
                _logger.LogInformation("Regra de quantidade de meses solicitados para cadastro: {TotalMonths} " +
                    "da conta configurada é superior ao configurado nesta aplicação que " +
                    "são {HowManyMonthForward}. Ou a quantidade de meses para adicionar " +
                    "for inferior ou igual a zero: {QtdMonthAdd}", totalMonths, _billToPayOptions.HowManyMonthForward, qtdMonthAdd);
                return;
            }

            bool addMonthForDueDate = false;
            var account = await _accountRepository.GetAccountByName(billToPayRegistration.Account);

            if (account == null)
            {
                _logger.LogInformation("A conta que foi pesquisada para cria as contas futuras não foi encontrada. Pesquisado: {Account}", billToPayRegistration.Account);
                return;
            }

            if (account.IsCreditCard)
            {
                addMonthForDueDate = true;
            }

            Dictionary<string, DateTime> purchasesDate = new();
            bool considerPurchase = false;

            if (account.IsCreditCard)
            {
                // Cartão de Crédito
                billToPayRegistration.BestPayDay = account.DueDate!.Value;

                if (billToPayRegistration.Frequence == FREQUENCIA_MENSAL_RECORRENTE)
                {
                    // Crédito Recorrente.
                    considerPurchase = true;
                    purchasesDate = DateServiceUtils
                        .GetNextYearMonthAndDateTime(billToPayRegistration.PurchaseDate, qtdMonthAdd, null, true);
                }
            }

            if (billToPayRegistration.InitialMonthYear == billToPayRegistration.FynallyMonthYear
                && DateServiceUtils.IsCurrentMonth(billToPayRegistration.InitialMonthYear, billToPayRegistration.FynallyMonthYear))
            {
                qtdMonthAdd = 0;
            }

            var nextMonthYearToRegister = DateServiceUtils
                .GetNextYearMonthAndDateTime(
                null, qtdMonthAdd, billToPayRegistration.BestPayDay,
                DateServiceUtils.IsCurrentMonth(billToPayRegistration.InitialMonthYear, billToPayRegistration.FynallyMonthYear), addMonthForDueDate);

            billToPayRegistration.LastChangeDate = DateTime.Now;

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
                    purchase = billToPayRegistration.PurchaseDate;
                }

                var consideredPaid = await _paymentAdjustmentHandler
                    .ConsideredPaid(billToPayRegistration.Account, billToPayRegistration.RegistrationType);

                listBillToPay.Add(MapBillToPay(null, billToPayRegistration, consideredPaid, nextMonth.Value, nextMonth.Key, purchase));
            }

            await _paymentAdjustmentHandler
                .Handle(CreatePaymentAdjustment(billToPayRegistration, qtdMonthAdd, account, nextMonthYearToRegister), new CancellationToken());

            await _billToPayRegistrationRepository.Edit(billToPayRegistration);

            await _billToPayRepository.SaveRange(listBillToPay);
        }

        private static PaymentAdjustmentInput CreatePaymentAdjustment(BillToPayRegistration billToPayRegistration, int qtdMonthAdd, Account account, Dictionary<string, DateTime> nextMonthYearToRegister)
        {
            return new PaymentAdjustmentInput
            {
                AccountType = Domain.Entities.Enums.AccountType.ContaAPagar,
                Name = billToPayRegistration.Name,
                ConsideredPaid = account.ConsiderPaid.Value,
                RegistrationType = billToPayRegistration.RegistrationType,
                QuantityMonthsAdd = qtdMonthAdd,
                Frequence = billToPayRegistration.Frequence,
                Account = billToPayRegistration.Account,
                Category = billToPayRegistration.Category,
                YearMonth = nextMonthYearToRegister.First().Key,
                PaymentDate = DateTime.Now.Date,
                Value = billToPayRegistration.Value,
                AdditionalMessage = billToPayRegistration.AdditionalMessage,
                LastChangeDate = DateTime.Now
            };
        }

        private bool CheckConsiderParamHowManyMonthForward(BillToPayRegistration billToPayRegistration, int totalMonths)
        {
            if (string.IsNullOrEmpty(billToPayRegistration.FynallyMonthYear))
            {
                return totalMonths > _billToPayOptions.HowManyMonthForward;
            }
            else
            {
                _logger.LogInformation("O parâmetro configurado na aplicação para quantidade de meses futuros é de {HowManyMonthForward} meses" +
                    "porém a conta que chegou: {Name} trata-se de uma conta com mês ano final em {FynallyMonthYear} portando, " +
                    "será ignorado o parâmetro e respeitado quando finaliza a conta.", _billToPayOptions.HowManyMonthForward,
                    billToPayRegistration.Name, billToPayRegistration.FynallyMonthYear
                 );
                return false;
            }
        }

        private static bool IsNotFaturaFixa(BillToPayRegistration billToPayRegistration)
        {
            return billToPayRegistration.RegistrationType != TIPO_REGISTRO_FATURA_FIXA;
        }

        private async Task LogicByBillToPay(BillToPay billToPay)
        {
            List<BillToPay> listBillToPay = new();

            var billToPayRegistration = await _billToPayRegistrationRepository
                .GetById(billToPay.IdBillToPayRegistration);

            var account = await _accountRepository
                .GetAccountByName(billToPayRegistration?.Account!);

            if (billToPayRegistration?.FynallyMonthYear?.Length > 0)
            {
                var lastDueDateBillToPay = billToPay.DueDate;

                var MustBeRegisteredBy = DateServiceUtils
                    .GetDateTimeByYearMonthBrazilian(billToPayRegistration.FynallyMonthYear);

                if (lastDueDateBillToPay < MustBeRegisteredBy)
                {
                    var getMonthsByDateTime = DateServiceUtils
                        .GetMonthsByDateTime(billToPay.DueDate, MustBeRegisteredBy);

                    var nextTotalMonthYear = DateServiceUtils
                        .GetNextYearMonthAndDateTime(
                        billToPay.DueDate, getMonthsByDateTime, null, false);

                    foreach (var nextMonth in nextTotalMonthYear!)
                    {
                        listBillToPay.Add(MapBillToPay(billToPay, null, account.ConsiderPaid.Value, nextMonth.Value, nextMonth.Key));
                    }

                    await _billToPayRepository.SaveRange(listBillToPay);
                }

                await EditBillToPayRegistrationNow(billToPay);

                return;
            }

            var totalMonths = DateServiceUtils
                .GetMonthsByDateTime(billToPay.DueDate, null);

            var qtdMonthAdd = GetMonthsAdd(totalMonths, _billToPayOptions.HowManyMonthForward);

            if (qtdMonthAdd <= 0 || totalMonths > _billToPayOptions.HowManyMonthForward)
            {
                await EditBillToPayRegistrationNow(billToPay);
                return;
            }

            var nextMonthYearToRegister = DateServiceUtils
                .GetNextYearMonthAndDateTime(
                billToPay.DueDate, qtdMonthAdd, null, false);

            foreach (var nextMonth in nextMonthYearToRegister!)
            {
                listBillToPay.Add(MapBillToPay(billToPay, null, account.ConsiderPaid.Value, nextMonth.Value, nextMonth.Key));
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
            BillToPay? billToPay, BillToPayRegistration? billToPayRegistration, bool consideredPaid,
            DateTime dueDate, string yearMonth, DateTime? purchaseDate = null)
        {
            if (billToPay is not null)
            {
                return new BillToPay()
                {
                    Id = Guid.NewGuid(),
                    IdBillToPayRegistration = billToPay!.IdBillToPayRegistration,
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
                    IdBillToPayRegistration = billToPayRegistration!.Id,
                    Account = billToPayRegistration.Account,
                    Name = billToPayRegistration.Name,
                    Category = billToPayRegistration.Category,
                    RegistrationType = billToPayRegistration.RegistrationType,
                    Value = billToPayRegistration.Value,
                    PurchaseDate = purchaseDate,
                    DueDate = dueDate,
                    YearMonth = yearMonth,
                    Frequence = billToPayRegistration.Frequence,
                    PayDay = null,
                    HasPay = false,
                    AdditionalMessage = billToPayRegistration.AdditionalMessage,
                    CreationDate = DateTime.Now,
                    LastChangeDate = new DateTime(1753, 01, 01, 12, 0, 0, DateTimeKind.Local)
                };

                if (consideredPaid)
                {
                    newBillToPay.HasPay = true;
                    newBillToPay.PayDay = purchaseDate.ToString();
                    newBillToPay.LastChangeDate = DateTime.Now;
                }

                return newBillToPay;
            }
        }

        public async Task EditBillToPayRegistrationNow(BillToPay billToPay)
        {
            var billToPayRegistrationByDb = await _billToPayRegistrationRepository.GetById(billToPay.IdBillToPayRegistration);

            if (billToPayRegistrationByDb == null)
            {
                return;
            }

            var billToPayRegistration = new BillToPayRegistration()
            {
                Id = billToPayRegistrationByDb!.Id,
                Name = billToPayRegistrationByDb.Name,
                Account = billToPayRegistrationByDb.Account,
                Frequence = billToPayRegistrationByDb.Frequence,
                RegistrationType = billToPayRegistrationByDb.RegistrationType,
                PurchaseDate = billToPayRegistrationByDb.PurchaseDate,
                InitialMonthYear = billToPayRegistrationByDb.InitialMonthYear,
                FynallyMonthYear = billToPayRegistrationByDb.FynallyMonthYear,
                Category = billToPayRegistrationByDb.Category,
                Value = billToPayRegistrationByDb.Value,
                BestPayDay = billToPayRegistrationByDb.BestPayDay,
                AdditionalMessage = billToPayRegistrationByDb.AdditionalMessage,
                CreationDate = billToPayRegistrationByDb.CreationDate,
                LastChangeDate = DateTime.Now
            };

            await _billToPayRegistrationRepository.Edit(billToPayRegistration);
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
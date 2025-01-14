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
        private readonly IBillToPayRegistrationRepository _billToPayRegistrationRepository;
        private readonly IBillToPayRepository _billToPayRepository;
        private readonly IAccountRepository _accountRepository;
        private const string FREQUENCIA_MENSAL_RECORRENTE = "Mensal:Recorrente";
        private const string FREQUENCIA_MENSAL = "Mensal";
        private const string FREQUENCIA_LIVRE = "Livre";
        private const string TIPO_REGISTRO_FATURA_FIXA = "Conta/Fatura Fixa";
        private const string TIPO_REGISTRO_COMPRA_LIVRE = "Compra Livre";
        private const int QUANTOS_DIAS_PASSADOS_CONSIDERAR = -1;

        public CreateBillToPayEventHandler(
            ILogger logger,
            IOptions<BillToPayOptions> options,
            IBillToPayRegistrationRepository billToPayRegistrationRepository,
            IBillToPayRepository billToPayRepository,
            IAccountRepository accountRepository)
        {
            _logger = logger;
            _billToPayRegistrationRepository = billToPayRegistrationRepository;
            _billToPayOptions = options.Value;
            _billToPayRepository = billToPayRepository;
            _accountRepository = accountRepository;
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

                    _logger.Information("Objeto BillToPayRegistration que será processado: {@json}", json);

                    var billsToPay = await _billToPayRepository.GetBillToPayByBillToPayRegistrationId(toProcess.Id);

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
                _logger.Information("O nome da conta está inválido.");
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

            if (qtdMonthAdd <= 0 || totalMonths > _billToPayOptions.HowManyMonthForward)
            {
                return;
            }

            bool addMonthForDueDate = false;
            var account = await _accountRepository.GetAccountByName(billToPayRegistration.Account);

            if (account == null)
            {
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

                listBillToPay.Add(MapBillToPay(null, billToPayRegistration, account, nextMonth.Value, nextMonth.Key, purchase));
            }

            await DebitFixedAccount(billToPayRegistration, qtdMonthAdd);

            EditBillToPayRegistration(billToPayRegistration);

            await _billToPayRegistrationRepository.Edit(billToPayRegistration);

            await _billToPayRepository.SaveRange(listBillToPay);
        }

        private async Task DebitFixedAccount(BillToPayRegistration billToPayRegistration, int qtdMonthAdd)
        {
            if (IsValidToDebit(billToPayRegistration, qtdMonthAdd))
            {
                var descontar = await _billToPayRepository
                    .GetByYearMonthCategoryAndRegistrationType(
                    billToPayRegistration.InitialMonthYear!, billToPayRegistration.Category!, TIPO_REGISTRO_FATURA_FIXA);

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

                    descontar.Value -= billToPayRegistration.Value;
                    descontar.AdditionalMessage += $"Retirado: [R$ {billToPayRegistration.Value}] em [{DateTime.Now.Date}] do valor que estava: [R$ {valueOld}] pela seguinte conta: [{billToPayRegistration.Name}] | ";

                    var edited = await _billToPayRepository.Edit(descontar);

                    if (edited == 1)
                    {
                        _logger.Information($"A conta relacionada de Id [{descontar.Id}] foi descontado valores com base no gasto da conta de Id [{billToPayRegistration.Id}] com a seguinte informação [{descontar.AdditionalMessage}]");
                    }
                }
            }
        }

        private static bool IsValidToDebit(BillToPayRegistration billToPayRegistration, int qtdMonthAdd)
        {
            var isTrue = billToPayRegistration.RegistrationType == TIPO_REGISTRO_COMPRA_LIVRE
                      && qtdMonthAdd == 0
                      && billToPayRegistration.Frequence == FREQUENCIA_LIVRE;

            return isTrue;
        }

        public static bool EnterPaid(BillToPayRegistration? billToPayRegistration, Account account)
        {
            bool considerPaid = false;

            if (billToPayRegistration == null)
            {
                return considerPaid;
            }

            if (account.ConsiderPaid.HasValue
                && account.ConsiderPaid.Value
                && IsNotFaturaFixa(billToPayRegistration))
            {
                considerPaid = true;
            }

            return considerPaid;
        }

        private static bool IsNotFaturaFixa(BillToPayRegistration billToPayRegistration)
        {
            return billToPayRegistration.RegistrationType != TIPO_REGISTRO_FATURA_FIXA;
        }

        private async Task LogicByBillToPay(BillToPay billToPay)
        {
            var result = await _billToPayRegistrationRepository.GetById(billToPay.IdBillToPayRegistration);

            if (result?.FynallyMonthYear?.Length > 0)
            {
                await EditBillToPayRegistrationNow(billToPay);
                return;
            }

            List<BillToPay> listBillToPay = new();

            var totalMonths = DateServiceUtils.GetMonthsByDateTime(billToPay.DueDate, null);

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
            BillToPay? billToPay, BillToPayRegistration? billToPayRegistration, Account account, DateTime dueDate, string yearMonth, DateTime? purchaseDate = null)
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

                if (EnterPaid(billToPayRegistration, account))
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

        private static void EditBillToPayRegistration(BillToPayRegistration billToPayRegistration)
        {
            billToPayRegistration.LastChangeDate = DateTime.Now;
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
using Domain.Entities;
using Domain.Interfaces;
using Domain.Options;
using Domain.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Application.EventHandlers.CreateCashReceivableEvent
{
    public class CreateCashReceivableEventHandler : ICreateCashReceivableEventHandler
    {
        private const int QUANTOS_DIAS_PASSADOS_CONSIDERAR = -1;
        private const string FREQUENCIA_MENSAL = "Mensal";
        private const string TIPO_REGISTRO_FATURA_FIXA = "Conta Fixa";
        private const bool IS_CASH_RECEIVABLE = true;

        private readonly ILogger<CreateCashReceivableEventHandler> _logger;
        private readonly CashReceivableOptions _cashReceivableOptions;
        private readonly IAccountRepository _accountRepository;
        private readonly ICashReceivableRegistrationRepository _cashReceivableRegistrationRepository;
        private readonly ICashReceivableRepository _cashReceivableRepository;

        public CreateCashReceivableEventHandler(
            ILogger<CreateCashReceivableEventHandler> logger,
            IOptions<CashReceivableOptions> options,
            IAccountRepository accountRepository,
            ICashReceivableRegistrationRepository cashReceivableRegistrationRepository,
            ICashReceivableRepository cashReceivableRepository)
        {
            _logger = logger;
            _cashReceivableOptions = options.Value;
            _accountRepository = accountRepository;
            _cashReceivableRegistrationRepository = cashReceivableRegistrationRepository;
            _cashReceivableRepository = cashReceivableRepository;
        }

        public async Task Handle(CreateCashReceivableEventInput input)
        {
            var participants = await _cashReceivableRegistrationRepository
                .GetOnlyOldRecordsAndParticipants(QUANTOS_DIAS_PASSADOS_CONSIDERAR, FREQUENCIA_MENSAL);

            foreach (var cashReceivableRegistration in participants)
            {
                var json = JsonSerializeUtils.Serialize(cashReceivableRegistration);

                _logger.LogInformation("Objeto BillToPayRegistration que será processado: {@json}", json);

                var cashReceivableList = await _cashReceivableRepository.GetCashReceivableRegistrationId(cashReceivableRegistration.Id);

                await StartRegistration(cashReceivableRegistration, cashReceivableList);
            }
        }

        public async Task StartRegistration(CashReceivableRegistration registration, IList<CashReceivable> cashReceivableList)
        {
            if (cashReceivableList == null || cashReceivableList.Count <= 0)
            {
                await LogicByCashReceivableRegistration(registration);
            }
        }

        public async Task LogicByCashReceivableRegistration(CashReceivableRegistration registration)
        {
            if (string.IsNullOrEmpty(registration.Account))
            {
                _logger.LogInformation("O nome da conta está inválido.");
                return;
            }

            var account = await _accountRepository.GetAccountByName(registration.Account);

            if (account == null)
            {
                _logger.LogInformation("A conta que foi pesquisada para cria as contas futuras não foi encontrada. Pesquisado: {Account}", registration.Account);
                return;
            }

            List<CashReceivable> listCashReceivable = new();

            var totalMonths = DateServiceUtils.GetMonthsByDateTime(
                DateServiceUtils.GetDateTimeByYearMonthBrazilian(registration.InitialMonthYear),
                DateServiceUtils.GetDateTimeByYearMonthBrazilian(registration.FynallyMonthYear));

            int qtdMonthAdd;

            if (registration.FynallyMonthYear == null)
            {
                qtdMonthAdd = GetMonthsAdd(totalMonths, _cashReceivableOptions.HowManyMonthForward);
            }
            else
            {
                qtdMonthAdd = totalMonths;
            }

            if (qtdMonthAdd <= 0 || CheckConsiderParamHowManyMonthForward(registration, totalMonths))
            {
                _logger.LogInformation("Regra de quantidade de meses solicitados para cadastro: {TotalMonths} " +
                    "da conta configurada é superior ao configurado nesta aplicação que " +
                    "são {HowManyMonthForward}. Ou a quantidade de meses para adicionar " +
                    "for inferior ou igual a zero: {QtdMonthAdd}", totalMonths, _cashReceivableOptions.HowManyMonthForward, qtdMonthAdd);
                return;
            }

            bool addMonthForDueDate = false;

            Dictionary<string, DateTime> purchasesDate = new();
            bool considerPurchase = false;

            if (registration.InitialMonthYear == registration.FynallyMonthYear
                && DateServiceUtils.IsCurrentMonth(registration.InitialMonthYear, registration.FynallyMonthYear))
            {
                qtdMonthAdd = 0;
            }

            var nextMonthYearToRegister = DateServiceUtils
                .GetNextYearMonthAndDateTime(
                null, qtdMonthAdd, registration.BestReceivingDay,
                DateServiceUtils.IsCurrentMonth(registration.InitialMonthYear, registration.FynallyMonthYear), addMonthForDueDate);

            foreach (var nextMonth in nextMonthYearToRegister!)
            {
                DateTime? agreementDate;
                if (considerPurchase)
                {
                    agreementDate = purchasesDate.GetValueOrDefault(nextMonth.Key);

                    if (agreementDate == DateTime.MinValue)
                    {
                        continue;
                    }
                }
                else
                {
                    agreementDate = registration.AgreementDate;
                }

                listCashReceivable.Add(MapCashReceivable(null, registration, account, nextMonth.Value, nextMonth.Key, agreementDate));
            }

            registration.LastChangeDate = DateTime.Now;

            await _cashReceivableRegistrationRepository.Edit(registration);

            await _cashReceivableRepository.SaveRange(listCashReceivable);
        }

        public static CashReceivable MapCashReceivable(
                    CashReceivable cashReceivable, CashReceivableRegistration registration, Account account, DateTime dueDate, string yearMonth, DateTime? agreementDate = null)
        {
            if (cashReceivable is not null)
            {
                return new CashReceivable()
                {
                    Id = Guid.NewGuid(),
                    IdCashReceivableRegistration = cashReceivable!.IdCashReceivableRegistration,
                    Account = cashReceivable.Account,
                    Name = cashReceivable.Name,
                    Category = cashReceivable.Category,
                    RegistrationType = cashReceivable.RegistrationType,
                    Value = cashReceivable.Value,
                    ManipulatedValue = cashReceivable.ManipulatedValue,
                    AgreementDate = agreementDate,
                    DueDate = dueDate,
                    YearMonth = yearMonth,
                    Frequence = cashReceivable.Frequence,
                    DateReceived = null,
                    HasReceived = false,
                    AdditionalMessage = cashReceivable.AdditionalMessage,
                    CreationDate = DateTime.Now,
                    LastChangeDate = new DateTime(1753, 01, 01, 12, 0, 0, DateTimeKind.Local)
                };
            }
            else
            {
                var newBillToPay = new CashReceivable()
                {
                    Id = Guid.NewGuid(),
                    IdCashReceivableRegistration = registration!.Id,
                    Account = registration.Account,
                    Name = registration.Name,
                    Category = registration.Category,
                    RegistrationType = registration.RegistrationType,
                    Value = registration.Value,
                    ManipulatedValue = registration.Value,
                    AgreementDate = agreementDate,
                    DueDate = dueDate,
                    YearMonth = yearMonth,
                    Frequence = registration.Frequence,
                    DateReceived = null,
                    HasReceived = false,
                    AdditionalMessage = registration.AdditionalMessage,
                    CreationDate = DateTime.Now,
                    LastChangeDate = new DateTime(1753, 01, 01, 12, 0, 0, DateTimeKind.Local)
                };

                if (!IS_CASH_RECEIVABLE)
                {
                    if (EnterReceived(registration, account))
                    {
                        newBillToPay.HasReceived = true;
                        newBillToPay.DateReceived = agreementDate.ToString();
                        newBillToPay.LastChangeDate = DateTime.Now;
                    }
                }

                return newBillToPay;
            }
        }

        public static bool EnterReceived(CashReceivableRegistration? registration, Account account)
        {
            bool considerPaid = false;

            if (registration == null)
            {
                return considerPaid;
            }

            if (account.ConsiderPaid.HasValue
                && account.ConsiderPaid.Value
                && IsNotFaturaFixa(registration))
            {
                considerPaid = true;
            }

            return considerPaid;
        }

        private static bool IsNotFaturaFixa(CashReceivableRegistration registration)
        {
            return registration.RegistrationType != TIPO_REGISTRO_FATURA_FIXA;
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

        private bool CheckConsiderParamHowManyMonthForward(CashReceivableRegistration registration, int totalMonths)
        {
            if (string.IsNullOrEmpty(registration.FynallyMonthYear))
            {
                return totalMonths > _cashReceivableOptions.HowManyMonthForward;
            }
            else
            {
                _logger.LogInformation("O parâmetro configurado na aplicação para quantidade de meses futuros é de {HowManyMonthForward} meses" +
                    "porém a conta que chegou: {Name} trata-se de uma conta com mês ano final em {FynallyMonthYear} portando, " +
                    "será ignorado o parâmetro e respeitado quando finaliza a conta.", _cashReceivableOptions.HowManyMonthForward,
                    registration.Name, registration.FynallyMonthYear
                 );
                return false;
            }
        }
    }
}
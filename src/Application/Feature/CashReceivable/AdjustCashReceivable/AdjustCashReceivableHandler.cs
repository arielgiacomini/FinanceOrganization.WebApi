using Domain.Interfaces;
using Microsoft.Extensions.Logging;


namespace Application.Feature.CashReceivable.AdjustCashReceivable
{
    public class AdjustCashReceivableHandler : IAdjustCashReceivableHandler
    {
        private readonly ILogger<AdjustCashReceivableHandler> _logger;
        private readonly ICashReceivableRepository _cashReceivableRepository;

        public AdjustCashReceivableHandler(ILogger<AdjustCashReceivableHandler> logger,
            ICashReceivableRepository cashReceivableRepository)
        {
            _logger = logger;
            _cashReceivableRepository = cashReceivableRepository;
        }

        public async Task Adjust<T>(T input) where T : class
        {
            var validate = Validator(input, out string accountValue, out string monthYearValue, out decimal valueValue);

            if (!validate)
            {
                _logger.LogError("Validation failed for input type {InputType}.", typeof(T).Name);
            }

            await CashReceivablePayment(accountValue, monthYearValue, valueValue);
        }

        private bool Validator<T>(T input, out string accountValue, out string monthYearValue, out decimal valueValue) where T : class
        {
            var accountProperty = typeof(T).GetProperty("Account");
            var monthYearProperty = typeof(T).GetProperty("InitialMonthYear");
            if (accountProperty == null)
            {
                monthYearProperty = typeof(T).GetProperty("YearMonth");
            }

            if (accountProperty == null || monthYearProperty == null)
            {
                _logger.LogError("The input type {InputType} does not contain the required properties 'Account' or 'InitialMonthYear'.", typeof(T).Name);
                accountValue = null;
                monthYearValue = null;
                valueValue = 0;
                return false;
            }

            accountValue = accountProperty.GetValue(input)?.ToString();
            monthYearValue = monthYearProperty.GetValue(input)?.ToString();
            if (string.IsNullOrEmpty(accountValue) || string.IsNullOrEmpty(monthYearValue))
            {
                _logger.LogError("The input object is missing required values for 'Account' or 'InitialMonthYear'.");
                accountValue = null;
                monthYearValue = null;
                valueValue = 0;
                return false;
            }

            valueValue = (decimal)typeof(T).GetProperty("Value")?.GetValue(input);
            return true;
        }

        private async Task<bool> CashReceivablePayment(string accountValue, string monthYearValue, decimal value)
        {
            var cashReceivable = await _cashReceivableRepository
                .GetByAccountAndMonthYear(accountValue, monthYearValue);

            if (cashReceivable == null)
            {
                return false;
            }

            var oldManipuledValue = cashReceivable.ManipulatedValue;
            var newManipulatedValue = oldManipuledValue - value;

            cashReceivable.ManipulatedValue = newManipulatedValue;
            cashReceivable.LastChangeDate = DateTime.Now;

            var accountsReceivableAdjusted = await _cashReceivableRepository
                .Edit(cashReceivable);

            if (accountsReceivableAdjusted == 1)
            {
                _logger.LogInformation("O valor manipulado da conta a receber: {Account} foi ajustado de: {OldManipulatedValue} para: {NewManipulatedValue} " +
                    "devido ao cadastro da conta a pagar: {BillToPayName} com o valor de: {BillToPayValue}.",
                    cashReceivable.Account, oldManipuledValue, newManipulatedValue, cashReceivable.Name, cashReceivable.Value);
            }

            return true;
        }
    }
}
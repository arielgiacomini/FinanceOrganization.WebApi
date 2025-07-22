using Domain.Interfaces;
using Microsoft.Extensions.Logging;


namespace Application.Feature.CashReceivableLogic
{
    public class AdjustCashReceivable : IAdjustCashReceivable
    {
        private readonly ILogger<AdjustCashReceivable> _logger;
        private readonly ICashReceivableRepository _cashReceivableRepository;

        public AdjustCashReceivable(ILogger<AdjustCashReceivable> logger,
            ICashReceivableRepository cashReceivableRepository)
        {
            _logger = logger;
            _cashReceivableRepository = cashReceivableRepository;
        }

        public async Task AdjustCashReceivableManipulatedValue<T>(T input) where T : class
        {
            bool flowControl = await SetManipuletedValue(input);
            if (!flowControl)
            {
                return;
            }
        }

        /// <summary>
        /// Mantém a lógica de ajuste do valor manipulado da conta a receber génerica.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<bool> SetManipuletedValue<T>(T input) where T : class
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
                return false;
            }

            var accountValue = accountProperty.GetValue(input)?.ToString();
            var monthYearValue = monthYearProperty.GetValue(input)?.ToString();

            if (string.IsNullOrEmpty(accountValue) || string.IsNullOrEmpty(monthYearValue))
            {
                _logger.LogError("The input object is missing required values for 'Account' or 'InitialMonthYear'.");
                return false;
            }

            var cashReceivable = await _cashReceivableRepository
                .GetByAccountAndMonthYear(accountValue, monthYearValue);

            if (cashReceivable == null)
            {
                return false;
            }

            var oldManipuledValue = cashReceivable.ManipulatedValue;
            var newManipulatedValue = oldManipuledValue - (decimal)typeof(T).GetProperty("Value")?.GetValue(input);

            cashReceivable.ManipulatedValue = newManipulatedValue;
            cashReceivable.LastChangeDate = DateTime.Now;

            var accountsReceivableAdjusted = await _cashReceivableRepository
                .Edit(cashReceivable);

            if (accountsReceivableAdjusted == 1)
            {
                _logger.LogInformation("O valor manipulado da conta a receber: {Account} foi ajustado de: {OldManipulatedValue} para: {NewManipulatedValue} " +
                    "devido ao cadastro da conta a pagar: {BillToPayName} com o valor de: {BillToPayValue}.",
                    cashReceivable.Account, oldManipuledValue, newManipulatedValue, typeof(T).GetProperty("Name")?.GetValue(input), typeof(T).GetProperty("Value")?.GetValue(input));
            }

            return true;
        }
    }
}
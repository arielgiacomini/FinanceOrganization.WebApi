using Domain.Interfaces;

namespace Application.Feature.CashReceivableRegistration.DisableCashReceivableRegistration
{
    public class DisableCashReceivableRegistrationValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(
            DisableCashReceivableRegistrationInput input,
            ICashReceivableRegistrationRepository cashReceivableRegistrationRepository)
        {
            return await CreateValidateBaseInput(input, cashReceivableRegistrationRepository);
        }

        public static async Task<Dictionary<string, string>> CreateValidateBaseInput(DisableCashReceivableRegistrationInput input,
            ICashReceivableRegistrationRepository cashReceivableRegistrationRepository)
        {
            Dictionary<string, string> validatorBase = new();

            return validatorBase;
        }
    }
}
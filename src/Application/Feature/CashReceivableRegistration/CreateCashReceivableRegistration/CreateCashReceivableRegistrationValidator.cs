using Domain.Interfaces;

namespace Application.Feature.CashReceivableRegistration.CreateCashReceivableRegistration
{
    public static class CreateCashReceivableRegistrationValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(
            CreateCashReceivableRegistrationInput input,
            ICashReceivableRegistrationRepository cashReceivablerepository)
        {
            return await CreateValidateBaseInput(input, cashReceivablerepository);
        }

        public static async Task<Dictionary<string, string>> CreateValidateBaseInput(CreateCashReceivableRegistrationInput input,
            ICashReceivableRegistrationRepository cashReceivableRepository)
        {
            Dictionary<string, string> validatorBase = new();

            return validatorBase;
        }
    }
}
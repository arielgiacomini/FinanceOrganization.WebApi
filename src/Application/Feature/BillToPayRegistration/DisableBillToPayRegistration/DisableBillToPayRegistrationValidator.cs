using Domain.Interfaces;

namespace Application.Feature.BillToPayRegistration.DisableBillToPayRegistration
{
    public static class DisableBillToPayRegistrationValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(
            DisableBillToPayRegistrationInput input,
            IBillToPayRegistrationRepository billToPayRegistrationRepository)
        {
            return await CreateValidateBaseInput(input, billToPayRegistrationRepository);
        }

        public static async Task<Dictionary<string, string>> CreateValidateBaseInput(DisableBillToPayRegistrationInput input,
            IBillToPayRegistrationRepository billToPayRegistrationRepository)
        {
            Dictionary<string, string> validatorBase = new();

            return validatorBase;
        }
    }
}
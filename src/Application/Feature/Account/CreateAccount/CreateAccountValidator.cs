using Domain.Interfaces;

namespace Application.Feature.Account.CreateAccount
{
    public class CreateAccountValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(CreateAccountInput input, IAccountRepository accountRepository)
        {
            return await CreateValidateBaseInput(input, accountRepository);
        }

        public static async Task<Dictionary<string, string>> CreateValidateBaseInput(CreateAccountInput input,
            IAccountRepository accountRepository)
        {
            Dictionary<string, string> validatorBase = new();

            if (input.CardNumber.Length > 4)
            {
                validatorBase.Add("[100]", "No campo CardNumber informar apenas os últimos 4 digitos do cartão por questões de segurança.");
            }

            return validatorBase;
        }
    }
}
using Domain.Interfaces;

namespace Application.Feature.Account.EditAccount
{
    public static class EditAccountValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(EditAccountInput input, IAccountRepository accountRepository)
        {
            return await EditValidateBaseInput(input, accountRepository);
        }

        public static async Task<Dictionary<string, string>> EditValidateBaseInput(EditAccountInput input,
            IAccountRepository accountRepository)
        {
            Dictionary<string, string> validatorBase = new();

            if (!string.IsNullOrWhiteSpace(input.CardNumber) && input.CardNumber.Length > 4)
            {
                validatorBase.Add("[100]", "No campo CardNumber informar apenas os últimos 4 digitos do cartão por questões de segurança.");
            }

            if (input.Colors != null &&
                (string.IsNullOrWhiteSpace(input.Colors.BackgroundColorHexadecimal) || string.IsNullOrWhiteSpace(input.Colors.FonteColorHexadecimal)))
            {
                validatorBase.Add("[101]", "Quando informado Colors, os campos BackgroundColorHexadecimal e FonteColorHexadecimal são obrigatórios.");
            }

            return validatorBase;
        }
    }
}

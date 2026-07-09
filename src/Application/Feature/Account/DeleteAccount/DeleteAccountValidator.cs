using Domain.Interfaces;

namespace Application.Feature.Account.DeleteAccount
{
    public static class DeleteAccountValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(DeleteAccountInput input, IAccountRepository accountRepository)
        {
            return await DeleteValidateBaseInput(input, accountRepository);
        }

        public static async Task<Dictionary<string, string>> DeleteValidateBaseInput(DeleteAccountInput input,
            IAccountRepository accountRepository)
        {
            Dictionary<string, string> validatorBase = new();

            var account = await accountRepository.GetById(input.Id);

            if (account == null)
            {
                validatorBase.Add("[100]", $"Não foi encontrada nenhuma conta para o Id informado: [{input.Id}]");
            }

            return validatorBase;
        }
    }
}

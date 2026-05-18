using Domain.Interfaces;

namespace Application.Feature.Wallet.EditWallet
{
    public static class EditWalletValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(
            EditWalletInput input,
            IWalletRepository walletRepository)
        {
            return await EditValidateBaseInput(input, walletRepository);
        }

        public static async Task<Dictionary<string, string>> EditValidateBaseInput(EditWalletInput input,
            IWalletRepository walletRepository)
        {
            Dictionary<string, string> validatorBase = new();

            return validatorBase;
        }
    }
}
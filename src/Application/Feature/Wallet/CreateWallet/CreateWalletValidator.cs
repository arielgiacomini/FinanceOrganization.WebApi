using Domain.Interfaces;

namespace Application.Feature.Wallet.CreateWallet
{
    public static class CreateWalletValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(
        CreateWalletInput input,
        IWalletRepository walletRepository)
        {
            return await CreateValidateBaseInput(input, walletRepository);
        }

        public static async Task<Dictionary<string, string>> CreateValidateBaseInput(CreateWalletInput input,
            IWalletRepository walletRepository)
        {
            Dictionary<string, string> validatorBase = new();

            return validatorBase;
        }
    }
}
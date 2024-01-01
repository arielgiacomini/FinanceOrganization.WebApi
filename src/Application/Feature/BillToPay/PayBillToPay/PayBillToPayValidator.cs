using Domain.Interfaces;

namespace Application.Feature.BillToPay.PayBillToPay
{
    public static class PayBillToPayValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(
            PayBillToPayInput input,
            IWalletToPayRepository walletToPayRepository)
        {
            return await CreateValidateBaseInput(input, walletToPayRepository);
        }

        private static async Task<Dictionary<string, string>> CreateValidateBaseInput(PayBillToPayInput input,
            IWalletToPayRepository walletToPayRepository)
        {
            Dictionary<string, string> validatorBase = new();

            var result = await walletToPayRepository.GetBillToPayById(input.Id);

            if (result == null)
            {
                validatorBase.Add("[29]", $"Não foi encontrado Conta a pagar pelo Id: {input.Id}");
            }

            return validatorBase;
        }
    }
}
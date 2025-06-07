using Domain.Interfaces;

namespace Application.Feature.BillToPay.EditBillToPay
{
    public static class EditBillToPayValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(
            EditBillToPayInput input,
            IBillToPayRepository walletToPayRepository)
        {
            return await CreateValidateBaseInput(input, walletToPayRepository);
        }

        private static async Task<Dictionary<string, string>> CreateValidateBaseInput(EditBillToPayInput input,
            IBillToPayRepository walletToPayRepository)
        {
            Dictionary<string, string> validatorBase = new();

            var result = await walletToPayRepository.GetBillToPayById(input.Id);

            if (result == null)
            {
                validatorBase.Add("[29]", $"Não foi encontrado a conta a pagar {input.Name} pelo Id: {input.Id}");
            }

            return validatorBase;
        }
    }
}
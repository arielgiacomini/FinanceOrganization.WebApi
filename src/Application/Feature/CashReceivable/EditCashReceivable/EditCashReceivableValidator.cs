using Domain.Interfaces;

namespace Application.Feature.CashReceivable.EditCashReceivable
{
    public static class EditCashReceivableValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(
            EditCashReceivableInput input,
            ICashReceivableRepository repository)
        {
            return await CreateValidateBaseInput(input, repository);
        }

        private static async Task<Dictionary<string, string>> CreateValidateBaseInput(EditCashReceivableInput input,
            ICashReceivableRepository repository)
        {
            Dictionary<string, string> validatorBase = new();

            var result = await repository.GetById(input.Id);

            if (result == null)
            {
                validatorBase.Add("[29]", $"Não foi encontrado a conta a pagar {input.Name} pelo Id: {input.Id}");
            }

            return validatorBase;
        }
    }
}
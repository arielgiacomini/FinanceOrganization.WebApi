using Domain.Interfaces;

namespace Application.Feature.CashReceivable.ReceiveCashReceivable
{
    public static class ReceiveCashReceivableValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(
            ReceiveCashReceivableInput input,
            ICashReceivableRepository cashReceivableRepository)
        {
            return await CreateValidateBaseInput(input, cashReceivableRepository);
        }

        private static async Task<Dictionary<string, string>> CreateValidateBaseInput(
            ReceiveCashReceivableInput input,
            ICashReceivableRepository cashReceivableRepository)
        {
            Dictionary<string, string> validatorBase = new();

            return validatorBase;
        }
    }
}
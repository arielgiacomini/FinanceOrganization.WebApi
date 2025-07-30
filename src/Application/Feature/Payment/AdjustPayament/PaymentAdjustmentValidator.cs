namespace Application.Feature.Payment.AdjustPayament
{
    public static class PaymentAdjustmentValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(
            PaymentAdjustmentInput input)
        {
            Dictionary<string, string> validatorBase = new();

            return await Task.FromResult(validatorBase);
        }
    }
}
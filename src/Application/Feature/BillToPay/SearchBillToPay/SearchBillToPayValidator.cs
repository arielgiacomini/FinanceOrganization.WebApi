namespace Application.Feature.BillToPay.SearchBillToPay
{
    public static class SearchBillToPayValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(
            SearchBillToPayInput input)
        {
            return await CreateValidateBaseInput(input);
        }

        public static async Task<Dictionary<string, string>> CreateValidateBaseInput(
            SearchBillToPayInput input)
        {
            Dictionary<string, string> validatorBase = new();

            return validatorBase;
        }
    }
}
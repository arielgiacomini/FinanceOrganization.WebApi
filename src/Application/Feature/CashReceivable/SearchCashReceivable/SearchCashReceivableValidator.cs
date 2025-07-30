namespace Application.Feature.CashReceivable.SearchCashReceivable
{
    public static class SearchCashReceivableValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(SearchCashReceivableInput input)
        {
            Dictionary<string, string> validatorBase = new();

            return await Task.FromResult(validatorBase);
        }
    }
}
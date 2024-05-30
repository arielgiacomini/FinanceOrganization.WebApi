namespace Application.Feature.BillToPay.SearchMonthlyAverageAnalysis
{
    public static class SearchMonthlyAverageAnalysisValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(
         SearchMonthlyAverageAnalysisInput input)
        {
            return await CreateValidateBaseInput(input);
        }

        public static async Task<Dictionary<string, string>> CreateValidateBaseInput(
            SearchMonthlyAverageAnalysisInput input)
        {
            Dictionary<string, string> validatorBase = new();

            return validatorBase;
        }
    }
}
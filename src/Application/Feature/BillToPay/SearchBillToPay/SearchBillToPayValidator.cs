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

            if (input.YearMonth == null)
            {
                if (input.IdFixedInvoices?.Length == 0 && input.Id?.Length == 0)
                {
                    validatorBase.Add("[01]", "Se não for informado um Mês/Ano um dos Ids [IdFixedInvoice ou Id] deve ser informado.");
                }
            }

            if (input.YearMonth != null)
            {
                if (input.IdFixedInvoices?.Length > 0 || input.Id?.Length > 0)
                {
                    validatorBase.Add("[02]", "Deve ser informado apenas o Mês/Ano");
                }
            }

            return validatorBase;
        }
    }
}
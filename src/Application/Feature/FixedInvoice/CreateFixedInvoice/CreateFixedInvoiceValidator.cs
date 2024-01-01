using Domain.Interfaces;

namespace Application.Feature.FixedInvoice.CreateFixedInvoice
{
    public static class CreateFixedInvoiceValidator
    {
        private const string CARTAO_CREDITO = "Cartão de Crédito";

        public static async Task<Dictionary<string, string>> ValidateInput(
            CreateFixedInvoiceInput input,
            IFixedInvoiceRepository fixedInvoiceRepository)
        {
            return await CreateValidateBaseInput(input, fixedInvoiceRepository);
        }

        public static async Task<Dictionary<string, string>> CreateValidateBaseInput(CreateFixedInvoiceInput input,
            IFixedInvoiceRepository fixedInvoiceRepository)
        {
            Dictionary<string, string> validatorBase = new();

            var fixedInvoice = await fixedInvoiceRepository.GetFixedInvoiceByName(input.Name);

            if (fixedInvoice != null && fixedInvoice.Account != CARTAO_CREDITO)
            {
                validatorBase.Add("[32]", $"Já existe uma conta a pagar cadastada com este nome {input.Name} verifique se está correto.");
            }

            return validatorBase;
        }
    }
}
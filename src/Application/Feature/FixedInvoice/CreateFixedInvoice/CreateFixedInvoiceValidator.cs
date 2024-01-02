using Domain.Interfaces;

namespace Application.Feature.FixedInvoice.CreateFixedInvoice
{
    public static class CreateFixedInvoiceValidator
    {
        private const string CARTAO_CREDITO = "Cartão de Crédito";

        public static async Task<Dictionary<string, string>> ValidateInput(
            CreateFixedInvoiceInput input,
            IFixedInvoiceRepository fixedInvoiceRepository,
            IBillToPayRepository billToPayRepository)
        {
            return await CreateValidateBaseInput(input, fixedInvoiceRepository, billToPayRepository);
        }

        public static async Task<Dictionary<string, string>> CreateValidateBaseInput(CreateFixedInvoiceInput input,
            IFixedInvoiceRepository fixedInvoiceRepository, IBillToPayRepository billToPayRepository)
        {
            Dictionary<string, string> validatorBase = new();

            var billToPay = await billToPayRepository.GetBillToPayByNameAndDueDate(input.Name!, input.InitialMonthYear!, input.Frequence!);

            if (billToPay != null && billToPay.Account != CARTAO_CREDITO)
            {
                validatorBase.Add("[32]", $"Já existe uma conta a pagar para este mesmo nome: {input.Name}, neste mesmo Ano/Mes: {input.InitialMonthYear} e nesta frequência: {input.Frequence}");
            }

            return validatorBase;
        }
    }
}
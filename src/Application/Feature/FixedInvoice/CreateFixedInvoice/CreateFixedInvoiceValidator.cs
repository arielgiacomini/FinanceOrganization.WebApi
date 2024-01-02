using Domain.Interfaces;

namespace Application.Feature.FixedInvoice.CreateFixedInvoice
{
    public static class CreateFixedInvoiceValidator
    {
        private const string CARTAO_CREDITO = "Cartão de Crédito";
        private const string CARTAO_VALE_ALIMENTACAO = "Cartão VA";
        private const string CARTAO_VALE_REFEICAO = "Cartão VR";

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

            if (billToPay != null && AccountIsValidRule(billToPay.Account))
            {
                validatorBase.Add("[32]", $"Já existe uma conta a pagar para este mesmo nome: {input.Name}, neste mesmo Ano/Mes: {input.InitialMonthYear} e nesta frequência: {input.Frequence}");
            }

            return validatorBase;
        }

        public static bool AccountIsValidRule(string? account)
        {
            bool isAccountValidRule = true;
            switch (account)
            {
                case CARTAO_CREDITO:
                    isAccountValidRule = false;
                    break;
                case CARTAO_VALE_ALIMENTACAO:
                    isAccountValidRule = false;
                    break;
                case CARTAO_VALE_REFEICAO:
                    isAccountValidRule = false;
                    break;
                default:
                    break;
            }

            return isAccountValidRule;
        }
    }
}
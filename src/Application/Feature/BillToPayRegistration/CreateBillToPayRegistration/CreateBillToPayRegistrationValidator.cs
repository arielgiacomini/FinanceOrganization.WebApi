using Domain.Interfaces;

namespace Application.Feature.BillToPayRegistration.CreateBillToPayRegistration
{
    public static class CreateBillToPayRegistrationValidator
    {
        private const string CARTAO_CREDITO = "Cartão de Crédito";
        private const string CARTAO_DEBITO = "Cartão de Débito";
        private const string CARTAO_VALE_ALIMENTACAO = "Cartão VA";
        private const string CARTAO_VALE_REFEICAO = "Cartão VR";

        public static async Task<Dictionary<string, string>> ValidateInput(
            CreateBillToPayRegistrationInput input,
            IBillToPayRegistrationRepository billToPayRegistrationRepository,
            IBillToPayRepository billToPayRepository)
        {
            return await CreateValidateBaseInput(input, billToPayRegistrationRepository, billToPayRepository);
        }

        public static async Task<Dictionary<string, string>> CreateValidateBaseInput(CreateBillToPayRegistrationInput input,
            IBillToPayRegistrationRepository billToPayRegistrationRepository, IBillToPayRepository billToPayRepository)
        {
            Dictionary<string, string> validatorBase = new();

            if (input.BestPayDay == null && input.PurchaseDate == null)
            {
                validatorBase.Add("[58]", $"Caso o BestPayDay (Melhor dia de Pagamento) não for informado é necessário ter um PurchaseDate (Data de Compra) pois ele será considerado.");
            }

            var billToPay = await billToPayRepository.GetBillToPayByNameDueDateAndFrequence(input.Name!, input.InitialMonthYear!, input.Frequence!);

            if (billToPay != null && AccountIsValidRule(billToPay.Account))
            {
                validatorBase.Add("[32]", $"Já existe uma conta a pagar para este mesmo nome: {input.Name}, " +
                    $"neste mesmo Ano/Mes: {input.InitialMonthYear} e nesta frequência: {input.Frequence}");
            }

            if (input.RegistrationType == "Conta/Fatura Fixa" && input.Frequence != "Livre" && input.Account == "Cartão de Crédito")
            {
                validatorBase.Add("[33]", $"O tipo: [{input.RegistrationType}] com a frequência: [{input.Frequence}] " +
                    $"não pode ser da conta: [{input.Account}]");
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
                case CARTAO_DEBITO:
                    isAccountValidRule = false;
                    break;
                default:
                    break;
            }

            return isAccountValidRule;
        }
    }
}
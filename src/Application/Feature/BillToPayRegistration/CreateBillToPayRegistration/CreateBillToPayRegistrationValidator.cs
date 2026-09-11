using Domain.Interfaces;

namespace Application.Feature.BillToPayRegistration.CreateBillToPayRegistration
{
    public static class CreateBillToPayRegistrationValidator
    {
        public static async Task<Dictionary<string, string>> ValidateInput(
            CreateBillToPayRegistrationInput input,
            IBillToPayRegistrationRepository billToPayRegistrationRepository,
            IBillToPayRepository billToPayRepository,
            IAccountRepository accountRepository)
        {
            return await CreateValidateBaseInput(input, billToPayRegistrationRepository, billToPayRepository, accountRepository);
        }

        public static async Task<Dictionary<string, string>> CreateValidateBaseInput(CreateBillToPayRegistrationInput input,
            IBillToPayRegistrationRepository billToPayRegistrationRepository, IBillToPayRepository billToPayRepository,
            IAccountRepository accountRepository)
        {
            Dictionary<string, string> validatorBase = new();

            if (input.IdBillToPayRegistration.HasValue)
            {
                var parentRegistration = await billToPayRegistrationRepository.GetById(input.IdBillToPayRegistration.Value);

                if (parentRegistration == null)
                {
                    validatorBase.Add("[59]", $"Não foi encontrada uma conta a pagar cadastrada com o Id: {input.IdBillToPayRegistration} para associação.");
                }

                if (string.IsNullOrEmpty(input.InitialMonthYear))
                {
                    validatorBase.Add("[60]", "Ao associar a uma conta a pagar já cadastrada (IdBillToPayRegistration) é obrigatório informar o InitialMonthYear (Mês/Ano) referente ao registro que está sendo criado diretamente.");
                }

                if (string.IsNullOrEmpty(input.Account) || await accountRepository.GetAccountByName(input.Account) == null)
                {
                    validatorBase.Add("[61]", $"Não foi encontrada a conta [{input.Account}], necessária para o cadastro direto da conta a pagar associada.");
                }
            }

            if (input.BestPayDay == null && input.PurchaseDate == null)
            {
                validatorBase.Add("[58]", $"Caso o BestPayDay (Melhor dia de Pagamento) não for informado é necessário ter um PurchaseDate (Data de Compra) pois ele será considerado.");
            }

            var billToPay = await billToPayRepository.GetBillToPayByNameDueDateAndFrequence(input.Name!, input.InitialMonthYear!, input.Frequence!);

            if (billToPay != null && await AccountIsValidRule(billToPayRegistrationRepository, input))
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

        public static async Task<bool> AccountIsValidRule(IBillToPayRegistrationRepository registration, CreateBillToPayRegistrationInput input)
        {
            bool isAccountValidRule = false;

            var teste = await registration.GetBillToPayRegistrationByName(input.Name);

            if (teste.Name == input.Name &&
                teste.PurchaseDate == input.PurchaseDate &&
                teste.Value == input.Value &&
                teste.InitialMonthYear == input.InitialMonthYear &&
                teste.FynallyMonthYear == input.FynallyMonthYear &&
                teste.AdditionalMessage == input.AdditionalMessage)
            {
                isAccountValidRule = true;
            }

            return await Task.FromResult(isAccountValidRule);
        }
    }
}
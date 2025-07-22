using Domain.Interfaces;

namespace Application.Feature.BillToPayRegistration.CreateCreditCardNFCMobileBillToPayRegistration
{
    public class CreateNFCMobileBillToPayRegistrationValidator
    {
        private const string CARTAO_CREDITO = "Cartão de Crédito";
        private const string CARTAO_DEBITO = "Cartão de Débito";
        private const string CARTAO_VALE_ALIMENTACAO = "Cartão VA";
        private const string CARTAO_VALE_REFEICAO = "Cartão VR";

        public static async Task<Dictionary<string, string>> ValidateInput(
            CreateNFCMobileBillToPayRegistrationInput input,
            IBillToPayRegistrationRepository billToPayRegistrationRepository,
            IBillToPayRepository billToPayRepository,
            IAccountRepository accountRepository)
        {
            return await CreateValidateBaseInput(input, billToPayRegistrationRepository, billToPayRepository, accountRepository);
        }

        public static async Task<Dictionary<string, string>> CreateValidateBaseInput(
            CreateNFCMobileBillToPayRegistrationInput input,
            IBillToPayRegistrationRepository billToPayRegistrationRepository,
            IBillToPayRepository billToPayRepository,
            IAccountRepository accountRepository)
        {
            Dictionary<string, string> validatorBase = new();

            if (string.IsNullOrEmpty(input.Account))
            {
                validatorBase.Add("001", $"A conta [{input.Account}] deve ser informada.");
            }

            var validateAccount = await accountRepository.GetAccountByName(input.Account!);

            if (validateAccount == null)
            {
                validatorBase.Add("002", $"Não foi encontrada essa conta [{input.Account}] em nossos registros.");
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
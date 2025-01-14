using Domain.Interfaces;
using Domain.Utils;

namespace Application.Feature.BillToPay.PayBillToPay
{
    public static class PayBillToPayValidator
    {
        private const string EH_CARTAO_CREDITO_NAIRA = "Cartão de Crédito Nubank Naíra";

        public static async Task<Dictionary<string, string>> ValidateInput(
            PayBillToPayInput input,
            IBillToPayRepository billToPayRepository,
            IAccountRepository accountRepository)
        {
            return await CreateValidateBaseInput(input, billToPayRepository, accountRepository);
        }

        private static async Task<Dictionary<string, string>> CreateValidateBaseInput(
            PayBillToPayInput input,
            IBillToPayRepository billToPayRepository,
            IAccountRepository accountRepository)
        {
            Dictionary<string, string> validatorBase = new();

            Domain.Entities.BillToPay billToPay = new();

            if (string.IsNullOrEmpty(input.Account))
            {
                validatorBase.Add("[30]", $"A conta [{input.Account}] deve ser informada.");
            }

            var validateAccount = await accountRepository.GetAccountByName(input.Account!);

            if (validateAccount == null)
            {
                validatorBase.Add("[31]", $"Não foi encontrada essa conta [{input.Account}] em nossos registros.");
            }

            if (input.Id != null)
            {
                billToPay = await billToPayRepository.GetBillToPayById(input.Id.Value) ?? new Domain.Entities.BillToPay();
            }

            //TODO: VALIDAR E TESTES
            if (validateAccount!.IsCreditCard && billToPay.Id == Guid.Empty)
            {
                if (string.IsNullOrEmpty(input.YearMonth))
                {
                    validatorBase.Add("[32]", $"Se a conta for [{input.Account}] é obrigatório informar o Ano/Mês de Pagamento.");
                }

                if (input.Id != null)
                {
                    validatorBase.Add("[33]", $"Se a conta for [{input.Account}] não deve ser informado um Id de conta para pagamento. " +
                        $"O sistema irá fazer a baixa de todos os itens pendentes de pagamento da fatura de cartão de crédito.");
                }

                var invoiceClosingDate = DateServiceUtils.GetDateTimeByYearMonthBrazilian(input.YearMonth, 1, 1);

                if (DateTime.Now < invoiceClosingDate)
                {
                    validatorBase.Add("[34]", $"A fatura do Ano/Mês: [{input.YearMonth}] só vai fechar a partir do dia: " +
                        $"[{invoiceClosingDate.Value.Date:dd/MM/yyyy}] os lançamentos atuais podem sofrer alterações " +
                        $"e portanto ainda não está disponível para pagamento.");
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(input.Account))
                {
                    validatorBase.Add("[35]", $"Esta conta: {input.Account} é inválida.");
                }

                if (!string.IsNullOrWhiteSpace(input.YearMonth))
                {
                    validatorBase.Add("[36]", $"O Ano/Mês só é valido quando uma conta válida for informada.");
                }

                if (input.Id != null)
                {
                    var result = await billToPayRepository.GetBillToPayById(input.Id.Value);

                    if (result == null)
                    {
                        validatorBase.Add("[37]", $"Não foi encontrado Conta a pagar pelo Id: {input.Id}");
                    }
                }
                else
                {
                    validatorBase.Add("[38]", $"O id da conta a pagar é obrigatório: {input.Id}");
                }
            }

            return validatorBase;
        }
    }
}
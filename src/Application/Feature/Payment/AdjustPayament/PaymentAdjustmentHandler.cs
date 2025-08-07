using Application.Feature.CashReceivable.AdjustCashReceivable;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Feature.Payment.AdjustPayament
{
    public class PaymentAdjustmentHandler : IPaymentAdjustmentHandler
    {
        private readonly ILogger<PaymentAdjustmentHandler> _logger;
        private readonly IAdjustCashReceivableHandler _adjustCashReceivableHandler;
        private readonly IAccountRepository _accountRepository;
        private readonly IBillToPayRepository _billToPayRepository;

        private const string FREQUENCIA_MENSAL_RECORRENTE = "Mensal:Recorrente";
        private const string FREQUENCIA_LIVRE = "Livre";
        private const string TIPO_REGISTRO_FATURA_FIXA = "Conta/Fatura Fixa";
        private const string TIPO_REGISTRO_COMPRA_LIVRE = "Compra Livre";
        private const int QUANTOS_DIAS_PASSADOS_CONSIDERAR = -1;

        public PaymentAdjustmentHandler(
            ILogger<PaymentAdjustmentHandler> logger,
            IAdjustCashReceivableHandler adjustCashReceivable,
            IAccountRepository accountRepository,
            IBillToPayRepository billToPayRepository)
        {
            _logger = logger;
            _adjustCashReceivableHandler = adjustCashReceivable;
            _accountRepository = accountRepository;
            _billToPayRepository = billToPayRepository;
        }

        /// <summary>
        /// Isola o ajuste de pagamento, validando os dados de entrada e ajustando as contas a pagar e receber.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PaymentAdjustmentOutput> Handle(PaymentAdjustmentInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando o ajuste de pagamento para: {Name}", input.Name);

            var validate = await PaymentAdjustmentValidator.ValidateInput(input);

            if (validate.Any())
            {
                _logger.LogWarning("Erro de validação. Dados: {@input}, Validação: {@validate}", input, validate);

                return new PaymentAdjustmentOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };
            }

            var account = await _accountRepository
                .GetAccountByName(input.Account);

            if (account == null)
            {
                return new PaymentAdjustmentOutput
                {
                    Output = OutputBaseDetails.Error("", new Dictionary<string, string>(), 0)
                };
            }

            /* 1º Faz o ajuste da conta a pagar, encontrando qual é a conta a pagar com valor fixo que precisa ser alterada. */
            await AccountsPayableDebitFixed(input);

            /* 2º Faz o ajuste da conta a receber relacionado a conta. */
            var isAdjusted = _adjustCashReceivableHandler.Adjust(input);

            var output = new PaymentAdjustmentOutput
            {
                Output = OutputBaseDetails.Success($"Ajuste realizado com sucesso. Resultado: {isAdjusted}", new object(), 1)
            };

            return output;
        }

        /// <summary>
        /// Informa se a conta é considerada paga.
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="registrationType"></param>
        /// <returns></returns>
        public async Task<bool> ConsideredPaid(string accountName, string registrationType)
        {
            var account = await _accountRepository
                .GetAccountByName(accountName);

            bool consideredPaid = false;

            if (account == null)
            {
                consideredPaid = false;
                return consideredPaid;
            }

            if (account.ConsiderPaid.HasValue &&
                account.ConsiderPaid.Value &&
                IsNotFaturaFixa(registrationType))
            {
                consideredPaid = true;
            }

            return consideredPaid;
        }

        private static bool IsNotFaturaFixa(string registrationType)
        {
            return registrationType != TIPO_REGISTRO_FATURA_FIXA;
        }

        /// <summary>
        /// Caso for uma conta a pagar, verifica se é uma conta livre e se é válida para desconto, se for desconta o valor da conta fixa.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task AccountsPayableDebitFixed(PaymentAdjustmentInput input)
        {
            if (IsValidToDebit(input))
            {
                var descontar = await _billToPayRepository
                    .GetByYearMonthCategoryAndRegistrationType(
                    input.YearMonth!, input.Category!, TIPO_REGISTRO_FATURA_FIXA);

                if (descontar == null)
                {
                    _logger.LogInformation("Desconto de conta fixa, mesmo a conta sendo válida para desconto " +
                        "algo deu errado na pesquisa e retornou null. Mes Ano Inicial: {InitialMonthYear} Categoria: {Category} e {Tipo}",
                        input.YearMonth, input.Category, TIPO_REGISTRO_FATURA_FIXA);
                    return;
                }

                if (descontar.Frequence != FREQUENCIA_LIVRE)
                {
                    var valueOld = descontar.Value;

                    if (valueOld > 0)
                    {
                        descontar.Value -= input.Value;

                        descontar.Value = descontar.Value >= 0 ? descontar.Value : 0;
                    }

                    var dateTimeNow = DateTime.Now.Date.ToString("dd/MM/yyyy");
                    descontar.AdditionalMessage += $" | Removido automaticamente: [R$ {input.Value}] em [{dateTimeNow}] do valor que estava: [R$ {valueOld}] pela seguinte conta: [{input.Name}]";

                    var edited = await _billToPayRepository.Edit(descontar);

                    if (edited == 1)
                    {
                        _logger.LogInformation("Foi aplicado o desconto de: {ValueConta} da conta fixa: {Name} que tinha um valor antigo de: {valueOld} relacionada ao cadastro da conta livre: {freeConta} que acabou de ser cadastrada.",
                            input.Value, descontar.Name, valueOld, input.Name);
                    }
                }
            }
        }

        private static bool IsValidToDebit(PaymentAdjustmentInput input)
        {
            var isTrue = input.RegistrationType == TIPO_REGISTRO_COMPRA_LIVRE
                      && input.QuantityMonthsAdd == 0
                      && input.Frequence == FREQUENCIA_LIVRE;

            return isTrue;
        }
    }
}
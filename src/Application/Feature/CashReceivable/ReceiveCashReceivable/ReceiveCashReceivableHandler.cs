using Domain.Interfaces;
using Serilog;
using D = Domain.Entities;

namespace Application.Feature.CashReceivable.ReceiveCashReceivable
{
    public class ReceiveCashReceivableHandler : IReceiveCashReceivableHandler
    {
        private readonly ILogger _logger;
        private readonly ICashReceivableRepository _cashReceivableRepository;

        public ReceiveCashReceivableHandler(ILogger logger, ICashReceivableRepository cashReceivableRepository)
        {
            _logger = logger;
            _cashReceivableRepository = cashReceivableRepository;
        }

        public async Task<ReceiveCashReceivableOutput> Handle(ReceiveCashReceivableInput input, CancellationToken cancellationToken)
        {
            ReceiveCashReceivableOutput output = new();
            var validate = await ReceiveCashReceivableValidator.ValidateInput(input, _cashReceivableRepository);

            if (validate.Any())
            {
                _logger.Warning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new ReceiveCashReceivableOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var listToPay = await SearchToPay(input);

            if (listToPay == null || listToPay.Count == 0)
            {
                output.Output = OutputBaseDetails.Error($"Não foram encontradas nenhuma conta a pagar de ID [{input.Id}] informado.", new Dictionary<string, string>());

                return output;
            }

            var cashReceivables = MapInputToDomain(listToPay, input);

            var result = await _cashReceivableRepository.EditRange(cashReceivables!);

            if (result <= 0)
            {
                output.Output = OutputBaseDetails.Error("Houve erro ao efetuar o pagamento.", new Dictionary<string, string>(), cashReceivables.Count);

                return output;
            }

            output.Output = OutputBaseDetails.Success($"{result} - Pagamento realizado com sucesso.", new object(), cashReceivables.Count);

            return output;
        }

        private static IList<D.CashReceivable> MapInputToDomain(IList<D.CashReceivable> cashsReceivable, ReceiveCashReceivableInput input)
        {
            List<D.CashReceivable> mapCashsReceivable = new();

            if (cashsReceivable != null)
            {
                foreach (var cash in cashsReceivable)
                {
                    mapCashsReceivable.Add(new D.CashReceivable()
                    {
                        Id = cash.Id,
                        IdCashReceivableRegistration = cash.IdCashReceivableRegistration,
                        Name = cash.Name,
                        Account = cash.Account,
                        Frequence = cash.Frequence,
                        RegistrationType = cash.RegistrationType,
                        AgreementDate = cash.AgreementDate,
                        DueDate = cash.DueDate,
                        YearMonth = cash.YearMonth,
                        Category = cash.Category,
                        Value = cash.Value,
                        ManipulatedValue = 0.0m,
                        DateReceived = input.DateReceived,
                        HasReceived = true,
                        AdditionalMessage = string.Concat("Pagamento em: [", DateTime.Now.ToString("dd/MM/yyyy"), "] | ", cash.AdditionalMessage),
                        CreationDate = cash.CreationDate,
                        LastChangeDate = DateTime.Now,
                        Country = cash.Country,
                        Enabled = cash.Enabled
                    });
                }
            }

            return mapCashsReceivable;
        }

        private async Task<IList<D.CashReceivable>> SearchToPay(ReceiveCashReceivableInput input)
        {
            List<D.CashReceivable> listPay = new();

            if (input.Id != null)
            {
                var cash = await _cashReceivableRepository.GetById(input.Id.Value);

                if (cash != null)
                {
                    listPay.Add(cash);
                }
            }

            return listPay;
        }
    }
}
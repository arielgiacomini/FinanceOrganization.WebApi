using Domain.Interfaces;
using Serilog;

namespace Application.Feature.BillToPay.PayBillToPay
{
    public class PayBillToPayHandler : IPayBillToPayHandler
    {
        private readonly ILogger _logger;
        private readonly IBillToPayRepository _billToPayRepository;

        public PayBillToPayHandler(ILogger logger, IBillToPayRepository billToPayRepository)
        {
            _logger = logger;
            _billToPayRepository = billToPayRepository;
        }

        public async Task<PayBillToPayOutput> Handle(PayBillToPayInput input, CancellationToken cancellationToken)
        {
            PayBillToPayOutput output = new();
            var validate = await PayBillToPayValidator.ValidateInput(input, _billToPayRepository);

            if (validate.Any())
            {
                _logger.Warning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new PayBillToPayOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var billToPay = await MapInputToDomain(input);

            if (billToPay == null)
            {
                output.Output = OutputBaseDetails.Error("Não foi possível encontrar a conta para pagamento.", new Dictionary<string, string>(), 1);

                return output;
            }

            var result = await _billToPayRepository.Edit(billToPay);

            if (result != 1)
            {
                output.Output = OutputBaseDetails.Error("Houve erro ao efetuar o pagamento.", new Dictionary<string, string>());

                return output;
            }

            output.Output = OutputBaseDetails.Success($"{result} - Pagamento realizado com sucesso.", new object());

            return output;
        }

        private async Task<Domain.Entities.BillToPay?> MapInputToDomain(PayBillToPayInput input)
        {
            var billToPay = await _billToPayRepository.GetBillToPayById(input.Id);

            if (billToPay == null)
            {
                return null;
            }

            return new Domain.Entities.BillToPay()
            {
                Id = input.Id,
                IdFixedInvoice = billToPay.IdFixedInvoice,
                Account = billToPay.Account,
                Name = billToPay.Name,
                Category = billToPay.Category,
                Value = billToPay.Value,
                DueDate = billToPay.DueDate,
                YearMonth = billToPay.YearMonth,
                Frequence = billToPay.Frequence,
                PayDay = input.PayDay,
                HasPay = input.HasPay,
                CreationDate = billToPay.CreationDate,
                LastChangeDate = input.LastChangeDate
            };
        }
    }
}
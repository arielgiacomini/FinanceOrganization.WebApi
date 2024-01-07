using Domain.Interfaces;
using Serilog;
using D = Domain.Entities;

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

            var listToPay = await SearchToPay(input);

            var billsToPay = MapInputToDomain(listToPay, input);

            if (billsToPay == null)
            {
                output.Output = OutputBaseDetails.Error("Não foi possível encontrar a conta para pagamento.", new Dictionary<string, string>(), 1);

                return output;
            }

            var result = await _billToPayRepository.EditRange(billsToPay);

            if (result != 1)
            {
                output.Output = OutputBaseDetails.Error("Houve erro ao efetuar o pagamento.", new Dictionary<string, string>());

                return output;
            }

            output.Output = OutputBaseDetails.Success($"{result} - Pagamento realizado com sucesso.", new object());

            return output;
        }

        private static IList<D.BillToPay>? MapInputToDomain(IList<D.BillToPay>? billsToPay, PayBillToPayInput input)
        {
            List<D.BillToPay>? mapBillsToPay = new();

            if (billsToPay != null)
            {
                foreach (var bill in billsToPay)
                {
                    mapBillsToPay.Add(new D.BillToPay()
                    {
                        Id = bill.Id,
                        IdFixedInvoice = bill.IdFixedInvoice,
                        Account = bill.Account,
                        Name = bill.Name,
                        Category = bill.Category,
                        Value = bill.Value,
                        DueDate = bill.DueDate,
                        YearMonth = bill.YearMonth,
                        Frequence = bill.Frequence,
                        PayDay = input.PayDay,
                        HasPay = input.HasPay,
                        CreationDate = bill.CreationDate,
                        LastChangeDate = input.LastChangeDate
                    });
                }
            }

            return mapBillsToPay;
        }

        private async Task<IList<D.BillToPay>?> SearchToPay(PayBillToPayInput input)
        {
            List<D.BillToPay>? billsToPay = new();

            if (input.Id != null)
            {
                var bill = await _billToPayRepository.GetBillToPayById(input.Id.Value);

                if (bill != null)
                {
                    billsToPay.Add(bill);
                }
            }

            if (input.Account != null && input.YearMonth != null)
            {
                var listBill = await _billToPayRepository.GetBillToPayByYearMonthAndAccount(input.YearMonth, input.Account);

                if (listBill != null)
                {
                    billsToPay.AddRange(listBill);
                }
            }

            return billsToPay;
        }
    }
}
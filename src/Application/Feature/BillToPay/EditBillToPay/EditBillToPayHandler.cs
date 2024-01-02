using Domain.Interfaces;
using Serilog;

namespace Application.Feature.BillToPay.EditBillToPay
{
    public class EditBillToPayHandler : IEditBillToPayHandler
    {
        private readonly ILogger _logger;
        private readonly IBillToPayRepository _walletToPayRepository;

        public EditBillToPayHandler(ILogger logger, IBillToPayRepository walletToPayRepository)
        {
            _logger = logger;
            _walletToPayRepository = walletToPayRepository;
        }

        public async Task<EditBillToPayOutput> Handle(EditBillToPayInput input, CancellationToken cancellationToken)
        {
            var validate = await EditBillToPayValidator.ValidateInput(input, _walletToPayRepository);

            if (validate.Any())
            {
                _logger.Warning("");
                var outputValidator = new EditBillToPayOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var billToPay = MapInputToDomain(input);

            var result = await _walletToPayRepository.Edit(billToPay);

            var output = new EditBillToPayOutput
            {
                Output = OutputBaseDetails.Success($"[{result}] - Cadastro realizado com sucesso.", new object())
            };

            return output;
        }

        private static Domain.Entities.BillToPay MapInputToDomain(EditBillToPayInput updateBillToPayInput)
        {
            return new Domain.Entities.BillToPay()
            {
                Id = updateBillToPayInput.Id,
                IdFixedInvoice = updateBillToPayInput.IdFixedInvoice,
                Account = updateBillToPayInput.Account,
                Name = updateBillToPayInput.Name,
                Category = updateBillToPayInput.Category,
                Value = updateBillToPayInput.Value,
                DueDate = updateBillToPayInput.DueDate,
                YearMonth = updateBillToPayInput.YearMonth,
                Frequence = updateBillToPayInput.Frequence,
                PayDay = updateBillToPayInput.PayDay,
                HasPay = updateBillToPayInput.HasPay,
                LastChangeDate = updateBillToPayInput.LastChangeDate
            };
        }
    }
}
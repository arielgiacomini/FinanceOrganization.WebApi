using Domain.Interfaces;
using Serilog;

namespace Application.Feature.BillToPay.EditBillToPay
{
    public class EditBillToPayHandler : IEditBillToPayHandler
    {
        private readonly ILogger _logger;
        private readonly IBillToPayRepository _billToPayRepository;

        public EditBillToPayHandler(ILogger logger, IBillToPayRepository billToPayRepository)
        {
            _logger = logger;
            _billToPayRepository = billToPayRepository;
        }

        public async Task<EditBillToPayOutput> Handle(EditBillToPayInput input, CancellationToken cancellationToken)
        {
            var validate = await EditBillToPayValidator.ValidateInput(input, _billToPayRepository);

            if (validate.Any())
            {
                _logger.Warning("");
                var outputValidator = new EditBillToPayOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var billToPayDatabase = await _billToPayRepository.GetBillToPayById(input.Id);

            if (billToPayDatabase is null)
            {
                var outputError = new EditBillToPayOutput
                {
                    Output = OutputBaseDetails.Error($"[56] - Erro ao buscar o BillToPay do Banco de Dados.", new Dictionary<string, string>(), 0)
                };

                return outputError;
            }

            var billToPay = MapInputToDomain(input, billToPayDatabase);

            var result = await _billToPayRepository.Edit(billToPay);

            var output = new EditBillToPayOutput
            {
                Output = OutputBaseDetails.Success($"[{result}] - Cadastro realizado com sucesso.", new object())
            };

            return output;
        }

        private static Domain.Entities.BillToPay MapInputToDomain(EditBillToPayInput updateBillToPayInput,
            Domain.Entities.BillToPay? billToPayDatabase)
        {
            return new Domain.Entities.BillToPay()
            {
                Id = updateBillToPayInput.Id,
                IdBillToPayRegistration = billToPayDatabase!.IdBillToPayRegistration,
                Account = updateBillToPayInput.Account,
                Name = updateBillToPayInput.Name,
                Category = updateBillToPayInput.Category,
                Value = updateBillToPayInput.Value,
                PurchaseDate = updateBillToPayInput.PurchaseDate,
                DueDate = updateBillToPayInput.DueDate,
                YearMonth = updateBillToPayInput.YearMonth,
                Frequence = updateBillToPayInput.Frequence,
                RegistrationType = updateBillToPayInput.RegistrationType,
                PayDay = updateBillToPayInput.PayDay,
                HasPay = updateBillToPayInput.HasPay,
                AdditionalMessage = updateBillToPayInput.AdditionalMessage,
                CreationDate = billToPayDatabase!.CreationDate,
                LastChangeDate = updateBillToPayInput.LastChangeDate
            };
        }
    }
}
using Domain.Interfaces;
using Serilog;

namespace Application.Feature.BillToPay.EditBillToPay
{
    public class EditBillToPayHandler : IEditBillToPayHandler
    {
        private readonly ILogger _logger;
        private readonly IBillToPayRepository _billToPayRepository;
        private readonly IBillToPayRegistrationRepository _billToPayRegistrationRepository;

        public EditBillToPayHandler(
            ILogger logger,
            IBillToPayRepository billToPayRepository,
            IBillToPayRegistrationRepository billToPayRegistrationRepository)
        {
            _logger = logger;
            _billToPayRepository = billToPayRepository;
            _billToPayRegistrationRepository = billToPayRegistrationRepository;
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

            if (input.MustEditRegistrationAccount)
            {
                var registration = await _billToPayRegistrationRepository.GetById(billToPay.IdBillToPayRegistration);

                if (registration is null)
                {
                    _logger.Warning("Não foi possível fazer a edição da conta de registro. Essa edição era pra deixar a conta o mais atualizado possível");
                }

                registration.Name = input.Name;
                registration.Value = input.Value;
                registration.AdditionalMessage = string.Concat(" | ", input.AdditionalMessage);
                registration.Category = input.Category;
                registration.Account = input.Account;
                registration.RegistrationType = input.RegistrationType;
                registration.LastChangeDate = DateTime.Now;

                var editRegistration = await _billToPayRegistrationRepository.Edit(registration);

                if (editRegistration > 0)
                {
                    _logger.Warning("Houve um problema ao tentar editar a conta {Name} de Id: {Id} e o processo não foi efetuado.", registration.Name, registration.Id);
                }
                else
                {
                    _logger.Information("Alteração da conta {Name} de Id: {} realizado com sucesso.", registration.Name, registration.Id);
                }
            }

            var output = new EditBillToPayOutput
            {
                Output = OutputBaseDetails.Success($"[{result}] - Conta a Pagar alterada com sucesso.", new object())
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
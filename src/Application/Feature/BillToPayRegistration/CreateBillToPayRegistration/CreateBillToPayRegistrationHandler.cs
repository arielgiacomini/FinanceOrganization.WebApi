using Domain.Interfaces;
using Serilog;

namespace Application.Feature.BillToPayRegistration.CreateBillToPayRegistration;

public class CreateBillToPayRegistrationHandler : ICreateBillToPayRegistrationHandler
{
    private readonly ILogger _logger;
    private readonly IBillToPayRegistrationRepository _fixedInvoiceRepository;
    private readonly IBillToPayRepository _billToPayRepository;

    public CreateBillToPayRegistrationHandler(ILogger logger,
        IBillToPayRegistrationRepository fixedInvoiceRepository,
        IBillToPayRepository billToPayRepository)
    {
        _logger = logger;
        _fixedInvoiceRepository = fixedInvoiceRepository;
        _billToPayRepository = billToPayRepository;
    }

    public async Task<CreateBillToPayRegistrationOutput> Handle(CreateBillToPayRegistrationInput input,
        CancellationToken cancellationToken = default)
    {
        _logger.Information("Está sendo criado a conta a pagar de nome: {Name}", input.Name);

        var validate = await CreateBillToPayRegistrationValidator.ValidateInput(input, _fixedInvoiceRepository, _billToPayRepository);

        if (validate.Any())
        {
            _logger.Warning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

            var outputValidator = new CreateBillToPayRegistrationOutput
            {
                Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
            };

            return outputValidator;
        }

        var isSaved = await _fixedInvoiceRepository.Save(MapInputFixedInvoiceToDomain(input));

        var output = new CreateBillToPayRegistrationOutput
        {
            Output = OutputBaseDetails.Success($"[{isSaved}] - Cadastro realizado com sucesso.", new object(), 1)
        };

        return await Task.FromResult(output);
    }

    private static Domain.Entities.BillToPayRegistration MapInputFixedInvoiceToDomain(CreateBillToPayRegistrationInput input)
    {
        return new Domain.Entities.BillToPayRegistration
        {
            Name = input.Name,
            Category = input.Category,
            Account = input.Account,
            Value = input.Value,
            PurchaseDate = input.PurchaseDate,
            BestPayDay = input.BestPayDay ?? input.PurchaseDate!.Value.Day,
            InitialMonthYear = input.InitialMonthYear,
            FynallyMonthYear = input.FynallyMonthYear,
            Frequence = input.Frequence,
            RegistrationType = input.RegistrationType,
            AdditionalMessage = input.AdditionalMessage,
            CreationDate = input.CreationDate,
            LastChangeDate = input.LastChangeDate
        };
    }
}
using Domain.Interfaces;

namespace Application.Feature.FixedInvoice.CreateFixedInvoice
{
    public class CreateFixedInvoiceHandler : ICreateFixedInvoiceHandler
    {
        private readonly IFixedInvoiceRepository _fixedInvoiceRepository;

        public CreateFixedInvoiceHandler(
            IFixedInvoiceRepository fixedInvoiceRepository)
        {
            _fixedInvoiceRepository = fixedInvoiceRepository;
        }

        public async Task<CreateFixedInvoiceOutput> Handle(CreateFixedInvoiceInput input,
            CancellationToken cancellationToken = default)
        {
            var IdFixedInvoice = await _fixedInvoiceRepository.Save(MapInputFixedInvoiceToDomain(input));

            var output = new CreateFixedInvoiceOutput
            {
                Output = OutputBaseDetails.Success($"[{IdFixedInvoice}] - Cadastro realizado com sucesso.", new object())
            };

            return await Task.FromResult(output);
        }

        private static Domain.Entities.FixedInvoice MapInputFixedInvoiceToDomain(CreateFixedInvoiceInput input)
        {
            return new Domain.Entities.FixedInvoice
            {
                Id = input.Id,
                Name = input.Name,
                Category = input.Category,
                Account = input.Account,
                Value = input.Value,
                BestPayDay = input.BestPayDay,
                InitialMonthYear = input.InitialMonthYear,
                FynallyMonthYear = input.FynallyMonthYear,
                Frequence = input.Frequence,
                CreationDate = input.CreationDate,
                HasRegistration = input.HasRegistration,
                LastChangeDate = input.LastChangeDate
            };
        }
    }
}
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Feature.CreateFixedInvoice
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

        private static FixedInvoice MapInputFixedInvoiceToDomain(CreateFixedInvoiceInput input)
        {
            return new FixedInvoice
            {
                Id = input.Id,
                Name = input.Name,
                Frequence = input.Frequence,
                InitialMonthYear = input.InitialMonthYear,
                FynallyMonthYear = input.FynallyMonthYear,
                Category = input.Category,
                Value = input.Value,
                CreationDate = input.CreationDate,
                HasRegistration = input.HasRegistration,
                LastChangeDate = input.LastChangeDate
            };
        }
    }
}
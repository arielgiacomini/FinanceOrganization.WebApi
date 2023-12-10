using Domain.Entities;
using Domain.Interfaces;

namespace Application.Feature.CreateFixedInvoice
{
    public class CreateFixedInvoiceHandler : ICreateFixedInvoiceHandler
    {
        private readonly IFixedInvoiceRepository _fixedInvoiceRepository;

        public CreateFixedInvoiceHandler(IFixedInvoiceRepository fixedInvoiceRepository)
        {
            _fixedInvoiceRepository = fixedInvoiceRepository;
        }

        public async Task<CreateFixedInvoiceOutput> Handler(CreateFixedInvoiceInput input, 
            CancellationToken cancellationToken = default)
        {
            var IdFixedInvoice = await _fixedInvoiceRepository.Save(MapInputFixedInvoiceToDomain(input));

            var output = new CreateFixedInvoiceOutput
            {
                OutputBaseDetails = OutputBaseDetails.Success($"[{IdFixedInvoice}] - Cadastro realizado com sucesso.")
            };

            return await Task.FromResult(output);
        }

        private static FixedInvoice MapInputFixedInvoiceToDomain(CreateFixedInvoiceInput input)
        {
            return new FixedInvoice
            {
                Id = input.Id
            };
        }
    }
}
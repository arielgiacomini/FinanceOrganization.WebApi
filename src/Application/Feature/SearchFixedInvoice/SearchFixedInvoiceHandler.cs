﻿using Domain.Interfaces;

namespace Application.Feature.SearchFixedInvoice
{
    public class SearchFixedInvoiceHandler : ISearchFixedInvoiceHandler
    {
        private readonly IFixedInvoiceRepository _fixedInvoiceRepository;

        public SearchFixedInvoiceHandler(IFixedInvoiceRepository fixedInvoiceRepository)
        {
            _fixedInvoiceRepository = fixedInvoiceRepository;
        }

        public async Task<SearchFixedInvoiceOutput> Handle(CancellationToken cancellationToken = default)
        {
            var getByAll = await _fixedInvoiceRepository.GetByAll();

            var output = new SearchFixedInvoiceOutput
            {
                Output = OutputBaseDetails.Success("Busca realizada com sucesso.", getByAll)
            };

            return output;
        }
    }
}
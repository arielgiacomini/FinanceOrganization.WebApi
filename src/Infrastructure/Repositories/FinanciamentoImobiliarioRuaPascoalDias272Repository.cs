using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Infrastructure.Repositories
{
    public class FinanciamentoImobiliarioRuaPascoalDias272Repository : IFinanciamentoImobiliarioRuaPascoalDias272Repository
    {
        private readonly FinanceOrganizationContext _context;
        private readonly ILogger _logger;

        public FinanciamentoImobiliarioRuaPascoalDias272Repository(
            ILogger logger, FinanceOrganizationContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IList<FinanciamentoImobiliarioRuaPascoalDias272>> GetFinancingList()
        {
            try
            {
                var financingList = await _context.FinanciamentoImobiliarioRuaPascoalDias272.ToListAsync();

                return financingList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
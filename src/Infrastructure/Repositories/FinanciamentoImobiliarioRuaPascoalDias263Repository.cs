using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Repositories
{
    public class FinanciamentoImobiliarioRuaPascoalDias263Repository : IFinanciamentoImobiliarioRuaPascoalDias263Repository //Criar Interface
    {
        private readonly FinanceOrganizationContext _context;
        private readonly ILogger _looger;

        public FinanciamentoImobiliarioRuaPascoalDias263Repository(
            ILogger logger, FinanceOrganizationContext context)
        {
            _context = context;
            _looger = logger;
        }

        public async Task<IList<FinanciamentoImobiliarioRuaPascoalDias263>> GetAllInstallments() 
        {
            try
            {
                var installments = await _context.FinanciamentoImobiliarioRuaPascoalDias263.ToListAsync();

                return installments;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}

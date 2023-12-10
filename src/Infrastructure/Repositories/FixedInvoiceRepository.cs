using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class FixedInvoiceRepository : IFixedInvoiceRepository
    {
        private readonly FinanceOrganizationContext _context;

        public FixedInvoiceRepository(FinanceOrganizationContext context)
        {
            _context = context;
        }

        public async Task<IList<FixedInvoice>> GetByAll()
        {
            var result = await _context.FixedInvoices
                   .AsNoTracking()
                   .ToListAsync();

            return result;
        }
    }
}
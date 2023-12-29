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
            var result = await _context.FixedInvoice!
                   .AsNoTracking()
                   .ToListAsync();

            return result;
        }

        public async Task<FixedInvoice?> GetFixedInvoiceByName(string? name)
        {
            var result = await _context.FixedInvoice!
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name == name);

            return result;
        }

        public async Task<FixedInvoice?> GetById(int iD)
        {
            try
            {
                var result = await _context.FixedInvoice!
                    .AsNoTracking()
                    .FirstOrDefaultAsync(fixedInvoice => fixedInvoice.Id == iD);

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> Save(FixedInvoice fixedInvoice)
        {
            _context.Add(fixedInvoice);
            var qtdEntry = await _context.SaveChangesAsync();

            return qtdEntry;
        }
    }
}
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

        public async Task<IList<FixedInvoice>> GetAll()
        {
            var result = await _context.FixedInvoice!
                   .AsNoTracking()
                   .ToListAsync();

            return result;
        }

        public async Task<IList<FixedInvoice>> GetOnlyOldRecordsAndParticipants(int daysLater, string registrationType)
        {
            var result = await _context.FixedInvoice!
                   .AsNoTracking()
                   .Where(fixedInvoice =>
                            (fixedInvoice.LastChangeDate == null
                         || (fixedInvoice.LastChangeDate <= DateTime.Now.AddDays(daysLater)
                         && fixedInvoice.RegistrationType == registrationType)))
                   .OrderBy(orderBy => orderBy.Id)
                   .ToListAsync();

            return result;
        }

        public async Task<IList<FixedInvoice>> GetAutomationParticipantsOnly(string registrationType)
        {
            var result = await _context.FixedInvoice!
                .AsNoTracking()
                .Where(fixedInvoice => fixedInvoice.RegistrationType == registrationType)
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
                Exception exception = new(ex.Message);
                throw exception;
            }
        }

        public async Task<int> Save(FixedInvoice fixedInvoice)
        {
            _context.Add(fixedInvoice);
            var qtdEntry = await _context.SaveChangesAsync();

            return qtdEntry;
        }

        public async Task<int> Edit(FixedInvoice fixedInvoice)
        {
            _context.ChangeTracker.Clear();

            _context.FixedInvoice!.Update(fixedInvoice);

            var result = _context.SaveChanges();

            return await Task.FromResult(result);
        }
    }
}
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
                   .Where(x => !x.Enabled.HasValue || x.Enabled.Value)
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
                         && fixedInvoice.RegistrationType == registrationType)) && (!fixedInvoice.Enabled.HasValue || fixedInvoice.Enabled.Value))
                   .OrderBy(orderBy => orderBy.Id)
                   .ToListAsync();

            return result;
        }

        public async Task<IList<FixedInvoice>> GetAutomationParticipantsOnly(string registrationType)
        {
            var result = await _context.FixedInvoice!
                .AsNoTracking()
                .Where(fixedInvoice => fixedInvoice.RegistrationType == registrationType && (!fixedInvoice.Enabled.HasValue || fixedInvoice.Enabled.Value))
                .ToListAsync();

            return result;
        }

        public async Task<FixedInvoice?> GetFixedInvoiceByName(string? name)
        {
            var result = await _context.FixedInvoice!
                .AsNoTracking()
                .FirstOrDefaultAsync(fixedInvoice => fixedInvoice.Name == name && (!fixedInvoice.Enabled.HasValue || fixedInvoice.Enabled.Value));

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

        public async Task<int> DeleteRange(IList<FixedInvoice> fixedInvoices)
        {
            _context.ChangeTracker.Clear();

            _context.FixedInvoice!.RemoveRange(fixedInvoices);

            var result = _context.SaveChanges();

            return await Task.FromResult(result);
        }

        public async Task<int> Delete(FixedInvoice fixedInvoice)
        {
            _context.ChangeTracker.Clear();

            _ = _context.FixedInvoice!.Remove(fixedInvoice);

            var result = _context.SaveChanges();

            return await Task.FromResult(result);
        }
    }
}
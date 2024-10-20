using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BillToPayRegistrationRepository : IBillToPayRegistrationRepository
    {
        private readonly FinanceOrganizationContext _context;

        public BillToPayRegistrationRepository(FinanceOrganizationContext context)
        {
            _context = context;
        }

        public async Task<IList<BillToPayRegistration>> GetAll()
        {
            var result = await _context.BillToPayRegistration!
                   .AsNoTracking()
                   .Where(x => !x.Enabled.HasValue || x.Enabled.Value)
                   .ToListAsync();

            return result;
        }

        public async Task<IList<BillToPayRegistration>> GetOnlyOldRecordsAndParticipants(int daysLater, string registrationType)
        {
            var result = await _context.BillToPayRegistration!
                   .AsNoTracking()
                   .Where(billToPayRegistration =>
                            (billToPayRegistration.LastChangeDate == null
                         || (billToPayRegistration.LastChangeDate <= DateTime.Now.AddDays(daysLater)
                         && billToPayRegistration.RegistrationType == registrationType)) && (!billToPayRegistration.Enabled.HasValue || billToPayRegistration.Enabled.Value))
                   .OrderBy(orderBy => orderBy.Id)
                   .ToListAsync();

            return result;
        }

        public async Task<IList<BillToPayRegistration>> GetAutomationParticipantsOnly(string registrationType)
        {
            var result = await _context.BillToPayRegistration!
                .AsNoTracking()
                .Where(billToPayRegistration => billToPayRegistration.RegistrationType == registrationType && (!billToPayRegistration.Enabled.HasValue || billToPayRegistration.Enabled.Value))
                .ToListAsync();

            return result;
        }

        public async Task<BillToPayRegistration?> GetFixedInvoiceByName(string? name)
        {
            var result = await _context.BillToPayRegistration!
                .AsNoTracking()
                .FirstOrDefaultAsync(billToPayRegistration => billToPayRegistration.Name == name && (!billToPayRegistration.Enabled.HasValue || billToPayRegistration.Enabled.Value));

            return result;
        }

        public async Task<BillToPayRegistration?> GetById(int iD)
        {
            try
            {
                var result = await _context.BillToPayRegistration!
                    .AsNoTracking()
                    .FirstOrDefaultAsync(billToPayRegistration => billToPayRegistration.Id == iD);

                return result;
            }
            catch (Exception ex)
            {
                Exception exception = new(ex.Message);
                throw exception;
            }
        }

        public async Task<int> Save(BillToPayRegistration billToPayRegistration)
        {
            _context.Add(billToPayRegistration);
            var qtdEntry = await _context.SaveChangesAsync();

            return qtdEntry;
        }

        public async Task<int> Edit(BillToPayRegistration billToPayRegistration)
        {
            _context.ChangeTracker.Clear();

            _context.BillToPayRegistration!.Update(billToPayRegistration);

            var result = _context.SaveChanges();

            return await Task.FromResult(result);
        }

        public async Task<int> DeleteRange(IList<BillToPayRegistration> billToPayRegistrations)
        {
            _context.ChangeTracker.Clear();

            _context.BillToPayRegistration!.RemoveRange(billToPayRegistrations);

            var result = _context.SaveChanges();

            return await Task.FromResult(result);
        }

        public async Task<int> Delete(BillToPayRegistration billToPayRegistration)
        {
            _context.ChangeTracker.Clear();

            _ = _context.BillToPayRegistration!.Remove(billToPayRegistration);

            var result = _context.SaveChanges();

            return await Task.FromResult(result);
        }
    }
}
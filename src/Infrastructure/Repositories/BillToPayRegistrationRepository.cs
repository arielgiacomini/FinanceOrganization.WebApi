using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BillToPayRegistrationRepository : IBillToPayRegistrationRepository
    {
        private readonly FinanceOrganizationContext _context;
        private readonly ICurrentUserService _currentUserService;

        public BillToPayRegistrationRepository(FinanceOrganizationContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
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

        public async Task<BillToPayRegistration?> GetBillToPayRegistrationByName(string? name)
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
            billToPayRegistration.UserId = _currentUserService.UserId ?? Guid.Empty;

            _context.Add(billToPayRegistration);
            var qtdEntry = await _context.SaveChangesAsync();

            return qtdEntry;
        }

        public async Task<int> Edit(BillToPayRegistration billToPayRegistration)
        {
            _context.ChangeTracker.Clear();

            // O handler chamador reconstrói o objeto a partir do input e normalmente não carrega
            // o UserId original — sempre reafirmar aqui, senão a edição zera o dono do registro.
            billToPayRegistration.UserId = _currentUserService.UserId ?? Guid.Empty;

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

        /// <summary>
        /// Busca no repositório todos os registros que já foram cadastrados porém sem ser inseridos na tabela principal.
        /// </summary>
        /// <returns></returns>
        public async Task<IList<BillToPayRegistration>> GetBillToPayNotRegistrationPrincipal()
        {
            var result = await _context.BillToPayRegistration!
            .AsNoTracking()
            .Where(x => !x.LastChangeDate.HasValue)
            .ToListAsync();

            return result;
        }

        public async Task<int> Disable(int id)
        {
            _context.ChangeTracker.Clear();

            // FindAsync ignora o filtro global de UserId (HasQueryFilter) — usar sempre uma
            // query normal aqui, senão qualquer usuário autenticado consegue desabilitar o
            // cadastro de outro usuário só sabendo o Id numérico.
            var billToPayRegistration = await _context.BillToPayRegistration!.FirstOrDefaultAsync(x => x.Id == id);
            if (billToPayRegistration == null)
            {
                return 0;
            }

            billToPayRegistration.Enabled = false;
            billToPayRegistration.LastChangeDate = DateTime.Now;

            _context.BillToPayRegistration!.Update(billToPayRegistration);

            var result = _context.SaveChanges();

            return await Task.FromResult(result);
        }
    }
}
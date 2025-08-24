using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class CashReceivableRegistrationRepository : ICashReceivableRegistrationRepository
{
    private readonly ILogger<CashReceivableRegistrationRepository> _logger;
    private readonly FinanceOrganizationContext _context;

    public CashReceivableRegistrationRepository(ILogger<CashReceivableRegistrationRepository> logger,
            FinanceOrganizationContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<int> Save(CashReceivableRegistration cashReceivable)
    {
        int qtd = 0;
        try
        {
            _context.Add(cashReceivable);
            qtd = await _context.SaveChangesAsync();

            return qtd;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);

            return qtd;
        }
    }

    public async Task<int> Edit(CashReceivableRegistration registration)
    {
        _context.ChangeTracker.Clear();

        _context.CashReceivableRegistration!.Update(registration);

        var result = _context.SaveChanges();

        return await Task.FromResult(result);
    }

    public async Task<IList<CashReceivableRegistration>> GetOnlyOldRecordsAndParticipants(int daysLater, string frequence)
    {
        var result = await _context.CashReceivableRegistration!
               .AsNoTracking()
               .Where(cashRegistration =>
                        (cashRegistration.LastChangeDate == null
                     || (cashRegistration.LastChangeDate <= DateTime.Now.AddDays(daysLater)
                     && cashRegistration.Frequence == frequence)) && (!cashRegistration.Enabled.HasValue || cashRegistration.Enabled.Value))
               .OrderBy(orderBy => orderBy.Id)
               .ToListAsync();

        return result;
    }

    public async Task<CashReceivableRegistration?> GetById(int iD)
    {
        try
        {
            var result = await _context.CashReceivableRegistration!
                .AsNoTracking()
                .FirstOrDefaultAsync(cashReceivableRegistration => cashReceivableRegistration.Id == iD);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            Exception exception = new(ex.Message);
            throw exception;
        }
    }
}
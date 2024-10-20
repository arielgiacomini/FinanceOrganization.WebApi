using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories;

public class CashReceivableRegistrationRepository : ICashReceivableRegistrationRepository
{
    private readonly FinanceOrganizationContext _context;
    private readonly ILogger _logger;

    public CashReceivableRegistrationRepository(ILogger logger,
            FinanceOrganizationContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<int> Save(CashReceivableRegistration cashReceivable)
    {
        _context.Add(cashReceivable);
        var qtdEntry = await _context.SaveChangesAsync();

        return qtdEntry;
    }
}
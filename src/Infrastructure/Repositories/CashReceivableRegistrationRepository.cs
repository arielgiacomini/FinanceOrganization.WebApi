using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database.Context;

namespace Infrastructure.Repositories;

public class CashReceivableRegistrationRepository : ICashReceivableRegistrationRepository
{
    private readonly FinanceOrganizationContext _context;
    private readonly Serilog.ILogger _logger;

    public CashReceivableRegistrationRepository(Serilog.ILogger logger,
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
            _logger.Error(ex, ex.Message);

            return qtd;
        }
    }
}
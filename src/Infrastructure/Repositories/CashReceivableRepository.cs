using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class CashReceivableRepository : ICashReceivableRepository
    {
        private readonly ILogger<CashReceivableRepository> _logger;
        private readonly FinanceOrganizationContext _context;

        public CashReceivableRepository(
            ILogger<CashReceivableRepository> logger,
            FinanceOrganizationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IList<CashReceivable>> GetCashReceivableRegistrationId(int cashReceivableRegistrationId)
        {
            try
            {
                var result = await _context.CashReceivable!
                    .AsNoTracking()
                    .Where(pay => pay.IdCashReceivableRegistration == cashReceivableRegistrationId)
                    .OrderBy(pay => pay.DateReceived)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar o método GetCashReceivableRegistrationId() no Repositório de CashReceivable. Erro: {Message}", ex.Message);
                Exception exception = new(ex.Message);
                throw exception;
            }
        }

        public async Task<int> SaveRange(IList<CashReceivable> cashsReceivable)
        {
            int contador = 0;

            foreach (var cashReceivable in cashsReceivable)
            {
                try
                {
                    _context.Add(cashReceivable);

                    _context.SaveChanges();

                    contador++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao salvar o seguinte CashReceivable: {@cashReceivable}. Erro: {message}", cashReceivable, ex.Message);
                    throw;
                }
            }

            return await Task.FromResult(contador);
        }

        public async Task<int> Edit(CashReceivable cashReceivable)
        {
            _context.ChangeTracker.Clear();

            _context.CashReceivable!.Update(cashReceivable);

            var result = _context.SaveChanges();

            return await Task.FromResult(result);
        }

        public async Task<IList<CashReceivable>> GetAll()
        {
            try
            {
                var result = await _context.CashReceivable!.ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar o método GetAll() no Repositório de CashReceivable. Erro: {Message}", ex.Message);
                Exception exception = new(ex.Message);
                throw exception;
            }
        }

        public async Task<CashReceivable> GetByAccountAndMonthYear(string account, string monthYear)
        {
            try
            {
                var result = await _context.CashReceivable!
                    .AsNoTracking()
                    .FirstOrDefaultAsync(receivable => receivable.YearMonth == monthYear && receivable.Account == account);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar o método GetById() no Repositório de CashReceivable. Erro: {Message}", ex.Message);
                Exception exception = new(ex.Message);
                throw exception;
            }
        }

        public async Task<IList<CashReceivable>> GetByMonthYear(string monthYear)
        {
            try
            {
                var result = await _context.CashReceivable!
                    .AsNoTracking()
                    .Where(receivable => receivable.YearMonth == monthYear)
                    .ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar o método GetByMonthYear() no Repositório de CashReceivable. Erro: {Message}", ex.Message);
                Exception exception = new(ex.Message);
                throw exception;
            }
        }

        public async Task<IList<CashReceivable>> GetByYearMonthAndCategoryAndRegistrationType(string yearMonth, string category, string registationType)
        {
            try
            {
                var result = await _context.CashReceivable!
                    .AsNoTracking()
                    .Where(pay => pay.Category == category
                        && pay.RegistrationType == registationType && pay.YearMonth == yearMonth)
                    .OrderBy(pay => pay.DueDate)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao consultar o método GetByYearMonthAndCategoryAndRegistrationType() no Repositório de CashReceivable. Erro: {Message}", ex.Message);
                Exception exception = new(ex.Message);
                throw exception;
            }
        }

        public async Task<CashReceivable?> GetById(Guid id)
        {
            var cashReceivableOrigin = await _context.CashReceivable!
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.Id == id);

            return cashReceivableOrigin;
        }

        public async Task<int> Save(CashReceivable cashReceivable)
        {
            int contador = 0;

            try
            {
                _context.Add(cashReceivable);

                _context.SaveChanges();

                contador++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar o seguinte CashReceivable: {@cashReceivable}. Erro: {message}", cashReceivable, ex.Message);
                throw;
            }

            return await Task.FromResult(contador);
        }
    }
}
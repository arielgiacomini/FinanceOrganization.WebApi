﻿using Domain.Entities;
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

            foreach (var item in cashsReceivable)
            {
                try
                {
                    _context.Add(item);

                    _context.SaveChanges();

                    contador++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{message}, item: {@item}", ex.Message, item);
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
    }
}
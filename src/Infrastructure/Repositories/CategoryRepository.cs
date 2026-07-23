using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Concurrent;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FinanceOrganizationContext _context;
        private readonly ILogger _logger;
        private readonly ConcurrentDictionary<string, IList<Category>> _returnCategoriesToActions = new();

        public CategoryRepository(
            ILogger logger,
            FinanceOrganizationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IList<Category>?> GetAllAsync(AccountType accountType, bool? filterEnable = null)
        {
            var accountTypeString = accountType.GetDescription();
            var categories = await _context
                .Category!
                .Where(category => category.AccountType == accountTypeString
                        && (filterEnable.HasValue ? category.Enable == filterEnable.Value : (category.Enable == null || category.Enable || !category.Enable)))
                .AsNoTracking()
                .ToListAsync();

            return categories;
        }

        /// <summary>
        /// Determina as ações de adicionar, habilitar e desabilitar categorias com base nas contas a pagar existentes.
        /// </summary>
        /// <returns></returns>
        public async Task<ConcurrentDictionary<string, IList<Category>>> GetCategoriesToActions()
        {
            try
            {
                var categoriesByBillToPay = _context.BillToPay!.AsNoTracking().Select(x => x.Category).Distinct().ToList();
                var categoriesByCashReceivable = _context.CashReceivable!.AsNoTracking().Select(x => x.Category).Distinct().ToList();

                var categoriesBillToPayRegister = await GetAllAsync(AccountType.ContaAPagar);
                var categoriesCashReceivableRegister = await GetAllAsync(AccountType.ContaAReceber);

                #region Passo 1 - Adiciona as categorias que estão na tabela de contas a pagar e não existem na tabela de categorias

                IList<Category> categoriesToAdd = new List<Category>();

                /*
                 * Adiciona as categorias que estão na tabela de contas a pagar e não existem na tabela de categorias
                 */
                foreach (var categoryToAdd in categoriesByBillToPay)
                {
                    var result = categoriesBillToPayRegister?.Where(c => c.Name == categoryToAdd).ToList();

                    if (result == null || result.Count == 0)
                    {
                        categoriesToAdd.Add(new Category { Name = categoryToAdd, Enable = true, CreationDate = DateTime.Now, LastChangeDate = null, AccountType = "Conta a Pagar" });
                    }
                }

                /*
                 * Adiciona as categorias que estão na tabela de contas a receber e não existem na tabela de categorias
                 */
                foreach (var categoryToAdd in categoriesByCashReceivable)
                {
                    var result = categoriesCashReceivableRegister?.Where(c => c.Name == categoryToAdd).ToList();

                    if (result == null || result.Count == 0)
                    {
                        categoriesToAdd.Add(new Category { Name = categoryToAdd, Enable = true, CreationDate = DateTime.Now, LastChangeDate = null, AccountType = "Conta a Receber" });
                    }
                }

                _returnCategoriesToActions.AddOrUpdate("Adicionar", categoriesToAdd, (key, existing) => categoriesToAdd);

                #endregion Passo 1 - Adiciona as categorias que não existem na tabela de categorias

                #region Passo 2 - Habilita as categorias que voltaram a existir na tabela de contas a pagar e que estavam inativadas.

                var categoriesBillToPayRegisterDisables = await GetAllAsync(AccountType.ContaAPagar, false);
                var categoriesCashReceivableRegisterDisables = await GetAllAsync(AccountType.ContaAReceber, false);

                var categoriesToEnable = new List<Category>(categoriesBillToPayRegisterDisables.Count + categoriesCashReceivableRegisterDisables.Count);

                /*                  
                 * Habilita as categorias que voltaram a existir na tabela de contas a pagar e que estavam inativadas.
                 */
                foreach (var categoryToEnable in categoriesBillToPayRegisterDisables)
                {
                    var result = categoriesByBillToPay?.Where(c => c == categoryToEnable.Name).ToList();

                    if (result.Count > 0)
                    {
                        categoriesToEnable.Add(categoryToEnable);
                    }
                }

                /*
                 * Habilita as categorias que voltaram a existir na tabela de contas a receber e que estavam inativadas.
                 */
                foreach (var categoryToEnable in categoriesCashReceivableRegisterDisables)
                {
                    var result = categoriesByCashReceivable?.Where(c => c == categoryToEnable.Name).ToList();

                    if (result.Count > 0)
                    {
                        categoriesToEnable.Add(categoryToEnable);
                    }
                }

                _returnCategoriesToActions.AddOrUpdate("Habilitar", categoriesToEnable, (key, existing) => categoriesToEnable);

                #endregion Passo 2 - Habilita as categorias que voltaram a existir na tabela de contas a pagar

                #region Passo 3 - Desabilita as categorias que não existem mais na tabela de contas a pagar.

                var categoriesToDisable = new List<Category>(categoriesBillToPayRegister.Count + categoriesCashReceivableRegister.Count);

                foreach (var categoryToDisable in categoriesBillToPayRegister)
                {
                    if (categoryToDisable.Enable)
                    {
                        var resultBillToPay = categoriesByBillToPay?.Where(c => c == categoryToDisable.Name).ToList();
                        var resultCashReceivable = categoriesByCashReceivable?.Where(c => c == categoryToDisable.Name).ToList();

                        if ((resultBillToPay == null || resultBillToPay.Count == 0) && (resultCashReceivable == null || resultCashReceivable.Count == 0))
                        {
                            categoriesToDisable.Add(categoryToDisable);
                        }
                    }
                }

                _returnCategoriesToActions.AddOrUpdate("Desabilitar", categoriesToDisable, (key, existing) => categoriesToDisable);

                #endregion Passo 3 - Desabilita as categorias que não existem mais na tabela de contas a pagar

                return _returnCategoriesToActions;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{message}", ex.Message);
                throw;
            }
        }

        public async Task<bool> SetEnableOrDisableCategoryByRange(IList<Category> categoriesNotExists, bool enable)
        {
            var categoriesToAction = new List<Category>();
            foreach (var category in categoriesNotExists)
            {
                category.Enable = enable;
                category.LastChangeDate = DateTime.Now;
                categoriesToAction.Add(category);
            }

            var result = await UpdateRange(categoriesToAction);

            return result > 1;
        }

        public async Task<int> SaveRange(IList<Category> categories)
        {
            int contador = 0;

            foreach (var category in categories)
            {
                try
                {
                    _context.Add(category);

                    _context.SaveChanges();

                    contador++;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "{message}, item: {@item}", ex.Message, category);
                    throw;
                }
            }

            return await Task.FromResult(contador);
        }

        public async Task<int> UpdateRange(IList<Category> categories)
        {
            int contador = 0;
            foreach (var category in categories)
            {
                try
                {
                    _context.Update(category);
                    _context.SaveChanges();
                    contador++;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "{message}, item: {@item}", ex.Message, category);
                    throw;
                }
            }
            return await Task.FromResult(contador);
        }

        public void ClearTempCategories() => _returnCategoriesToActions.Clear();
    }
}
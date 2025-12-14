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
                IList<Category> categoriesToAdd = new List<Category>();

                var categoriesByBillToPay = _context.BillToPay!.AsNoTracking().Select(x => x.Category).Distinct().ToList();

                var categoriesRegister = await GetAllAsync(AccountType.ContaAPagar);

                #region Passo 1 - Adiciona as categorias que estão na tabela de contas a pagar e não existem na tabela de categorias

                foreach (var categoryNonRegister in categoriesByBillToPay)
                {
                    var result = categoriesRegister!.FirstOrDefault(x => x.Name == categoryNonRegister);

                    if (result == null)
                    {
                        categoriesToAdd.Add(new Category { Name = categoryNonRegister, Enable = true, CreationDate = DateTime.Now, LastChangeDate = null, AccountType = "Conta a Pagar" });
                    }
                }

                _returnCategoriesToActions.AddOrUpdate("Adicionar", categoriesToAdd, (key, existing) => categoriesToAdd);

                #endregion Passo 1 - Adiciona as categorias que não existem na tabela de categorias

                #region Passo 2 - Habilita as categorias que voltaram a existir na tabela de contas a pagar e que estavam inativadas.

                var categoriesRegisterDisables = await GetAllAsync(AccountType.ContaAPagar, false);

                var categoriesToEnable = new List<Category>(categoriesRegisterDisables.Count);

                foreach (var categoryToEnable in categoriesRegisterDisables)
                {
                    var result = categoriesByBillToPay!.FirstOrDefault(x => x.Equals(categoryToEnable.Name));

                    if (result != null)
                    {
                        categoriesToEnable.Add(categoryToEnable);
                    }
                }

                _returnCategoriesToActions.AddOrUpdate("Habilitar", categoriesToEnable, (key, existing) => categoriesToEnable);

                #endregion Passo 2 - Habilita as categorias que voltaram a existir na tabela de contas a pagar

                #region Passo 3 - Desabilita as categorias que não existem mais na tabela de contas a pagar.

                var categoriesToDisable = new List<Category>(categoriesRegister.Count);
                foreach (var categoryToDisable in categoriesRegister)
                {
                    if (categoryToDisable.Enable)
                    {
                        var result = categoriesByBillToPay!.FirstOrDefault(x => x.Equals(categoryToDisable.Name));

                        if (result == null)
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

            return result > 1 ? true : false;
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
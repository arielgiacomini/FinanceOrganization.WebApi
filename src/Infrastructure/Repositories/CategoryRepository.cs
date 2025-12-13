using Domain.Entities;
using Domain.Entities.Enums;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly FinanceOrganizationContext _context;
        private readonly ILogger _logger;

        public CategoryRepository(
            ILogger logger,
            FinanceOrganizationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IList<Category>?> GetAllAsync(AccountType accountType)
        {
            var accountTypeString = accountType.GetDescription();
            var categories = await _context
                .Category!
                .Where(accountType => accountType.AccountType == accountTypeString)
                .AsNoTracking()
                .ToListAsync();

            return categories;
        }

        public async Task<IList<Category>> GetNonRegister()
        {
            try
            {
                IList<Category> categoriesResult = new List<Category>();

                var categoriesNonRegister = _context.BillToPay!.AsNoTracking().Select(x => x.Category).Distinct().ToList();

                var categoriesRegister = await GetAllAsync(AccountType.ContaAPagar);

                foreach (var categoryNonRegister in categoriesNonRegister)
                {
                    var result = categoriesRegister!.FirstOrDefault(x => x.Name == categoryNonRegister);

                    if (result == null)
                    {
                        categoriesResult.Add(new Category { Name = categoryNonRegister, Enable = true, CreationDate = DateTime.Now, LastChangeDate = null, AccountType = "Conta a Pagar" });
                    }
                }

                return categoriesResult;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "{message}", ex.Message);
                throw;
            }
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
    }
}
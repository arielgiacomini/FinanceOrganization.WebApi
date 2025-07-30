using Domain.Entities;
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

        public CategoryRepository(ILogger logger,
            FinanceOrganizationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IList<Category>?> GetAllAsync()
        {
            var categories = await _context
                .Category!
                .AsNoTracking()
                .ToListAsync();

            return categories;
        }

        public async Task<IList<string>?> GetNonRegister()
        {
            try
            {
                IList<string>? categoriesResult = new List<string>();

                var categoriesNonRegister = _context.BillToPay!.AsNoTracking().Select(x => x.Category).Distinct().ToList();

                var categoriesRegister = await GetAllAsync();

                foreach (var itemNonRegister in categoriesNonRegister)
                {
                    var result = categoriesRegister!.FirstOrDefault(x => x.Name == itemNonRegister);

                    if (result == null)
                    {
                        categoriesResult.Add(itemNonRegister!);
                    }
                }

                return categoriesResult;
            }
            catch (Exception ex)
            {
                throw ex;
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
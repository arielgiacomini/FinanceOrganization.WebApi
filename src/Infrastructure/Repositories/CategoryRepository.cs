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
            var categories = await _context.Category!.ToListAsync();

            return categories;
        }
    }
}
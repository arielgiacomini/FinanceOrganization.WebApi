using Domain.Entities.Dashboard;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ILogger<DashboardRepository> _logger;
        private readonly FinanceOrganizationContext _context;

        public DashboardRepository(ILogger<DashboardRepository> logger, FinanceOrganizationContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// Busca os dados para montar o dashboard de categoria e valor por mês/ano e categoria.
        /// </summary>
        /// <param name="yearMonth"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public async Task<IList<DailyExpenseByCategoryDateDashboard>> GetDashboardBillToPayCategoryAndValueByMonthYearAndCategory(
            string? yearMonth, string? category)
        {
            QuerySqlDailyExpenseByCategoryDateDashboard query = new(yearMonth, category);

            var result = await _context
                .Set<DailyExpenseByCategoryDateDashboard>()
                .FromSqlInterpolated(query.Sql)
                .ToListAsync();

            return result;
        }
    }
}
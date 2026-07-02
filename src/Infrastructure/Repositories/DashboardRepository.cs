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
        public async Task<IList<DailyGoalExpenseByCategoryDateDashboard>> GetDashboardBillToPayCategoryAndValueByMonthYearAndCategory(
            string? yearMonth, string? category)
        {
            QuerySqlDailyExpenseByCategoryDateDashboard query = new(yearMonth, category);

            var result = await _context
                .Set<DailyGoalExpenseByCategoryDateDashboard>()
                .FromSqlInterpolated(query.Sql)
                .ToListAsync();

            return result;
        }

        public async Task<IList<MonthlyCashFlowDashboard>> GetDashboardMonthlyCashFlowByMonthYear(string? years, string? months, string? foodVoucher, string? loanNextMonths)
        {
            try
            {
                QuerySqlMonthlyCashFlowDashboard query = new(years, months, foodVoucher, loanNextMonths);

                var result = await _context
                    .Set<MonthlyCashFlowDashboard>()
                    .FromSqlInterpolated(query.Sql)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching monthly cash flow dashboard data for years: {Years} and months: {Months}", years, months);
                throw;
            }
        }

        public async Task<IList<DailyExpenseByCategoryAndAccountDateDashboard>> GetDailyExpenseByCategoryAndAccountDateDashboard(string? years, string? months, string? category)
        {
            try
            {
                QuerySqlDailyExpenseByCategoryAndAccountDateDashboard query = new(years, months, category);

                var result = await _context
                    .Set<DailyExpenseByCategoryAndAccountDateDashboard>()
                    .FromSqlInterpolated(query.Sql)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching daily expense dashboard data for years: {Years} and months: {Months}", years, months);
                throw;
            }
        }
    }
}
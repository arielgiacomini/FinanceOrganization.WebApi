
using Domain.Entities.Dashboard;

namespace Domain.Interfaces
{
    public interface IDashboardRepository
    {
        Task<IList<DailyExpenseByCategoryDateDashboard>> GetDashboardBillToPayCategoryAndValueByMonthYearAndCategory(string? yearMonth, string? category);
        Task<IList<MonthlyCashFlowDashboard>> GetDashboardMonthlyCashFlowByMonthYear(string? years, string? months);
    }
}
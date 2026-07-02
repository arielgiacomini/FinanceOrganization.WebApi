
using Domain.Entities.Dashboard;

namespace Domain.Interfaces
{
    public interface IDashboardRepository
    {
        Task<IList<DailyExpenseByCategoryAndAccountDateDashboard>> GetDailyExpenseByCategoryAndAccountDateDashboard(string? years, string? months, string? category);
        Task<IList<DailyGoalExpenseByCategoryDateDashboard>> GetDashboardBillToPayCategoryAndValueByMonthYearAndCategory(string? yearMonth, string? category);
        Task<IList<MonthlyCashFlowDashboard>> GetDashboardMonthlyCashFlowByMonthYear(string? years, string? months, string? foodVoucher, string? loanNextMonths);
    }
}
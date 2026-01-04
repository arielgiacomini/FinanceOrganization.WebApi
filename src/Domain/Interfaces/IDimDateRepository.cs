using Domain.Entities.Extern;

namespace Domain.Interfaces
{
    public interface IDimDateRepository
    {
        Task<IList<DimDate>> GetAllAsync();
        Task<IList<string>> GetAllGroupedByMonthYear();
        Task<IList<DimDate>> GetByDate(DateTime date);
        Task<IList<DimDate>> GetByMonthYear(string monthYear);
        Task<IList<string>> GetByStartEndYearGroupedByMonthYear(int startYear, int endYear);
        Task<IList<DimDate>> GetByYear(int year);
    }
}
using Domain.Entities.Extern;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class DimDateRepository : IDimDateRepository
    {
        private readonly ILogger<DimDateRepository> _logger;
        private readonly FinanceOrganizationContext _context;

        public DimDateRepository(ILogger<DimDateRepository> logger, FinanceOrganizationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IList<DimDate>> GetAllAsync()
        {
            var dimData = await _context
                .DimDate!
                .AsNoTracking()
                .ToListAsync();

            return dimData;
        }

        public async Task<IList<DimDate>> GetByYear(int year)
        {
            var dimData = await _context
                .DimDate!
                .Where(y => y.Year == year)
                .AsNoTracking()
                .ToListAsync();

            return dimData;
        }

        public async Task<IList<DimDate>> GetByDate(DateTime date)
        {
            var dimData = await _context
                .DimDate!
                .Where(y => y.Date == date)
                .AsNoTracking()
                .ToListAsync();

            return dimData;
        }

        public async Task<IList<DimDate>> GetByMonthYear(string monthYear)
        {
            var dimData = await _context
                .DimDate!
                .Where(y => y.MonthYear == monthYear)
                .AsNoTracking()
                .ToListAsync();

            return dimData;
        }

        public async Task<IList<string>> GetAllGroupedByMonthYear()
        {
            var monthYears = await _context
                .DimDate!
                .GroupBy(g => g.MonthYear)
                .Select(g => g.Key)
                .AsNoTracking()
                .ToListAsync();

            return monthYears;
        }

        public async Task<IList<string>> GetByStartEndYearGroupedByMonthYear(int startYear, int endYear)
        {
            var filteredOrder = await _context
                .DimDate!
                .Where(x => x.Year >= startYear && x.Year <= endYear)
                .OrderBy(g => g.Date)
                .AsNoTracking()
                .ToListAsync();

            var monthYears = filteredOrder
                .GroupBy(g => g.MonthYear)
                .Select(g => g.Key)
                .ToList();

            return monthYears;
        }
    }
}
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("v1/dashboard")]
    [Produces("application/json")]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(ILogger<DashboardController> logger, IDashboardRepository dashboardRepository)
        {
            _logger = logger;
            _dashboardRepository = dashboardRepository;
        }

        /// <summary>
        /// Busca os dados do dashboard Contas a Pagar por dia/semana e categoria
        /// </summary>
        /// <param name="mesAno"></param>
        /// <param name="categoria"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("billToPay-day-week-category")]
        public async Task<IActionResult> GetDashboardBillToPayCategoryAndValueByMonthYearAndCategory([FromQuery] string? mesAno, string? categoria, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Dashboard.GetDashboardCategoryAndValueByMonthYearAndCategory - Busca de Dados");

            var output = await _dashboardRepository.GetDashboardBillToPayCategoryAndValueByMonthYearAndCategory(mesAno, categoria);

            return Ok(output);
        }

        /// <summary>
        /// Busca os dados do dashboard Contas a Pagar/Receber por mês/ano, quantidade e valor.
        /// </summary>
        /// <param name="mesAno"></param>
        /// <param name="categoria"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("monthly-cashflow-billtopay-cashreceivable")]
        public async Task<IActionResult> GetDashboardMonthlyCashFlowByMonthYear([FromQuery] string? years, string? months, string? foodVoucher, string? loanNextMonths, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Dashboard.GetDashboardMonthlyCashFlowByMonthYear - Busca de Dados");

            var output = await _dashboardRepository.GetDashboardMonthlyCashFlowByMonthYear(years, months, foodVoucher, loanNextMonths);
            return Ok(output);
        }

        /// <summary>
        /// Busca os dados do dashboard Contas a Pagar por Categoria e Conta Bancária Diario
        /// </summary>
        /// <param name="mesAno"></param>
        /// <param name="categoria"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("daily-expense-category-account-date")]
        public async Task<IActionResult> GetDashboardDailyExpenseByCategoryAndAccountDate([FromQuery] string? years, string? months, string? category, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Dashboard.GetDashboardDailyExpenseByCategoryAndAccountDate - Busca de Dados");

            var output = await _dashboardRepository.GetDailyExpenseByCategoryAndAccountDateDashboard(years, months, category);
            return Ok(output);
        }
    }
}
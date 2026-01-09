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
        public async Task<IActionResult> GetDashboardBillToPayCategoryAndValueByMonthYearAndCategory([FromHeader] string? mesAno, string? categoria, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Dashboard.GetDashboardCategoryAndValueByMonthYearAndCategory - Busca de Dados");

            var output = await _dashboardRepository.GetDashboardBillToPayCategoryAndValueByMonthYearAndCategory(mesAno, categoria);

            return Ok(output);
        }
    }
}
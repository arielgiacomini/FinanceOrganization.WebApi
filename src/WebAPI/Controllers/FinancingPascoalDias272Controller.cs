using Application.Feature.RealEstateFinancing.SearchFinancingPascoalDias272;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("v1/realEstateFinancing")]
    [Produces("application/json")]
    public class FinancingPascoalDias272Controller : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly ISearchFinancingPascoalDias272Handler _financingPascoalDias272Handler; 

        public FinancingPascoalDias272Controller(Serilog.ILogger logger,
        ISearchFinancingPascoalDias272Handler financingPascoalDias272Handler)
        {
            _logger = logger;
            _financingPascoalDias272Handler = financingPascoalDias272Handler;
        }

        [HttpGet("search-all-272")]
        public async Task<IActionResult> GetFinancingPascoalDias272(CancellationToken cancellationToken)
        {
            _logger.Information($"[FinancingPascoalDias272Controller.GetFinancingPascoalDias272()] - Busca de todos os financiamentos com todos os atributos");

            var output = await _financingPascoalDias272Handler.Handle();

            return Ok(output);
        }
    }
}
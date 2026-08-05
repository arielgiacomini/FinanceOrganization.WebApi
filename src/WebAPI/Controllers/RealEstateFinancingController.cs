using Application.Feature.RealEstateFinancing.SearchRealEstateFinancing;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("v1/realEstateFinancing")]
    [Produces("application/json")]
    public class RealEstateFinancingController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly ISearchRealEstateFinancingHandler _searchRealEstateFinancingHandler;

        public RealEstateFinancingController(Serilog.ILogger logger,
           ISearchRealEstateFinancingHandler searchRealEstateFinancingHandler)
        {
            _logger = logger;
            _searchRealEstateFinancingHandler = searchRealEstateFinancingHandler;
            
        }
        /// <summary>
        /// Busca de Todos os financiamentos imobiliarios com todos os atributos
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("search-all")]
        public async Task<IActionResult> GetRealEstateFinancing(CancellationToken cancellationToken)
        {
            _logger.Information($"[RealEstateFinancingController.GetRealEstateFinancing()] - Busca de todos os financiamentos com todos os atributos");

            var output = await _searchRealEstateFinancingHandler.Handle();

            return Ok(output);
        }
    }
}

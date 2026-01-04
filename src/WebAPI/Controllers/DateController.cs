using Application.Feature.Date.SearchAllWithFilters;
using Application.Feature.Date.SearchMonthYear;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("v1/date")]
    [Produces("application/json")]
    public class DateController : ControllerBase
    {
        private readonly ILogger<DateController> _logger;
        private readonly ISearchDateAllWithFiltersHandler _searchDateAllWithFiltersHandler;
        private readonly ISearchMonthYearHandler _searchDateMonthYearHandler;

        public DateController(ILogger<DateController> logger, ISearchDateAllWithFiltersHandler searchCategoryHandler, ISearchMonthYearHandler searchMonthYear)
        {
            _logger = logger;
            _searchDateAllWithFiltersHandler = searchCategoryHandler;
            _searchDateMonthYearHandler = searchMonthYear;
        }

        /// <summary>
        /// Busca todos os registros de datas disponíveis
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromHeader] DateTime? date, int? year, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Busca de todas as datas disponíveis.");

            var input = new SearchDateAllWithFiltersInput { Date = date, Year = year };

            var output = await _searchDateAllWithFiltersHandler.Handle(input, cancellationToken);

            return Ok(output);
        }

        /// <summary>
        /// Busca todos os MesAno disponíveis
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("month-year-all")]
        public async Task<IActionResult> GetMonthYear([FromHeader] int? startYear, int? endYear, CancellationToken cancellationToken)
        {
            var input = new SearchMonthYearInput { StartYear = startYear, EndYear = endYear };

            var output = await _searchDateMonthYearHandler.Handle(input, cancellationToken);

            return Ok(output);
        }
    }
}
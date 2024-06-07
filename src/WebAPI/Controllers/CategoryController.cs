using Application.Feature.Category.SearchCategory;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("v1/category")]
    [Produces("application/json")]
    public class CategoryController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly ISearchCategoryHandler _searchCategoryHandler;

        public CategoryController(
            Serilog.ILogger logger,
            ISearchCategoryHandler searchCategoryHandler)
        {
            _logger = logger;
            _searchCategoryHandler = searchCategoryHandler;
        }

        /// <summary>
        /// Busca do registro de uma conta a pagar
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<IActionResult> GetCategory(CancellationToken cancellationToken)
        {
            _logger.Information($"[CategoryController.GetCategory()] - Busca de todas as categorias disponíveis");

            var output = await _searchCategoryHandler.Handle();

            return Ok(output);
        }
    }
}
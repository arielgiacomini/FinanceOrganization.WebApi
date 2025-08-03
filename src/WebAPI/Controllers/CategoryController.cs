using Application.Feature.Category.SearchCategory;
using Domain.Entities.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("v1/category")]
    [Produces("application/json")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ISearchCategoryHandler _searchCategoryHandler;

        public CategoryController(
            ILogger<CategoryController> logger,
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
        public async Task<IActionResult> GetCategory([FromHeader] string? accountType, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Busca de todas as categorias disponíveis com o parâmetro: {accountType}", accountType);

            var input = new SearchCategoryInput
            {
                AccountType = GetAccountTypeByString(accountType)
            };

            var output = await _searchCategoryHandler.Handle(input);

            return Ok(output);
        }

        private AccountType GetAccountTypeByString(string? headerInput)
        {
            AccountType accountType;

            if (string.IsNullOrWhiteSpace(headerInput))
            {
                accountType = AccountType.ContaAPagar;
            }
            else if (headerInput.Contains("Pagar", StringComparison.OrdinalIgnoreCase))
            {
                accountType = AccountType.ContaAPagar;
            }
            else if (headerInput.Contains("Receber", StringComparison.OrdinalIgnoreCase))
            {
                accountType = AccountType.ContaAReceber;
            }
            else
            {
                accountType = AccountType.ContaAPagar;
                _logger.LogWarning("Tipo de conta não reconhecido: {headerInput}", headerInput);
            }

            return accountType;
        }
    }
}
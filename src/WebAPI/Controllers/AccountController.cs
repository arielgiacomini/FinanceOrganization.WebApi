using Application.Feature.Account.CreateAccount;
using Application.Feature.Account.DeleteAccount;
using Application.Feature.Account.EditAccount;
using Application.Feature.Account.SearchAccount;
using Application.Feature.Account.SearchAccountOnlyName;
using Domain.Utils;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("v1/account")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly ISearchAccountOnlyNameHandler _searchAccountOnlyNameHandler;
        private readonly ISearchAccountHandler _searchAccountHandler;
        private readonly ICreateAccountHandler _createAccountHandler;
        private readonly IEditAccountHandler _editAccountHandler;
        private readonly IDeleteAccountHandler _deleteAccountHandler;

        public AccountController(
            Serilog.ILogger logger,
            ISearchAccountOnlyNameHandler searchAccountOnlyNameHandler,
            ISearchAccountHandler searchAccountHandler,
            ICreateAccountHandler createAccountHandler,
            IEditAccountHandler editAccountHandler,
            IDeleteAccountHandler deleteAccountHandler)
        {
            _logger = logger;
            _searchAccountOnlyNameHandler = searchAccountOnlyNameHandler;
            _searchAccountHandler = searchAccountHandler;
            _createAccountHandler = createAccountHandler;
            _editAccountHandler = editAccountHandler;
            _deleteAccountHandler = deleteAccountHandler;
        }

        /// <summary>
        /// Cadastro de uma nova conta
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountInput input,
                    CancellationToken cancellationToken)
        {
            _logger.Information($"[AccountController.CreateAccount()] - Cadastro de uma nova conta. Input: {JsonSerializeUtils.Serialize(input)}");

            var output = await _createAccountHandler.Handle(input, cancellationToken);

            if (output.Output.Status == Application.Feature.OutputBaseDetails.OutputStatus.HasValidationIssue)
            {
                return BadRequest(output);
            }

            return Ok(output);
        }

        /// <summary>
        /// Edita uma conta já cadastrada
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("edit")]
        public async Task<IActionResult> EditAccount([FromBody] EditAccountInput input,
                    CancellationToken cancellationToken)
        {
            _logger.Information($"[AccountController.EditAccount()] - Alteração de uma conta já cadastrada. Input: {JsonSerializeUtils.Serialize(input)}");

            var output = await _editAccountHandler.Handle(input, cancellationToken);

            if (output.Output.Status == Application.Feature.OutputBaseDetails.OutputStatus.HasValidationIssue)
            {
                return BadRequest(output);
            }

            return Ok(output);
        }

        /// <summary>
        /// Deleta uma conta já cadastrada
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountInput input,
                    CancellationToken cancellationToken)
        {
            _logger.Information($"[AccountController.DeleteAccount()] - Exclusão de uma conta já cadastrada. Input: {JsonSerializeUtils.Serialize(input)}");

            var output = await _deleteAccountHandler.Handle(input, cancellationToken);

            if (output.Output.Status == Application.Feature.OutputBaseDetails.OutputStatus.HasValidationIssue)
            {
                return BadRequest(output);
            }

            return Ok(output);
        }

        /// <summary>
        /// Busca de Todas as Contas dísponíveis porém só os nomes
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("search-only-name")]
        public async Task<IActionResult> GetSearchAccountName(CancellationToken cancellationToken)
        {
            _logger.Information($"[AccountController.GetSearchAccountName()] - Busca de todas as contas apenas o nome disponíveis");

            var output = await _searchAccountOnlyNameHandler.Handle();

            return Ok(output);
        }

        /// <summary>
        /// Busca de Todas as Contas dísponíveis com todos os atributos
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("search-all")]
        public async Task<IActionResult> GetSearchAccountAll(CancellationToken cancellationToken)
        {
            _logger.Information($"[AccountController.GetSearchAccountAll()] - Busca de todas as contas com todos os atributos");

            var output = await _searchAccountHandler.Handle();

            return Ok(output);
        }
    }
}
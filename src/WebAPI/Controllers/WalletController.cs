using Application.Feature.Wallet.CreateWallet;
using Application.Feature.Wallet.EditWallet;
using Application.Feature.Wallet.SearchWallet;
using Domain.Utils;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("v1/wallet")]
    [Produces("application/json")]
    public class WalletController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly ICreateWalletHandler _createWalletHandler;
        private readonly IEditWalletHandler _editWalletHandler;
        private readonly ISearchWalletHandler _searchWalletHandler;

        public WalletController(Serilog.ILogger logger, ICreateWalletHandler createWalletHandler, IEditWalletHandler editWalletHandler, ISearchWalletHandler searchWalletHandler)
        {
            _logger = logger;
            _createWalletHandler = createWalletHandler;
            _editWalletHandler = editWalletHandler;
            _searchWalletHandler = searchWalletHandler;
        }

        /// <summary>
        /// Criação de uma nova carteira.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> CreateWallet([FromBody] CreateWalletInput input,
                    CancellationToken cancellationToken)
        {
            _logger.Information($"[WalletController.CreateWallet()] - Cadastro de uma nova carteira. Input: {JsonSerializeUtils.Serialize(input)}");

            var output = await _createWalletHandler.Handle(input, cancellationToken);

            if (output.Output.Status == Application.Feature.OutputBaseDetails.OutputStatus.HasValidationIssue)
            {
                return BadRequest(output);
            }

            return Ok(output);
        }

        /// <summary>
        /// Editar uma carteira já cadastrada.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("edit")]
        public async Task<IActionResult> EditWallet([FromBody] EditWalletInput input,
            CancellationToken cancellationToken)
        {
            _logger.Information($"[WalletController.EditWallet()] - Alteração de uma carteira já cadastrada. Input: {JsonSerializeUtils.Serialize(input)}");

            var output = await _editWalletHandler.Handle(input, cancellationToken);

            if (output.Output.Status == Application.Feature.OutputBaseDetails.OutputStatus.HasValidationIssue)
            {
                return BadRequest(output);
            }

            return Ok(output);
        }

        /// <summary>
        /// Busca de carteiras cadastradas, podendo ser filtrada por chave da carteira.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("search")]
        public async Task<IActionResult> SearchWallet([FromBody] SearchWalletInput input,
            CancellationToken cancellationToken)
        {
            _logger.Information($"[WalletController.SearchWallet()] - Busca de carteiras. Input: {JsonSerializeUtils.Serialize(input)}");

            var output = await _searchWalletHandler.Handle(input, cancellationToken);

            return Ok(output);
        }
    }
}
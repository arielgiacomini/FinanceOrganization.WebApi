using Application.Feature;
using Application.Feature.CashReceivable.EditCashReceivable;
using Application.Feature.CashReceivable.SearchCashReceivable;
using Application.Feature.CashReceivableRegistration.CreateCashReceivableRegistration;
using Domain.Utils;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("v1/cash-receivable")]
    [Produces("application/json")]
    public class CashReceivableController : ControllerBase
    {
        private readonly ILogger<CashReceivableController> _logger;
        private readonly ICreateCashReceivableRegistrationHandler _createCashReceivableHandler;
        private readonly ISearchCashReceivableHandler _searchCashReceivableHandler;
        private readonly IEditCashReceivableHandler _editCashReceivableHandler;

        public CashReceivableController(
            ILogger<CashReceivableController> logger,
            ICreateCashReceivableRegistrationHandler createCashReceivableHandler,
            ISearchCashReceivableHandler searchCashReceivableHandler,
            IEditCashReceivableHandler editCashReceivableHandler)
        {
            _logger = logger;
            _createCashReceivableHandler = createCashReceivableHandler;
            _searchCashReceivableHandler = searchCashReceivableHandler;
            _editCashReceivableHandler = editCashReceivableHandler;
        }

        /// <summary>
        /// Registrar uma conta a receber
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> CashReceivableRegistration([FromBody] CreateCashReceivableRegistrationInput input,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[CashReceivableController.CashReceivableRegistration()] - Cadastro de uma nova conta a receber. Input: {JsonSerializeUtils.Serialize(input)}");

            var output = await _createCashReceivableHandler.Handle(input, cancellationToken);

            return Ok(output);
        }

        /// <summary>
        /// Registrar uma lista de contas a receber
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("register-basket")]
        public async Task<IActionResult> CreateBasketCashReceivable([FromBody] IList<CreateCashReceivableRegistrationInput> input,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("[CashReceivableController.CreateBasketCashReceivable()] - Cadastro de uma nova conta a receber via basket. Input: {@Input}", JsonSerializeUtils.Serialize(input));

            List<CreateCashReceivableRegistrationOutput> outputList = new();
            CreateCashReceivableRegistrationOutput output = new();

            foreach (var createCashReceivable in input)
            {
                outputList.Add(await _createCashReceivableHandler.Handle(createCashReceivable, cancellationToken));
            }

            output.Output = OutputBaseDetails.Success("A lista de contas a receber para cadastro teve exito.", outputList, outputList.Count);

            return Ok(output);
        }

        /// <summary>
        /// Busca de contas a receber
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("search")]
        public async Task<IActionResult> GetCashReceivable([FromBody] SearchCashReceivableInput input,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Busca de contar a receber");

            var output = await _searchCashReceivableHandler.Handle(input, cancellationToken);

            return Ok(output);
        }

        /// <summary>
        /// Edita uma conta a receber
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("edit")]
        public async Task<IActionResult> EditCashReceivable([FromBody] EditCashReceivableInput input,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Edição de uma conta a receber");

            var output = await _editCashReceivableHandler.Handle(input, cancellationToken);

            return Ok(output);
        }
    }
}
using Application.Feature;
using Application.Feature.CashReceivable.DeleteCashReceivable;
using Application.Feature.CashReceivable.EditCashReceivable;
using Application.Feature.CashReceivable.SearchCashReceivable;
using Application.Feature.CashReceivableRegistration.CreateCashReceivableRegistration;
using Application.Feature.CashReceivableRegistration.DisableCashReceivableRegistration;
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
        private readonly IDeleteCashReceivableHandler _deleteBillToPayHandler;
        private readonly IDisableCashReceivableRegistrationHandler _disableCashReceivableRegistrationHandler;

        public CashReceivableController(
            ILogger<CashReceivableController> logger,
            ICreateCashReceivableRegistrationHandler createCashReceivableHandler,
            ISearchCashReceivableHandler searchCashReceivableHandler,
            IEditCashReceivableHandler editCashReceivableHandler,
            IDeleteCashReceivableHandler deleteBillToPayHandler,
            IDisableCashReceivableRegistrationHandler disableCashReceivableRegistrationHandler)
        {
            _logger = logger;
            _createCashReceivableHandler = createCashReceivableHandler;
            _searchCashReceivableHandler = searchCashReceivableHandler;
            _editCashReceivableHandler = editCashReceivableHandler;
            _deleteBillToPayHandler = deleteBillToPayHandler;
            _disableCashReceivableRegistrationHandler = disableCashReceivableRegistrationHandler;
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

        /// <summary>
        /// Edita um lote de contas a pagar
        /// </summary>
        /// <param name="edits"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("edit-basket")]
        public async Task<IActionResult> EditBasketCashReceivable([FromBody] IList<EditCashReceivableInput> edits,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Alteração de Contas a Receber via Lote. Input: {@Inputs}", edits);

            List<EditCashReceivableOutput> outputList = new();
            EditCashReceivableOutput output = new();

            foreach (var input in edits)
            {
                outputList.Add(await _editCashReceivableHandler.Handle(input, cancellationToken));
            }

            output.Output = OutputBaseDetails.Success("A edição em lote de Contas a Receber foi realizada com sucesso.", outputList, outputList.Count);

            return Ok(output);
        }

        /// <summary>
        /// Deleta uma conta a receber
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteCashReceivable([FromBody] DeleteCashReceivableInput input,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleta registro de uma conta a receber. Input: {@Inputs}", input);

            var output = await _deleteBillToPayHandler.Handle(input, cancellationToken);

            return Ok(output);
        }

        /// <summary>
        /// Desabilita uma conta a receber
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("disable-registration")]
        public async Task<IActionResult> DisableCashReceivableRegistration([FromBody] DisableCashReceivableRegistrationInput input,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Dabilitando uma conta a receber. Input: {@Inputs}", input);

            var output = await _disableCashReceivableRegistrationHandler.Handle(input, cancellationToken);

            return Ok(output);
        }
    }
}
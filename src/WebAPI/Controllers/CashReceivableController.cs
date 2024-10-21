using Application.Feature;
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
        private readonly Serilog.ILogger _logger;
        private readonly ICreateCashReceivableRegistrationHandler _createCashReceivableHandler;

        public CashReceivableController(
            Serilog.ILogger logger,
            ICreateCashReceivableRegistrationHandler createCashReceivableHandler)
        {
            _logger = logger;
            _createCashReceivableHandler = createCashReceivableHandler;
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
            _logger.Information($"[CashReceivableController.CashReceivableRegistration()] - Cadastro de uma nova conta a receber. Input: {JsonSerializeUtils.Serialize(input)}");

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
            _logger.Information("[CashReceivableController.CreateBasketCashReceivable()] - Cadastro de uma nova conta a receber via basket. Input: {@Input}", JsonSerializeUtils.Serialize(input));

            List<CreateCashReceivableRegistrationOutput> outputList = new();
            CreateCashReceivableRegistrationOutput output = new();

            foreach (var createCashReceivable in input)
            {
                outputList.Add(await _createCashReceivableHandler.Handle(createCashReceivable, cancellationToken));
            }

            output.Output = OutputBaseDetails.Success("A lista de contas a receber para cadastro teve exito.", outputList, outputList.Count);

            return Ok(output);
        }
    }
}
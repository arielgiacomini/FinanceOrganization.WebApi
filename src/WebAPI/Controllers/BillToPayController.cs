using Application.Feature.BillToPay.EditBillToPay;
using Application.Feature.BillToPay.PayBillToPay;
using Application.Feature.FixedInvoice.CreateFixedInvoice;
using Application.Feature.FixedInvoice.SearchFixedInvoice;
using Domain.Utils;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("v1/bills-to-pay")]
    [Produces("application/json")]
    public class BillToPayController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly ICreateFixedInvoiceHandler _createFixedInvoiceHandler;
        private readonly ISearchFixedInvoiceHandler _searchFixedInvoiceHandler;
        private readonly IEditBillToPayHandler _editBillToPayHandler;
        private readonly IPayBillToPayHandler _payBillToPayHandler;

        public BillToPayController(
            Serilog.ILogger logger,
            ICreateFixedInvoiceHandler createFixedInvoiceHandler,
            ISearchFixedInvoiceHandler searchFixedInvoiceHandler,
            IEditBillToPayHandler editBillToPayHandler,
            IPayBillToPayHandler payBillToPayHandler)
        {
            _logger = logger;
            _createFixedInvoiceHandler = createFixedInvoiceHandler;
            _searchFixedInvoiceHandler = searchFixedInvoiceHandler;
            _editBillToPayHandler = editBillToPayHandler;
            _payBillToPayHandler = payBillToPayHandler;
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit([FromBody] EditBillToPayInput input,
            CancellationToken cancellationToken)
        {
            _logger.Information($"[BillToPayController.Edit()] - Alteração de uma conta à pagar já cadastrada. Input: {JsonSerializeUtils.Serialize(input)}");

            var output = await _editBillToPayHandler.Handle(input, cancellationToken);

            return Ok(output);
        }

        /// <summary>
        /// Faz o processo de pagamento de uma conta à pagar.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPatch("pay")]
        public async Task<IActionResult> Pay([FromBody] PayBillToPayInput input,
            CancellationToken cancellationToken)
        {
            _logger.Information($"[BillToPayController.Pay()] - Efetuar pagamento. Input: {JsonSerializeUtils.Serialize(input)}");

            var output = await _payBillToPayHandler.Handle(input, cancellationToken);

            return Ok(output);
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateFixedInvoice([FromBody] CreateFixedInvoiceInput input,
            CancellationToken cancellationToken)
        {
            _logger.Information($"[BillToPayController.CreateFixedInvoice()] - Cadastro de uma nova conta/fatura fixa. Input: {JsonSerializeUtils.Serialize(input)}");

            var output = await _createFixedInvoiceHandler.Handle(input, cancellationToken);

            return Ok(output);
        }

        [HttpPost("register-basket")]
        public async Task<IActionResult> CreateBasketFixedInvoice([FromBody] IList<CreateFixedInvoiceInput> input,
            CancellationToken cancellationToken)
        {
            _logger.Information("[BillToPayController.CreateBasketFixedInvoice()] - Cadastro de uma nova conta/fatura fixa via basket. Input: {@Input}", JsonSerializeUtils.Serialize(input));

            List<CreateFixedInvoiceOutput> output = new();

            foreach (var fixedInvoice in input)
            {
                output.Add(await _createFixedInvoiceHandler.Handle(fixedInvoice, cancellationToken));
            }

            return Ok(output);
        }

        [HttpGet("register-search")]
        public async Task<IActionResult> GetFixedInvoice(CancellationToken cancellationToken)
        {
            _logger.Information($"[BillToPayController.GetFixedInvoice()] - Busca de conta/fatura fixa.");

            var output = await _searchFixedInvoiceHandler.Handle();

            return Ok(output);
        }
    }
}
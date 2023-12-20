using Application.Feature.BillToPay.UpdateBillToPay;
using Application.Feature.FixedInvoice.CreateFixedInvoice;
using Application.Feature.FixedInvoice.SearchFixedInvoice;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("v1/bill-to-pay")]
    [Produces("application/json")]
    public class BillToPayController : ControllerBase
    {
        private readonly ILogger<BillToPayController> _logger;
        private readonly ICreateFixedInvoiceHandler _createFixedInvoiceHandler;
        private readonly ISearchFixedInvoiceHandler _searchFixedInvoiceHandler;
        private readonly IUpdateBillToPayHandler _updateBillToPayHandler;

        public BillToPayController(
            ILogger<BillToPayController> logger,
            ICreateFixedInvoiceHandler createFixedInvoiceHandler,
            ISearchFixedInvoiceHandler searchFixedInvoiceHandler,
            IUpdateBillToPayHandler updateBillToPayHandler)
        {
            _logger = logger;
            _createFixedInvoiceHandler = createFixedInvoiceHandler;
            _searchFixedInvoiceHandler = searchFixedInvoiceHandler;
            _updateBillToPayHandler = updateBillToPayHandler;
        }

        [HttpPost("fixed-invoice")]
        public async Task<IActionResult> CreateFixedInvoice([FromBody] CreateFixedInvoiceInput input,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[BillToPayController.CreateFixedInvoice()] - Cadastro de uma nova conta/fatura fixa. Input: {JsonSerializer.Serialize(input)}");

            var output = await _createFixedInvoiceHandler.Handle(input, cancellationToken);

            return Ok(output);
        }

        [HttpPost("basket-fixed-invoice")]
        public async Task<IActionResult> CreateBasketFixedInvoice([FromBody] IList<CreateFixedInvoiceInput> input,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("[BillToPayController.CreateBasketFixedInvoice()] - Cadastro de uma nova conta/fatura fixa via basket. Input: {@Input}", JsonSerializer.Serialize(input));

            List<CreateFixedInvoiceOutput> output = new();

            foreach (var fixedInvoice in input)
            {
                output.Add(await _createFixedInvoiceHandler.Handle(fixedInvoice, cancellationToken));
            }

            return Ok(output);
        }

        [HttpGet("fixed-invoice")]
        public async Task<IActionResult> GetFixedInvoice(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[BillToPayController.GetFixedInvoice()] - Busca de conta/fatura fixa.");

            var output = await _searchFixedInvoiceHandler.Handle();

            return Ok(output);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBillToPay([FromBody] UpdateBillToPayInput input,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[BillToPayController.CreateFixedInvoice()] - Cadastro de uma nova conta/fatura fixa. Input: {JsonSerializer.Serialize(input)}");

            var validator = UpdateBillToPayValidator

            var output = await _updateBillToPayHandler.Handle(input, cancellationToken);

            return Ok(output);
        }
    }
}
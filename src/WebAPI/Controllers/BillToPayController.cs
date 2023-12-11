using Application.Feature.CreateFixedInvoice;
using Application.Feature.SearchFixedInvoice;
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

        public BillToPayController(
            ILogger<BillToPayController> logger,
            ICreateFixedInvoiceHandler createFixedInvoiceHandler,
            ISearchFixedInvoiceHandler searchFixedInvoiceHandler)
        {
            _logger = logger;
            _createFixedInvoiceHandler = createFixedInvoiceHandler;
            _searchFixedInvoiceHandler = searchFixedInvoiceHandler;
        }

        [HttpPost("fixed-invoice")]
        public async Task<IActionResult> CreateFixedInvoice([FromBody] CreateFixedInvoiceInput input,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[WalletToPayController.CreateFixedInvoice()] - Cadastro de uma nova conta/fatura fixa. Input: {JsonSerializer.Serialize(input)}");

            var output = await _createFixedInvoiceHandler.Handle(input, cancellationToken);

            return Ok(output);
        }

        [HttpGet("fixed-invoice")]
        public async Task<IActionResult> GetFixedInvoice(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[WalletToPayController.GetFixedInvoice()] - Busca de conta/fatura fixa.");

            var output = await _searchFixedInvoiceHandler.Handle();

            return Ok(output);
        }
    }
}
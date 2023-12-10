using Application.Feature.CreateFixedInvoice;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("v1/wallet-to-pay")]
    [Produces("application/json")]
    public class WalletToPayController : ControllerBase
    {
        private readonly ILogger<WalletToPayController> _logger;
        private readonly ICreateFixedInvoiceHandler _createFixedInvoiceHandler;

        public WalletToPayController(
            ILogger<WalletToPayController> logger,
            ICreateFixedInvoiceHandler createFixedInvoiceHandler)
        {
            _logger = logger;
            _createFixedInvoiceHandler = createFixedInvoiceHandler;
        }

        [HttpPost("fixed-invoice")]
        public async Task<IActionResult> CreateFixedInvoice([FromBody] CreateFixedInvoiceInput input,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[WalletToPayController.CreateFixedInvoice()] - Cadastro de uma nova conta/fatura fixa. Input: {JsonSerializer.Serialize(input)}");

            var output = await _createFixedInvoiceHandler.Handler(input, cancellationToken);

            return Ok(output);
        }
    }
}
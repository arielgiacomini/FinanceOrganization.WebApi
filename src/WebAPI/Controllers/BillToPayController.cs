using Application.Feature;
using Application.Feature.BillToPay.CreateBillToPay;
using Application.Feature.BillToPay.DeleteBillToPay;
using Application.Feature.BillToPay.EditBillToPay;
using Application.Feature.BillToPay.PayBillToPay;
using Application.Feature.BillToPay.SearchBillToPay;
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
        private readonly ICreateBillToPayHandler _createFixedInvoiceHandler;
        private readonly ISearchFixedInvoiceHandler _searchFixedInvoiceHandler;
        private readonly IEditBillToPayHandler _editBillToPayHandler;
        private readonly IPayBillToPayHandler _payBillToPayHandler;
        private readonly ISearchBillToPayHandler _searchBillToPayHandler;
        private readonly IDeleteBillToPayHandler _deleteBillToPayHandler;

        public BillToPayController(
            Serilog.ILogger logger,
            ICreateBillToPayHandler createFixedInvoiceHandler,
            ISearchFixedInvoiceHandler searchFixedInvoiceHandler,
            IEditBillToPayHandler editBillToPayHandler,
            IPayBillToPayHandler payBillToPayHandler,
            ISearchBillToPayHandler searchBillToPayHandler,
            IDeleteBillToPayHandler deleteBillToPayHandler)
        {
            _logger = logger;
            _createFixedInvoiceHandler = createFixedInvoiceHandler;
            _searchFixedInvoiceHandler = searchFixedInvoiceHandler;
            _editBillToPayHandler = editBillToPayHandler;
            _payBillToPayHandler = payBillToPayHandler;
            _searchBillToPayHandler = searchBillToPayHandler;
            _deleteBillToPayHandler = deleteBillToPayHandler;
        }

        /// <summary>
        /// Registrar uma conta a pagar
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> CreateFixedInvoice([FromBody] CreateBillToPayInput input,
            CancellationToken cancellationToken)
        {
            _logger.Information($"[BillToPayController.CreateFixedInvoice()] - Cadastro de uma nova conta/fatura fixa. Input: {JsonSerializeUtils.Serialize(input)}");

            var output = await _createFixedInvoiceHandler.Handle(input, cancellationToken);

            return Ok(output);
        }

        /// <summary>
        /// Registrar uma lista de contas a pagar
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("register-basket")]
        public async Task<IActionResult> CreateBasketFixedInvoice([FromBody] IList<CreateBillToPayInput> input,
            CancellationToken cancellationToken)
        {
            _logger.Information("[BillToPayController.CreateBasketFixedInvoice()] - Cadastro de uma nova conta/fatura fixa via basket. Input: {@Input}", JsonSerializeUtils.Serialize(input));

            List<CreateBillToPayOutput> outputList = new();
            CreateBillToPayOutput output = new();

            foreach (var fixedInvoice in input)
            {
                outputList.Add(await _createFixedInvoiceHandler.Handle(fixedInvoice, cancellationToken));
            }

            output.Output = OutputBaseDetails.Success("A lista de contas a pagar para cadastro teve exito.", outputList, outputList.Count);

            return Ok(output);
        }

        /// <summary>
        /// Busca do registro de uma conta a pagar
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("search-register")]
        public async Task<IActionResult> GetFixedInvoice(CancellationToken cancellationToken)
        {
            _logger.Information($"[BillToPayController.GetFixedInvoice()] - Busca de conta/fatura fixa.");

            var output = await _searchFixedInvoiceHandler.Handle();

            return Ok(output);
        }

        /// <summary>
        /// Edita uma conta a pagar
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("edit")]
        public async Task<IActionResult> EditBillToPay([FromBody] EditBillToPayInput input,
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

        /// <summary>
        /// Busca de contas a pagar
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("search")]
        public async Task<IActionResult> GetBillToPay([FromBody] SearchBillToPayInput input,
            CancellationToken cancellationToken)
        {
            _logger.Information($"[BillToPayController.GetBillToPay()] - Buscas de contas a pagar");

            var output = await _searchBillToPayHandler.Handle(input, cancellationToken);

            return Ok(output);
        }

        /// <summary>
        /// Busca de contas a pagar
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteBillToPay([FromBody] DeleteBillToPayInput input,
            CancellationToken cancellationToken)
        {
            _logger.Information($"[BillToPayController.DeleteBillToPay()] - Efetuar Delete");

            var output = await _deleteBillToPayHandler.Handle(input, cancellationToken);

            return Ok(output);
        }
    }
}
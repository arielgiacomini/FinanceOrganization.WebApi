using Application.Feature;
using Application.Feature.BillToPay.DeleteBillToPay;
using Application.Feature.BillToPay.EditBillToPay;
using Application.Feature.BillToPay.PayBillToPay;
using Application.Feature.BillToPay.SearchBillToPay;
using Application.Feature.BillToPay.SearchMonthlyAverageAnalysis;
using Application.Feature.BillToPayRegistration.CreateBillToPayRegistration;
using Application.Feature.BillToPayRegistration.CreateCreditCardNFCMobileBillToPayRegistration;
using Application.Feature.BillToPayRegistration.RecordsAwaitingCompleteRegistration;
using Application.Feature.BillToPayRegistration.SearchBillToPayRegistration;
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
        private readonly ICreateBillToPayRegistrationHandler _createBillToPayRegistrationHandler;
        private readonly ISearchBillToPayRegistrationHandler _searchBillToPayRegistrationHandler;
        private readonly IEditBillToPayHandler _editBillToPayHandler;
        private readonly IPayBillToPayHandler _payBillToPayHandler;
        private readonly ISearchBillToPayHandler _searchBillToPayHandler;
        private readonly IDeleteBillToPayHandler _deleteBillToPayHandler;
        private readonly ISearchMonthlyAverageAnalysisHandler _searchMonthlyAverageAnalysisHandler;
        private readonly ICreateNFCMobileBillToPayRegistrationHandler _createCreditCardNFCMobileBillToPayHandler;
        private readonly IRecordsAwaitingCompleteRegistrationHandler _recordsAwaitingCompleteRegistrationHandler;

        public BillToPayController(
            Serilog.ILogger logger,
            ICreateBillToPayRegistrationHandler createBillToPayRegistrationHandler,
            ISearchBillToPayRegistrationHandler searchBillToPayRegistrationHandler,
            IEditBillToPayHandler editBillToPayHandler,
            IPayBillToPayHandler payBillToPayHandler,
            ISearchBillToPayHandler searchBillToPayHandler,
            IDeleteBillToPayHandler deleteBillToPayHandler,
            ISearchMonthlyAverageAnalysisHandler searchMonthlyAverageAnalysisHandler,
            ICreateNFCMobileBillToPayRegistrationHandler createCreditCardNFCMobileBillToPayHandler,
            IRecordsAwaitingCompleteRegistrationHandler recordsAwaitingCompleteRegistrationHandler)
        {
            _logger = logger;
            _createBillToPayRegistrationHandler = createBillToPayRegistrationHandler;
            _searchBillToPayRegistrationHandler = searchBillToPayRegistrationHandler;
            _editBillToPayHandler = editBillToPayHandler;
            _payBillToPayHandler = payBillToPayHandler;
            _searchBillToPayHandler = searchBillToPayHandler;
            _deleteBillToPayHandler = deleteBillToPayHandler;
            _searchMonthlyAverageAnalysisHandler = searchMonthlyAverageAnalysisHandler;
            _createCreditCardNFCMobileBillToPayHandler = createCreditCardNFCMobileBillToPayHandler;
            _recordsAwaitingCompleteRegistrationHandler = recordsAwaitingCompleteRegistrationHandler;
        }

        /// <summary>
        /// Registrar uma conta a pagar
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> CreateBillToPayRegistration([FromBody] CreateBillToPayRegistrationInput input,
            CancellationToken cancellationToken)
        {
            _logger.Information($"[BillToPayController.CreateBillToPayRegistration()] - Cadastro de uma nova conta/fatura fixa. Input: {JsonSerializeUtils.Serialize(input)}");

            var output = await _createBillToPayRegistrationHandler.Handle(input, cancellationToken);

            return Ok(output);
        }

        /// <summary>
        /// Registrar uma lista de contas a pagar
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("register-basket")]
        public async Task<IActionResult> CreateBasketBillToPayRegistration([FromBody] IList<CreateBillToPayRegistrationInput> input,
            CancellationToken cancellationToken)
        {
            _logger.Information("[BillToPayController.CreateBasketBillToPayRegistration()] - Cadastro de uma nova conta/fatura fixa via basket. Input: {@Input}", JsonSerializeUtils.Serialize(input));

            List<CreateBillToPayRegistrationOutput> outputList = new();
            CreateBillToPayRegistrationOutput output = new();

            foreach (var billToPayRegistration in input)
            {
                outputList.Add(await _createBillToPayRegistrationHandler.Handle(billToPayRegistration, cancellationToken));
            }

            output.Output = OutputBaseDetails.Success("A lista de contas a pagar para cadastro teve exito.", outputList, outputList.Count);

            return Ok(output);
        }

        /// <summary>
        /// Registrar uma conta a pagar
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("register-creditcard-nfc-mobile")]
        public async Task<IActionResult> CreateBillToPayRegistrationCreditCardNFCMobile([FromBody] CreateNFCMobileBillToPayRegistrationInput input,
            CancellationToken cancellationToken)
        {
            _logger.Information($"[BillToPayController.CreateBillToPayRegistrationCreditCardNFCMobile()] - Cadastro de uma nova conta especifica quando ocorre a compra via Apple Pay Input: {JsonSerializeUtils.Serialize(input)}");

            var output = await _createCreditCardNFCMobileBillToPayHandler.Handle(input, cancellationToken);

            return Ok(output);
        }

        /// <summary>
        /// Busca do registro de uma conta a pagar
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("search-register")]
        public async Task<IActionResult> GetBillToPayRegistration(CancellationToken cancellationToken)
        {
            _logger.Information($"[BillToPayController.GetBillToPayRegistration()] - Busca de conta/fatura fixa.");

            var output = await _searchBillToPayRegistrationHandler.Handle();

            return Ok(output);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("records-awaiting-complete-registration")]
        public async Task<IActionResult> GetBillToPayRecordsAwaitingCompleteRegistration(CancellationToken cancellationToken)
        {
            var output = await _recordsAwaitingCompleteRegistrationHandler
                .Handle(new RecordsAwaitingCompleteRegistrationInput(), cancellationToken);

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
        /// Edita um lote de contas a pagar
        /// </summary>
        /// <param name="edits"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("edit-basket")]
        public async Task<IActionResult> EditBasketBillToPay([FromBody] IList<EditBillToPayInput> edits,
            CancellationToken cancellationToken)
        {
            _logger.Information("Alteração de Contas a Pagar via Lote. Input: {@Inputs}", edits);

            List<EditBillToPayOutput> outputList = new();
            EditBillToPayOutput output = new();

            foreach (var input in edits)
            {
                outputList.Add(await _editBillToPayHandler.Handle(input, cancellationToken));
            }

            output.Output = OutputBaseDetails.Success("A edição em lote de Contas a Pagar foi realizadas com sucesso.", outputList, outputList.Count);

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

        /// <summary>
        /// Análise média mensal
        /// </summary>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("SearchMonthlyAverageAnalysis")]
        public async Task<IActionResult> PostSearchMonthlyAverageAnalysis([FromBody] SearchMonthlyAverageAnalysisInput input,
            CancellationToken cancellationToken)
        {
            _logger.Information($"[BillToPayController.PostSearchMonthlyAverageAnalysis()] - Buscas de analises para tomadas de decisões");

            var output = await _searchMonthlyAverageAnalysisHandler.Handle(input, cancellationToken);

            return Ok(output);
        }
    }
}
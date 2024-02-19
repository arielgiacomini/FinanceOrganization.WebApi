using Domain.Interfaces;
using Serilog;
using System.Text.Json;

namespace Application.Feature.BillToPay.DeleteBillToPay
{
    public class DeleteBillToPayHandler : IDeleteBillToPayHandler
    {
        private readonly ILogger _logger;
        private readonly IBillToPayRepository _billToPayRepository;
        private readonly IFixedInvoiceRepository _fixedInvoiceRepository;

        public DeleteBillToPayHandler(ILogger logger, IBillToPayRepository billToPayRepository, IFixedInvoiceRepository fixedInvoiceRepository)
        {
            _logger = logger;
            _billToPayRepository = billToPayRepository;
            _fixedInvoiceRepository = fixedInvoiceRepository;
        }

        public async Task<DeleteBillToPayOutput> Handle(DeleteBillToPayInput input, CancellationToken cancellationToken)
        {
            _logger.Information("Os registros serão deletados a partir do objeto: {@input}", input);

            var validate = await DeleteBillToPayValidator.ValidateInput(input, _fixedInvoiceRepository, _billToPayRepository);

            if (validate.Any())
            {
                _logger.Warning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new DeleteBillToPayOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            int total = 0;
            Dictionary<int, Domain.Entities.FixedInvoice?> dicFixedInvoice = new();
            Dictionary<Guid, Domain.Entities.BillToPay?> dicBillToPay = new();

            if (input.Id != null)
            {
                foreach (var idBillToPay in input.Id)
                {
                    var billToPayReadyRemove = await _billToPayRepository.GetBillToPayById(idBillToPay);
                    dicBillToPay.Add(idBillToPay, billToPayReadyRemove);

                    var fixedInvoiceReadyRemove = await _fixedInvoiceRepository.GetById(billToPayReadyRemove!.IdFixedInvoice);
                    dicFixedInvoice.Add(billToPayReadyRemove!.IdFixedInvoice, fixedInvoiceReadyRemove);

                    var resultOne = await _billToPayRepository.Delete(billToPayReadyRemove);
                    total += resultOne;

                    var resultTwo = await _fixedInvoiceRepository.Delete(fixedInvoiceReadyRemove!);
                    total += resultTwo;
                }
            }

            if (input.IdFixedInvoices != null)
            {
                foreach (var idFixedInvoice in input.IdFixedInvoices)
                {
                    var fixedInvoicesReadyRemove = await _fixedInvoiceRepository.GetById(idFixedInvoice);
                    dicFixedInvoice.Add(idFixedInvoice, fixedInvoicesReadyRemove);

                    var billToPayReadyRemove = await _billToPayRepository.GetBillToPayByFixedInvoiceId(idFixedInvoice);

                    foreach (var remove in billToPayReadyRemove)
                    {
                        dicBillToPay.Add(remove.Id, remove);
                    }

                    var resultThree = await _billToPayRepository.DeleteRange(billToPayReadyRemove);
                    total += resultThree;

                    var resultFour = await _fixedInvoiceRepository.Delete(fixedInvoicesReadyRemove!);
                    total += resultFour;
                }
            }

            var output = new DeleteBillToPayOutput
            {
                Output = OutputBaseDetails
                .Success($"[{true}] - Delete realizado com sucesso.",
                 SetOutputData(dicFixedInvoice, dicBillToPay), total)
            };

            return await Task.FromResult(output);
        }

        private static string SetOutputData(Dictionary<int, Domain.Entities.FixedInvoice?> dicFixedInvoice, Dictionary<Guid, Domain.Entities.BillToPay?> dicBillToPay)
        {
            return string
                .Concat(
                JsonSerializer
                .Serialize(dicBillToPay.Values),
                JsonSerializer
                .Serialize(dicFixedInvoice.Values));
        }
    }
}
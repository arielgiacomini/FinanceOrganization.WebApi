using Domain.Interfaces;
using Serilog;
using System.Text.Json;

namespace Application.Feature.BillToPay.DeleteBillToPay
{
    public class DeleteBillToPayHandler : IDeleteBillToPayHandler
    {
        private readonly ILogger _logger;
        private readonly IBillToPayRepository _billToPayRepository;
        private readonly IBillToPayRegistrationRepository _fixedInvoiceRepository;

        public DeleteBillToPayHandler(ILogger logger, IBillToPayRepository billToPayRepository, IBillToPayRegistrationRepository fixedInvoiceRepository)
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
            int contador = 0;
            Dictionary<string, Domain.Entities.BillToPayRegistration?> dicFixedInvoice = new();
            Dictionary<string, Domain.Entities.BillToPay?> billToPayRemoved = new();

            if (input.Id != null)
            {
                foreach (var idBillToPay in input.Id)
                {
                    var billToPayReadyRemove = await _billToPayRepository.GetBillToPayById(idBillToPay);

                    if (billToPayReadyRemove == null)
                    {
                        _logger.Error($"[DeleteBillToPayHandler.Input.Id] - Não encontrado o BillToPay pelo Id: {idBillToPay}");
                        continue;
                    }

                    contador++;
                    var key = string.Concat(billToPayReadyRemove!.IdFixedInvoice, "-", contador);

                    billToPayRemoved.Add(key, billToPayReadyRemove);

                    var resultOne = await _billToPayRepository.Delete(billToPayReadyRemove);
                    total += resultOne;
                }

                foreach (var item in billToPayRemoved)
                {
                    var keyParsed = int.TryParse(item.Key.Split("-")[0], out int key);

                    var allBillToPay = await _billToPayRepository.GetBillToPayByFixedInvoiceId(key);

                    var existsBillToPayOpenAfterRemoved = allBillToPay
                        .Where(x => !x.HasPay)
                        .Any();

                    if (!existsBillToPayOpenAfterRemoved || input.DisableFixedInvoice)
                    {
                        var fixedInvoicesReadyToDisabled = await _fixedInvoiceRepository.GetById(key);

                        if (fixedInvoicesReadyToDisabled == null)
                        {
                            _logger.Error($"[DeleteBillToPayHandler.Input.Id] - Não encontrado o FixedInvoice pelo Id: {item.Key}");
                            continue;
                        }

                        dicFixedInvoice.Add(item.Key, fixedInvoicesReadyToDisabled);

                        fixedInvoicesReadyToDisabled.Enabled = false;

                        var resultTwo = await _fixedInvoiceRepository.Edit(fixedInvoicesReadyToDisabled!);
                        total += resultTwo;
                    }
                }
            }

            if (input.IdFixedInvoices != null)
            {
                foreach (var idFixedInvoice in input.IdFixedInvoices)
                {
                    var fixedInvoicesReadyDisabled = await _fixedInvoiceRepository.GetById(idFixedInvoice);

                    if (fixedInvoicesReadyDisabled == null)
                    {
                        _logger.Error($"[DeleteBillToPayHandler.Input.IdFixedInvoices] - Não encontrado o FixedInvoice pelo Id: {idFixedInvoice}");
                        continue;
                    }

                    contador++;
                    var key = string.Concat(fixedInvoicesReadyDisabled!.Id, "-", contador);

                    dicFixedInvoice.Add(key, fixedInvoicesReadyDisabled);

                    var billToPayReadyRemove = await _billToPayRepository.GetBillToPayByFixedInvoiceId(idFixedInvoice);

                    foreach (var remove in billToPayReadyRemove)
                    {
                        contador++;
                        var KeyInter = string.Concat(remove!.IdFixedInvoice, "-", contador);

                        billToPayRemoved.Add(KeyInter, remove);
                    }

                    var resultThree = await _billToPayRepository.DeleteRange(billToPayReadyRemove);
                    total += resultThree;

                    fixedInvoicesReadyDisabled!.Enabled = false;
                    var resultFour = await _fixedInvoiceRepository.Edit(fixedInvoicesReadyDisabled!);
                    total += resultFour;
                }
            }

            var output = new DeleteBillToPayOutput
            {
                Output = OutputBaseDetails
                .Success($"[{true}] - Delete realizado com sucesso.",
                 SetOutputData(dicFixedInvoice, billToPayRemoved), total)
            };

            return await Task.FromResult(output);
        }

        private static string SetOutputData(Dictionary<string, Domain.Entities.BillToPayRegistration?> dicFixedInvoice, Dictionary<string, Domain.Entities.BillToPay?> dicBillToPay)
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
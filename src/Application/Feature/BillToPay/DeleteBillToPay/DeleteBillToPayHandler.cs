using Domain.Interfaces;
using Serilog;
using System.Text.Json;

namespace Application.Feature.BillToPay.DeleteBillToPay
{
    public class DeleteBillToPayHandler : IDeleteBillToPayHandler
    {
        private readonly ILogger _logger;
        private readonly IBillToPayRepository _billToPayRepository;
        private readonly IBillToPayRegistrationRepository _billToPayRegistrationRepository;

        public DeleteBillToPayHandler(ILogger logger, IBillToPayRepository billToPayRepository, IBillToPayRegistrationRepository billToPayRegistrationRepository)
        {
            _logger = logger;
            _billToPayRepository = billToPayRepository;
            _billToPayRegistrationRepository = billToPayRegistrationRepository;
        }

        public async Task<DeleteBillToPayOutput> Handle(DeleteBillToPayInput input, CancellationToken cancellationToken)
        {
            _logger.Information("Os registros serão deletados a partir do objeto: {@input}", input);

            var validate = await DeleteBillToPayValidator.ValidateInput(input, _billToPayRegistrationRepository, _billToPayRepository);

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
            Dictionary<string, Domain.Entities.BillToPayRegistration?> dicBillToPayRegistration = new();
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
                    var key = string.Concat(billToPayReadyRemove!.IdBillToPayRegistration, "-", contador);

                    billToPayRemoved.Add(key, billToPayReadyRemove);

                    var resultOne = await _billToPayRepository.Delete(billToPayReadyRemove);
                    total += resultOne;
                }

                foreach (var item in billToPayRemoved)
                {
                    var keyParsed = int.TryParse(item.Key.Split("-")[0], out int key);

                    var allBillToPay = await _billToPayRepository.GetBillToPayByBillToPayRegistrationId(key);

                    var existsBillToPayOpenAfterRemoved = allBillToPay
                        .Where(x => !x.HasPay)
                        .Any();

                    if (!existsBillToPayOpenAfterRemoved || input.DisableBillToPayRegistration)
                    {
                        var billToPayRegistrationsReadyToDisabled = await _billToPayRegistrationRepository.GetById(key);

                        if (billToPayRegistrationsReadyToDisabled == null)
                        {
                            _logger.Error($"[DeleteBillToPayHandler.Input.Id] - Não encontrado o BillToPayRegistration pelo Id: {item.Key}");
                            continue;
                        }

                        dicBillToPayRegistration.Add(item.Key, billToPayRegistrationsReadyToDisabled);

                        billToPayRegistrationsReadyToDisabled.Enabled = false;

                        var resultTwo = await _billToPayRegistrationRepository.Edit(billToPayRegistrationsReadyToDisabled!);
                        total += resultTwo;
                    }
                    else if (!existsBillToPayOpenAfterRemoved && !input.DisableBillToPayRegistration)
                    {
                        var billToPayRegistrationsReadyToExclude = await _billToPayRegistrationRepository.GetById(key);

                        if (billToPayRegistrationsReadyToExclude == null)
                        {
                            _logger.Error($"[DeleteBillToPayHandler.Input.Id] - Não encontrado o BillToPayRegistration pelo Id: {item.Key}");
                            continue;
                        }

                        var deleted = await _billToPayRegistrationRepository
                            .Delete(billToPayRegistrationsReadyToExclude);

                        total += deleted;
                    }
                }
            }

            if (input.IdBillToPayRegistrations != null)
            {
                foreach (var idBillToPayRegistration in input.IdBillToPayRegistrations)
                {
                    var billToPayRegistrationsReadyDisabled = await _billToPayRegistrationRepository.GetById(idBillToPayRegistration);

                    if (billToPayRegistrationsReadyDisabled == null)
                    {
                        _logger.Error($"[DeleteBillToPayHandler.Input.IdBillToPayRegistrations] - Não encontrado o BillToPayRegistration pelo Id: {idBillToPayRegistration}");
                        continue;
                    }

                    contador++;
                    var key = string.Concat(billToPayRegistrationsReadyDisabled!.Id, "-", contador);

                    dicBillToPayRegistration.Add(key, billToPayRegistrationsReadyDisabled);

                    var billToPayReadyRemove = await _billToPayRepository.GetBillToPayByBillToPayRegistrationId(idBillToPayRegistration);

                    foreach (var remove in billToPayReadyRemove)
                    {
                        contador++;
                        var KeyInter = string.Concat(remove!.IdBillToPayRegistration, "-", contador);

                        billToPayRemoved.Add(KeyInter, remove);
                    }

                    var resultThree = await _billToPayRepository.DeleteRange(billToPayReadyRemove);
                    total += resultThree;

                    billToPayRegistrationsReadyDisabled!.Enabled = false;
                    var resultFour = await _billToPayRegistrationRepository.Edit(billToPayRegistrationsReadyDisabled!);
                    total += resultFour;
                }
            }

            var output = new DeleteBillToPayOutput
            {
                Output = OutputBaseDetails
                .Success($"[{true}] - Delete realizado com sucesso.",
                 SetOutputData(dicBillToPayRegistration, billToPayRemoved), total)
            };

            return await Task.FromResult(output);
        }

        private static string SetOutputData(Dictionary<string, Domain.Entities.BillToPayRegistration?> dicBillToPayRegistration, Dictionary<string, Domain.Entities.BillToPay?> dicBillToPay)
        {
            return string
                .Concat(
                JsonSerializer
                .Serialize(dicBillToPay.Values),
                JsonSerializer
                .Serialize(dicBillToPayRegistration.Values));
        }
    }
}
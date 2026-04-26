using Domain.Interfaces;
using Serilog;
using System.Text.Json;

namespace Application.Feature.CashReceivable.DeleteCashReceivable
{
    public class DeleteCashReceivableHandler : IDeleteCashReceivableHandler
    {
        private readonly ILogger _logger;
        private readonly ICashReceivableRepository _cashReceivableRepository;
        private readonly ICashReceivableRegistrationRepository _cashReceivableRegistrationRepository;

        public DeleteCashReceivableHandler(ILogger logger, ICashReceivableRepository cashReceivableRepository, ICashReceivableRegistrationRepository cashReceivableRegistrationRepository)
        {
            _logger = logger;
            _cashReceivableRepository = cashReceivableRepository;
            _cashReceivableRegistrationRepository = cashReceivableRegistrationRepository;
        }

        public async Task<DeleteCashReceivableOutput> Handle(DeleteCashReceivableInput input, CancellationToken cancellationToken)
        {
            _logger.Information("Os registros serão deletados a partir do objeto: {@input}", input);

            var validate = await DeleteCashReceivableValidator.ValidateInput(input, _cashReceivableRegistrationRepository, _cashReceivableRepository);

            if (validate.Any())
            {
                _logger.Warning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new DeleteCashReceivableOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            int total = 0;
            int contador = 0;
            Dictionary<string, Domain.Entities.CashReceivableRegistration?> dicCashReceivableRegistration = new();
            Dictionary<string, Domain.Entities.CashReceivable?> cashReceivableRemoved = new();

            if (input.Id != null)
            {
                foreach (var idBillToPay in input.Id)
                {
                    var cashReceivableReadyRemove = await _cashReceivableRepository.GetById(idBillToPay);

                    if (cashReceivableReadyRemove == null)
                    {
                        _logger.Error($"[DeleteCashReceivableHandler.Input.Id] - Não encontrado o CashReceivable pelo Id: {idBillToPay}");
                        continue;
                    }

                    contador++;
                    var key = string.Concat(cashReceivableReadyRemove!.IdCashReceivableRegistration, "-", contador);

                    cashReceivableRemoved.Add(key, cashReceivableReadyRemove);

                    var resultOne = await _cashReceivableRepository.Delete(cashReceivableReadyRemove);
                    total += resultOne;
                }

                foreach (var item in cashReceivableRemoved)
                {
                    var keyParsed = int.TryParse(item.Key.Split("-")[0], out int key);

                    var allBillToPay = await _cashReceivableRepository.GetCashReceivableRegistrationId(key);

                    var existsBillToPayOpenAfterRemoved = allBillToPay
                        .Where(x => !x.HasReceived)
                        .Any();

                    if (!existsBillToPayOpenAfterRemoved || input.DisableCashReceivableRegistration)
                    {
                        var billToPayRegistrationsReadyToDisabled = await _cashReceivableRegistrationRepository.GetById(key);

                        if (billToPayRegistrationsReadyToDisabled == null)
                        {
                            _logger.Error($"[DeleteBillToPayHandler.Input.Id] - Não encontrado o BillToPayRegistration pelo Id: {item.Key}");
                            continue;
                        }

                        dicCashReceivableRegistration.Add(item.Key, billToPayRegistrationsReadyToDisabled);

                        billToPayRegistrationsReadyToDisabled.Enabled = false;

                        var resultTwo = await _cashReceivableRegistrationRepository.Edit(billToPayRegistrationsReadyToDisabled!);
                        total += resultTwo;
                    }
                    else if (!existsBillToPayOpenAfterRemoved && !input.DisableCashReceivableRegistration)
                    {
                        var billToPayRegistrationsReadyToExclude = await _cashReceivableRegistrationRepository.GetById(key);

                        if (billToPayRegistrationsReadyToExclude == null)
                        {
                            _logger.Error($"[DeleteBillToPayHandler.Input.Id] - Não encontrado o BillToPayRegistration pelo Id: {item.Key}");
                            continue;
                        }

                        var deleted = await _cashReceivableRegistrationRepository
                            .Delete(billToPayRegistrationsReadyToExclude);

                        total += deleted;
                    }
                }
            }

            if (input.IdCashReceivableRegistrations != null)
            {
                foreach (var idCashReceivableRegistration in input.IdCashReceivableRegistrations)
                {
                    var cashReceivableRegistrationsReadyDisabled = await _cashReceivableRegistrationRepository.GetById(idCashReceivableRegistration);
                    if (cashReceivableRegistrationsReadyDisabled == null)
                    {
                        _logger.Error($"[DeleteBillToPayHandler.Input.IdBillToPayRegistrations] - Não encontrado o BillToPayRegistration pelo Id: {idCashReceivableRegistration}");
                        continue;
                    }

                    contador++;
                    var key = string.Concat(cashReceivableRegistrationsReadyDisabled!.Id, "-", contador);

                    dicCashReceivableRegistration.Add(key, cashReceivableRegistrationsReadyDisabled);

                    var cashReceivableReadyRemove = await _cashReceivableRepository.GetCashReceivableRegistrationId(idCashReceivableRegistration);

                    foreach (var remove in cashReceivableReadyRemove)
                    {
                        contador++;
                        var KeyInter = string.Concat(remove!.IdCashReceivableRegistration, "-", contador);

                        cashReceivableRemoved.Add(KeyInter, remove);
                    }

                    var resultThree = await _cashReceivableRepository.DeleteRange(cashReceivableReadyRemove);
                    total += resultThree;

                    cashReceivableRegistrationsReadyDisabled!.Enabled = false;
                    var resultFour = await _cashReceivableRegistrationRepository.Edit(cashReceivableRegistrationsReadyDisabled!);
                    total += resultFour;
                }
            }

            var output = new DeleteCashReceivableOutput
            {
                Output = OutputBaseDetails
                .Success($"[{true}] - Delete realizado com sucesso.",
                 SetOutputData(dicCashReceivableRegistration, cashReceivableRemoved), total)
            };

            return await Task.FromResult(output);
        }

        private static string SetOutputData(Dictionary<string, Domain.Entities.CashReceivableRegistration?> dicCashReceivableRegistration, Dictionary<string, Domain.Entities.CashReceivable?> dicCashReceivable)
        {
            return string
                .Concat(
                JsonSerializer
                .Serialize(dicCashReceivable.Values),
                JsonSerializer
                .Serialize(dicCashReceivableRegistration.Values));
        }
    }
}
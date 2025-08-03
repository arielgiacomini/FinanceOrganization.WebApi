using Application.Feature.BillToPay.SearchBillToPay;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Feature.CashReceivable.SearchCashReceivable
{
    public class SearchCashReceivableHandler : ISearchCashReceivableHandler
    {
        private readonly ILogger<SearchCashReceivableHandler> _logger;
        private readonly ICashReceivableRepository _cashReceivableRepository;

        public SearchCashReceivableHandler(
            ILogger<SearchCashReceivableHandler> logger,
            ICashReceivableRepository cashReceivableRepository)
        {
            _logger = logger;
            _cashReceivableRepository = cashReceivableRepository;
        }

        public async Task<SearchCashReceivableOutput> Handle(SearchCashReceivableInput input, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Está sendo criado a conta a pagar de nome: {YearMonth}", input.YearMonth);

            var validate = await SearchCashReceivableValidator.ValidateInput(input);

            if (validate.Any())
            {
                _logger.LogWarning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new SearchBillToPayOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

            }

            var output = new SearchCashReceivableOutput()
            {
                Output = new OutputBaseDetails()
            };

            var searchCashReceivableDataOutput = new List<SearchCashReceivableData>();

            if (input.YearMonth != null)
            {
                var cashReceivableByYearMonth = await _cashReceivableRepository.GetByMonthYear(input.YearMonth);

                if (cashReceivableByYearMonth != null)
                {
                    if (ShowDetails(input))
                    {
                        foreach (var item in cashReceivableByYearMonth)
                        {
                            decimal sumValue = 0;
                            if (item != null)
                            {
                                if (item.RegistrationType == RegistrationType.CONTA_FATURA_FIXA)
                                {
                                    var billToPayFree = await _cashReceivableRepository
                                        .GetByYearMonthAndCategoryAndRegistrationType(item.YearMonth!, item.Category!, RegistrationType.COMPRA_LIVRE);

                                    if (billToPayFree != null)
                                    {
                                        var list = new List<Domain.Entities.CashReceivable>();
                                        foreach (var relation in billToPayFree)
                                        {
                                            sumValue += relation.Value;
                                            list.Add(relation);
                                        }

                                        searchCashReceivableDataOutput.Add(MapDomainToData(item, list, sumValue));
                                    }
                                }
                                else
                                {
                                    searchCashReceivableDataOutput.Add(MapDomainToData(item, null, sumValue));
                                }
                            }
                        }
                    }

                    if (ShowDetails(input) && searchCashReceivableDataOutput.Count > 0)
                    {
                        output.Output = OutputBaseDetails.Success("", searchCashReceivableDataOutput, searchCashReceivableDataOutput.Count);
                    }
                    else
                    {
                        output.Output = OutputBaseDetails.Success("", cashReceivableByYearMonth, cashReceivableByYearMonth.Count);
                    }

                    return output;
                }
            }
            else if (input.Id != null)
            {
                foreach (var iDCashReceivable in input.Id)
                {
                    IList<Domain.Entities.CashReceivable> billToPays = new List<Domain.Entities.CashReceivable>();
                    var billToPayById = await _cashReceivableRepository.GetById(iDCashReceivable);

                    if (billToPayById != null)
                    {
                        billToPays.Add(billToPayById);

                        output.Output = OutputBaseDetails.Success("", billToPays, 1);

                        return output;
                    }
                }
            }
            else if (input.IdCashReceivableRegistrations != null)
            {
                foreach (var idBillToPayRegistration in input.IdCashReceivableRegistrations)
                {
                    var billToPayByIdBillToPayRegistration = await _cashReceivableRepository.GetCashReceivableRegistrationId(idBillToPayRegistration);

                    if (billToPayByIdBillToPayRegistration != null)
                    {
                        if (ShowDetails(input))
                        {
                            foreach (var item in billToPayByIdBillToPayRegistration)
                            {
                                decimal sumValue = 0;
                                if (item != null)
                                {
                                    if (item.RegistrationType == RegistrationType.CONTA_FATURA_FIXA)
                                    {
                                        var billToPayFree = await _cashReceivableRepository
                                            .GetByYearMonthAndCategoryAndRegistrationType(item.YearMonth!, item.Category!, RegistrationType.COMPRA_LIVRE);

                                        if (billToPayFree != null)
                                        {
                                            var list = new List<Domain.Entities.CashReceivable>();
                                            foreach (var relation in billToPayFree)
                                            {
                                                sumValue += relation.Value;
                                                list.Add(relation);
                                            }

                                            searchCashReceivableDataOutput.Add(MapDomainToData(item, list, sumValue));
                                        }
                                    }
                                    else
                                    {
                                        searchCashReceivableDataOutput.Add(MapDomainToData(item, null, sumValue));
                                    }
                                }
                            }
                        }

                        if (ShowDetails(input) && searchCashReceivableDataOutput.Count > 0)
                        {
                            output.Output = OutputBaseDetails.Success("", searchCashReceivableDataOutput, billToPayByIdBillToPayRegistration.Count);
                        }
                        else
                        {
                            output.Output = OutputBaseDetails.Success("", billToPayByIdBillToPayRegistration, billToPayByIdBillToPayRegistration.Count);
                        }
                    }

                    return output;
                }
            }
            else
            {
                var all = await _cashReceivableRepository.GetAll();

                foreach (var item in all)
                {
                    searchCashReceivableDataOutput.Add(MapDomainToData(item, null, 0));
                }

                output.Output = OutputBaseDetails.Success("", searchCashReceivableDataOutput, searchCashReceivableDataOutput.Count);
            }

            return output;
        }

        private static SearchCashReceivableData MapDomainToData(Domain.Entities.CashReceivable domain, List<Domain.Entities.CashReceivable>? details, decimal amount)
        {
            IList<SearchCashReceivableDataDetails> listDetails = new List<SearchCashReceivableDataDetails>();

            if (details is not null)
            {
                foreach (var itemDetail in details)
                {
                    listDetails.Add(new()
                    {
                        Id = itemDetail.Id,
                        IdCashReceivableRegistration = itemDetail.IdCashReceivableRegistration,
                        Name = itemDetail.Name,
                        Account = itemDetail.Account,
                        Frequence = itemDetail.Frequence,
                        RegistrationType = itemDetail.RegistrationType,
                        AgreementDate = itemDetail.AgreementDate,
                        DueDate = itemDetail.DueDate,
                        YearMonth = itemDetail.YearMonth,
                        Category = itemDetail.Category,
                        Value = itemDetail.Value,
                        ManipulatedValue = itemDetail.ManipulatedValue,
                        DateReceived = itemDetail.DateReceived,
                        HasReceived = itemDetail.HasReceived,
                        AdditionalMessage = itemDetail.AdditionalMessage,
                        Enabled = itemDetail.Enabled,
                        CreationDate = itemDetail.CreationDate,
                        LastChangeDate = itemDetail.LastChangeDate
                    });
                }
            }

            SearchCashReceivableData searchCashReceivable = new()
            {
                Id = domain.Id,
                IdCashReceivableRegistration = domain.IdCashReceivableRegistration,
                Name = domain.Name,
                Account = domain.Account,
                Frequence = domain.Frequence,
                RegistrationType = domain.RegistrationType,
                AgreementDate = domain.AgreementDate,
                DueDate = domain.DueDate,
                YearMonth = domain.YearMonth,
                Category = domain.Category,
                Value = domain.Value,
                ManipulatedValue = domain.ManipulatedValue,
                DateReceived = domain.DateReceived,
                HasReceived = domain.HasReceived,
                AdditionalMessage = domain.AdditionalMessage,
                Enabled = domain.Enabled,
                CreationDate = domain.CreationDate,
                LastChangeDate = domain.LastChangeDate,
                Details = listDetails
            };

            return searchCashReceivable;
        }

        private static bool ShowDetails(SearchCashReceivableInput input)
        {
            return input.ShowDetails.HasValue && input.ShowDetails.Value;
        }
    }
}
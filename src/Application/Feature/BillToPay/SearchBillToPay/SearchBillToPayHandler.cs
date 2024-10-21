using Domain.Entities;
using Domain.Interfaces;
using Serilog;

namespace Application.Feature.BillToPay.SearchBillToPay
{
    public class SearchBillToPayHandler : ISearchBillToPayHandler
    {
        private readonly ILogger _logger;
        private readonly IBillToPayRepository _billToPayRepository;

        public SearchBillToPayHandler(ILogger logger, IBillToPayRepository billToPayRepository)
        {
            _logger = logger;
            _billToPayRepository = billToPayRepository;
        }

        public async Task<SearchBillToPayOutput> Handle(SearchBillToPayInput input, CancellationToken cancellationToken = default)
        {
            _logger.Information("Está sendo criado a conta a pagar de nome: {YearMonth}", input.YearMonth);

            var validate = await SearchBillToPayValidator.ValidateInput(input);

            if (validate.Any())
            {
                _logger.Warning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new SearchBillToPayOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var output = new SearchBillToPayOutput()
            {
                Output = new OutputBaseDetails()
            };

            var searchBillToPayDataOutput = new List<SearchBillToPayData>();

            if (input.YearMonth != null)
            {
                var billToPayByYearMonth = await _billToPayRepository.GetBillToPayByYearMonth(input.YearMonth);

                if (billToPayByYearMonth != null)
                {
                    if (ShowDetails(input))
                    {
                        foreach (var item in billToPayByYearMonth)
                        {
                            decimal sumValue = 0;
                            if (item != null)
                            {
                                if (item.RegistrationType == RegistrationType.CONTA_FATURA_FIXA)
                                {
                                    var billToPayFree = await _billToPayRepository
                                        .GetBillToPayByYearMonthAndCategoryAndRegistrationType(item.YearMonth!, item.Category!, RegistrationType.COMPRA_LIVRE);

                                    if (billToPayFree != null)
                                    {
                                        var list = new List<Domain.Entities.BillToPay>();
                                        foreach (var relation in billToPayFree)
                                        {
                                            sumValue += relation.Value;
                                            list.Add(relation);
                                        }

                                        searchBillToPayDataOutput.Add(MapDomainToData(item, list, sumValue));
                                    }
                                }
                                else
                                {
                                    searchBillToPayDataOutput.Add(MapDomainToData(item, null, sumValue));
                                }
                            }
                        }
                    }

                    if (ShowDetails(input) && searchBillToPayDataOutput.Count > 0)
                    {
                        output.Output = OutputBaseDetails.Success("", searchBillToPayDataOutput, searchBillToPayDataOutput.Count);
                    }
                    else
                    {
                        output.Output = OutputBaseDetails.Success("", billToPayByYearMonth, billToPayByYearMonth.Count);
                    }

                    return output;
                }
            }

            if (input.Id != null)
            {
                foreach (var IdBillToPay in input.Id)
                {
                    IList<Domain.Entities.BillToPay> billToPays = new List<Domain.Entities.BillToPay>();
                    var billToPayById = await _billToPayRepository.GetBillToPayById(IdBillToPay);

                    if (billToPayById != null)
                    {
                        billToPays.Add(billToPayById);

                        output.Output = OutputBaseDetails.Success("", billToPays, 1);

                        return output;
                    }
                }
            }

            if (input.IdBillToPayRegistrations != null)
            {
                foreach (var idBillToPayRegistration in input.IdBillToPayRegistrations)
                {
                    var billToPayByIdBillToPayRegistration = await _billToPayRepository.GetBillToPayByBillToPayRegistrationId(idBillToPayRegistration);

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
                                        var billToPayFree = await _billToPayRepository
                                            .GetBillToPayByYearMonthAndCategoryAndRegistrationType(item.YearMonth!, item.Category!, RegistrationType.COMPRA_LIVRE);

                                        if (billToPayFree != null)
                                        {
                                            var list = new List<Domain.Entities.BillToPay>();
                                            foreach (var relation in billToPayFree)
                                            {
                                                sumValue += relation.Value;
                                                list.Add(relation);
                                            }

                                            searchBillToPayDataOutput.Add(MapDomainToData(item, list, sumValue));
                                        }
                                    }
                                    else
                                    {
                                        searchBillToPayDataOutput.Add(MapDomainToData(item, null, sumValue));
                                    }
                                }
                            }
                        }

                        if (ShowDetails(input) && searchBillToPayDataOutput.Count > 0)
                        {
                            output.Output = OutputBaseDetails.Success("", searchBillToPayDataOutput, billToPayByIdBillToPayRegistration.Count);
                        }
                        else
                        {
                            output.Output = OutputBaseDetails.Success("", billToPayByIdBillToPayRegistration, billToPayByIdBillToPayRegistration.Count);
                        }
                    }

                    return output;
                }
            }

            return output;
        }

        private static SearchBillToPayData MapDomainToData(Domain.Entities.BillToPay domain, List<Domain.Entities.BillToPay>? details, decimal amount)
        {
            IList<Details> listDetails = new List<Details>();

            if (details is not null)
            {
                foreach (var itemDetail in details)
                {
                    listDetails.Add(new()
                    {
                        Id = itemDetail.Id,
                        IdBillToPayRegistration = itemDetail.IdBillToPayRegistration,
                        Account = itemDetail.Account,
                        Name = itemDetail.Name,
                        Category = itemDetail.Category,
                        Value = itemDetail.Value,
                        PurchaseDate = itemDetail.PurchaseDate,
                        DueDate = itemDetail.DueDate,
                        YearMonth = itemDetail.YearMonth,
                        Frequence = itemDetail.Frequence,
                        RegistrationType = itemDetail.RegistrationType,
                        PayDay = itemDetail.PayDay,
                        HasPay = itemDetail.HasPay,
                        AdditionalMessage = itemDetail.AdditionalMessage,
                        CreationDate = itemDetail.CreationDate,
                        LastChangeDate = itemDetail.LastChangeDate
                    });
                };
            }

            SearchBillToPayData searchBillToPayData = new()
            {
                Id = domain.Id,
                IdBillToPayRegistration = domain.IdBillToPayRegistration,
                Account = domain.Account,
                Name = domain.Name,
                Category = domain.Category,
                Value = domain.Value,
                PurchaseDate = domain.PurchaseDate,
                DueDate = domain.DueDate,
                YearMonth = domain.YearMonth,
                Frequence = domain.Frequence,
                RegistrationType = domain.RegistrationType,
                PayDay = domain.PayDay,
                HasPay = domain.HasPay,
                AdditionalMessage = domain.AdditionalMessage,
                CreationDate = domain.CreationDate,
                LastChangeDate = domain.LastChangeDate,
                DetailsQuantity = listDetails.Count,
                DetailsAmount = amount,
                Details = listDetails
            };

            return searchBillToPayData;
        }

        private static bool ShowDetails(SearchBillToPayInput input)
        {
            return input.ShowDetails.HasValue && input.ShowDetails.Value;
        }
    }
}
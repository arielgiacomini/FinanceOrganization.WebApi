﻿using Domain.Interfaces;
using Serilog;
using D = Domain.Entities;

namespace Application.Feature.BillToPay.PayBillToPay
{
    public class PayBillToPayHandler : IPayBillToPayHandler
    {
        private readonly ILogger _logger;
        private readonly IBillToPayRepository _billToPayRepository;
        private const string EH_CARTAO_CREDITO_NAIRA = "Cartão de Crédito Nubank Naíra";

        public PayBillToPayHandler(ILogger logger, IBillToPayRepository billToPayRepository)
        {
            _logger = logger;
            _billToPayRepository = billToPayRepository;
        }

        public async Task<PayBillToPayOutput> Handle(PayBillToPayInput input, CancellationToken cancellationToken)
        {
            PayBillToPayOutput output = new();
            var validate = await PayBillToPayValidator.ValidateInput(input, _billToPayRepository);

            if (validate.Any())
            {
                _logger.Warning("Erro de validação. para os seguintes dados: {@input} e a validação foi: {@validate}", input, validate);

                var outputValidator = new PayBillToPayOutput
                {
                    Output = OutputBaseDetails.Validation("Houve erro de validação", validate)
                };

                return outputValidator;
            }

            var listToPay = await SearchToPay(input);

            if (listToPay == null || listToPay!.Count == 0)
            {
                output.Output = OutputBaseDetails.Error($"Não foram encontradas nenhuma conta a pagar do tipo conta [{input.Account}] no período [{input.YearMonth}] e/ou ID [{input.Id}] informado.", new Dictionary<string, string>());

                return output;
            }

            var billsToPay = MapInputToDomain(listToPay, input);

            var result = await _billToPayRepository.EditRange(billsToPay!);

            if (result <= 0)
            {
                output.Output = OutputBaseDetails.Error("Houve erro ao efetuar o pagamento.", new Dictionary<string, string>(), billsToPay.Count);

                return output;
            }

            output.Output = OutputBaseDetails.Success($"{result} - Pagamento realizado com sucesso.", new object(), billsToPay.Count);

            return output;
        }

        private static IList<D.BillToPay>? MapInputToDomain(IList<D.BillToPay>? billsToPay, PayBillToPayInput input)
        {
            List<D.BillToPay>? mapBillsToPay = new();

            if (billsToPay != null)
            {
                foreach (var bill in billsToPay)
                {
                    mapBillsToPay.Add(new D.BillToPay()
                    {
                        Id = bill.Id,
                        IdFixedInvoice = bill.IdFixedInvoice,
                        Account = bill.Account,
                        Name = bill.Name,
                        Category = bill.Category,
                        Value = bill.Value,
                        PurchaseDate = bill.PurchaseDate,
                        DueDate = bill.DueDate,
                        YearMonth = bill.YearMonth,
                        Frequence = bill.Frequence,
                        RegistrationType = bill.RegistrationType,
                        PayDay = input.PayDay,
                        HasPay = input.HasPay,
                        AdditionalMessage = bill.AdditionalMessage,
                        CreationDate = bill.CreationDate,
                        LastChangeDate = input.LastChangeDate
                    });
                }
            }

            return mapBillsToPay;
        }

        private async Task<IList<D.BillToPay>?> SearchToPay(PayBillToPayInput input)
        {
            List<D.BillToPay>? listPay = new();

            if (input.Id != null)
            {
                var bill = await _billToPayRepository.GetBillToPayById(input.Id.Value);

                if (bill != null)
                {
                    listPay.Add(bill);
                }
            }

            if (input.Account != null && input.YearMonth != null)
            {
                var listNotPaidYet = await _billToPayRepository.GetNotPaidYetByYearMonthAndAccount(input.YearMonth, input.Account);

                if (listNotPaidYet != null)
                {
                    if (input.ConsiderNairaCreditCard.HasValue && input.ConsiderNairaCreditCard.Value)
                    {
                        var creditCardNaira = listNotPaidYet
                            .Where(filterCreditCardNaira => filterCreditCardNaira.AdditionalMessage != null
                            && filterCreditCardNaira.AdditionalMessage.StartsWith(EH_CARTAO_CREDITO_NAIRA))
                            .ToList();

                        listPay.AddRange(creditCardNaira);
                    }
                    else
                    {
                        listPay.AddRange(listNotPaidYet);
                    }
                }
            }

            return listPay;
        }
    }
}
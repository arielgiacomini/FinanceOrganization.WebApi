﻿using Domain.Entities;
using Domain.Interfaces;
using Domain.Options;
using Domain.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Application.EventHandlers.CreateBillToPayEvent
{
    public class CreateBillToPayEventHandler : ICreateBillToPayEventHandler
    {
        private readonly ILogger<CreateBillToPayEventHandler> _logger;
        private readonly BillToPayOptions _billToPayOptions;
        private readonly IFixedInvoiceRepository _fixedInvoiceRepository;
        private readonly IWalletToPayRepository _walletToPayRepository;
        private const int QTD_MONTH_YEAR = 12;

        public CreateBillToPayEventHandler(
            ILogger<CreateBillToPayEventHandler> logger,
            IOptions<BillToPayOptions> options,
            IFixedInvoiceRepository fixedInvoiceRepository,
            IWalletToPayRepository walletToPayRepository)
        {
            _logger = logger;
            _fixedInvoiceRepository = fixedInvoiceRepository;
            _billToPayOptions = options.Value;
            _walletToPayRepository = walletToPayRepository;
        }

        public async Task Handle(CreateBillToPayEventInput input)
        {
            var fixedInvoices = await _fixedInvoiceRepository.GetByAll();

            foreach (var fixedInvoice in fixedInvoices)
            {
                var json = JsonSerializer.Serialize(fixedInvoice);

                _logger.LogInformation("Objeto FixedInvoice que será processado: {@json}", json);

                StartRegistration(fixedInvoice);
            }
        }

        /// <summary>
        /// Responsável de verificar se o registro no banco de dados está elegível para cadastro agora.
        /// </summary>
        /// <param name="fixedInvoice"></param>
        /// <returns></returns>
        private void StartRegistration(FixedInvoice fixedInvoice)
        {
            _ = LogicRegistration(fixedInvoice);
        }

        /// <summary>
        /// Contempla a lógica para cadastro de novas contas a pagar de acordo com o tempo que ela se encontra.
        /// </summary>
        /// <param name="fixedInvoice"></param>
        private async Task LogicRegistration(FixedInvoice fixedInvoice)
        {
            List<BillToPay> listBillToPay = new();

            var billsToPay = await _walletToPayRepository.GetBillToPayByFixedInvoiceId(fixedInvoice.Id);

            var lastBillToPay = GetLastRegistrationBillToPay(billsToPay);

            var currentYearMonth = DateServiceUtils.IsCurrentMonth(fixedInvoice.InitialMonthYear);

            var totalMonths = DateServiceUtils.GetMonthsByDateTime(lastBillToPay.DueDate);

            if (totalMonths > _billToPayOptions.HowManyMonthForward)
            {
                return;
            }

            var nextMonthYearToRegister = DateServiceUtils
                .GetNextYearMonthAndDateTime(
                lastBillToPay.DueDate, GetQtdMonthByConfig(), fixedInvoice.BestPayDay, currentYearMonth);

            if (nextMonthYearToRegister is null)
            {
                return;
            }

            foreach (var nextMonth in nextMonthYearToRegister!)
            {

                listBillToPay.Add(MapBillToPay(lastBillToPay, fixedInvoice, nextMonth.Value, nextMonth.Key));
            }

            await _walletToPayRepository.Save(listBillToPay);
        }

        private static BillToPay MapBillToPay(BillToPay? billToPay, FixedInvoice? fixedInvoice, DateTime dueDate, string yearMonth)
        {
            if (billToPay is not null)
            {
                return new BillToPay()
                {
                    Id = Guid.NewGuid(),
                    IdFixedInvoice = billToPay!.IdFixedInvoice,
                    Account = billToPay.Account,
                    Name = billToPay.Name,
                    Category = billToPay.Category,
                    Value = billToPay.Value,
                    DueDate = dueDate,
                    YearMonth = yearMonth,
                    Frequence = billToPay.Frequence,
                    PayDay = null,
                    HasPay = false
                };
            }
            else
            {
                return new BillToPay()
                {
                    Id = Guid.NewGuid(),
                    IdFixedInvoice = fixedInvoice!.Id,
                    Account = fixedInvoice.Account,
                    Name = fixedInvoice.Name,
                    Category = fixedInvoice.Category,
                    Value = fixedInvoice.Value,
                    DueDate = dueDate,
                    YearMonth = yearMonth,
                    Frequence = fixedInvoice.Frequence,
                    PayDay = null,
                    HasPay = false
                };
            }
        }

        private static BillToPay GetLastRegistrationBillToPay(IList<Domain.Entities.BillToPay> billsToPay)
        {
            var result = billsToPay
                .OrderByDescending(billToPay => billToPay.DueDate)
                .FirstOrDefault()!;

            return result;
        }

        private int GetQtdMonthByConfig()
        {
            var result = (_billToPayOptions.HowManyYearsForward * QTD_MONTH_YEAR);

            return result;
        }
    }
}
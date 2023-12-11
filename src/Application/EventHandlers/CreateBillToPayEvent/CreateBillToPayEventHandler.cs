using Domain.Entities;
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

        public async Task Handle(CreateBillToPayInput input)
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
        /// Contempla a lógica para cadastro de novas contas a pagar de acordo com o tempo que ela se encontra.
        /// </summary>
        /// <param name="fixedInvoice"></param>
        private async Task LogicRegistration(FixedInvoice fixedInvoice)
        {
            List<BillToPay> billsToPay = new();

            var listbillToPay = await _walletToPayRepository.GetBillToPayByFixedInvoiceId(fixedInvoice.Id);

            var lastRegistrationBillToPay = GetLastRegistrationBillToPay(listbillToPay);

            var nextMonthYearToRegister = DateServiceUtils
                .GetNextYearMonthAndDateTime(
                lastRegistrationBillToPay?.DueDate, GetQtdMonthByConfig(), fixedInvoice.BestPayDay);

            if (nextMonthYearToRegister is null)
            {
                return;
            }

            foreach (var nextMonth in nextMonthYearToRegister!)
            {
                if (lastRegistrationBillToPay != null)
                {
                    billsToPay.Add(MapBillToPayToBillToPay(lastRegistrationBillToPay, nextMonth.Value, nextMonth.Key));
                }
                else
                {
                    billsToPay.Add(MapFixedInvoiceToBillToPay(fixedInvoice, nextMonth.Value, nextMonth.Key));
                }
            }

            await _walletToPayRepository.Save(billsToPay);
        }

        private static BillToPay MapBillToPayToBillToPay(BillToPay billToPay, DateTime dueDate, string yearMonth)
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

        private static BillToPay MapFixedInvoiceToBillToPay(FixedInvoice fixedInvoice, DateTime dueDate, string yearMonth)
        {
            return new BillToPay()
            {
                Id = Guid.NewGuid(),
                IdFixedInvoice = fixedInvoice.Id,
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

        private static BillToPay GetLastRegistrationBillToPay(IList<Domain.Entities.BillToPay> billsToPay)
        {
            var result = billsToPay
                .OrderByDescending(billToPay => billToPay.DueDate)
                .FirstOrDefault()!;

            return result;
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

        private void StandyBy(FixedInvoice fixedInvoice)
        {
            if (!fixedInvoice.LastChangeDate.HasValue)
            {
                _ = LogicRegistration(fixedInvoice);
            }
            else
            {
                var lastChangeDate = fixedInvoice.LastChangeDate.Value;
                var nowDate = DateTime.Now.Date;
                var backDate = nowDate.AddYears(-_billToPayOptions.HowManyYearsForward);

                if (lastChangeDate.Date <= backDate)
                {
                    _ = LogicRegistration(fixedInvoice);
                }
            }
        }

        private int GetQtdMonthByConfig()
        {
            var result = (_billToPayOptions.HowManyYearsForward * QTD_MONTH_YEAR);

            return result;
        }
    }
}
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

                var billsToPay = await _walletToPayRepository.GetBillToPayByFixedInvoiceId(fixedInvoice.Id);

                var lastBillToPay = GetLastRegistrationBillToPay(billsToPay);

                StartRegistration(fixedInvoice, lastBillToPay);
            }
        }

        /// <summary>
        /// Responsável de verificar se o registro no banco de dados está elegível para cadastro agora.
        /// </summary>
        /// <param name="fixedInvoice"></param>
        /// <returns></returns>
        private void StartRegistration(FixedInvoice fixedInvoice, BillToPay? billToPay)
        {
            _ = LogicRegistration(fixedInvoice, billToPay);
        }

        /// <summary>
        /// Contempla a lógica para cadastro de novas contas a pagar de acordo com o tempo que ela se encontra.
        /// </summary>
        /// <param name="fixedInvoice"></param>
        private async Task LogicRegistration(FixedInvoice fixedInvoice, BillToPay? billToPay)
        {
            if (billToPay != null)
            {
                await LogicByBillToPay(billToPay);
            }
            else
            {
                await LogicByFixedInvoice(fixedInvoice);
            }
        }

        private async Task LogicByFixedInvoice(FixedInvoice fixedInvoice)
        {
            List<BillToPay> listBillToPay = new();

            var totalMonths = DateServiceUtils.GetMonthsByDateTime(
                DateServiceUtils.GetDateTimeByYearMonthBrazilian(fixedInvoice.InitialMonthYear));

            var qtdMonthAdd = GetMonthsAdd(totalMonths, _billToPayOptions.HowManyMonthForward);

            if (qtdMonthAdd <= 0 || totalMonths > _billToPayOptions.HowManyMonthForward)
            {
                return;
            }

            var nextMonthYearToRegister = DateServiceUtils
                .GetNextYearMonthAndDateTime(
                null, qtdMonthAdd, fixedInvoice.BestPayDay,
                DateServiceUtils.IsCurrentMonth(fixedInvoice.InitialMonthYear));

            foreach (var nextMonth in nextMonthYearToRegister!)
            {
                listBillToPay.Add(MapBillToPay(null, fixedInvoice, nextMonth.Value, nextMonth.Key));
            }

            await _walletToPayRepository.Save(listBillToPay);
        }

        private async Task LogicByBillToPay(BillToPay billToPay)
        {
            List<BillToPay> listBillToPay = new();

            var totalMonths = DateServiceUtils.GetMonthsByDateTime(billToPay.DueDate);

            var qtdMonthAdd = GetMonthsAdd(totalMonths, _billToPayOptions.HowManyMonthForward);

            if (qtdMonthAdd <= 0 || totalMonths > _billToPayOptions.HowManyMonthForward)
            {
                return;
            }

            var nextMonthYearToRegister = DateServiceUtils
                .GetNextYearMonthAndDateTime(
                billToPay.DueDate, qtdMonthAdd, null, false);

            foreach (var nextMonth in nextMonthYearToRegister!)
            {
                listBillToPay.Add(MapBillToPay(billToPay, null, nextMonth.Value, nextMonth.Key));
            }

            await _walletToPayRepository.Save(listBillToPay);
        }

        public static int GetMonthsAdd(int totalMonths, int howManyMonthForward)
        {
            if (totalMonths > howManyMonthForward)
            {
                return 0;
            }

            var result = Math.Abs(totalMonths - howManyMonthForward);

            return result;
        }

        public BillToPay MapBillToPay(
            BillToPay? billToPay, FixedInvoice? fixedInvoice, DateTime dueDate, string yearMonth)
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

        public BillToPay GetLastRegistrationBillToPay(IList<BillToPay> billsToPay)
        {
            var result = billsToPay
                .OrderByDescending(billToPay => billToPay.DueDate)
                .FirstOrDefault()!;

            return result;
        }
    }
}
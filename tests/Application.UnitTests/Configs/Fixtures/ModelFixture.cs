using Application.Feature.CreateFixedInvoice;
using Domain.Entities;
using Domain.Options;

namespace Application.UnitTests.Configs.Fixtures
{
    public class ModelFixture
    {
        public IList<FixedInvoice> GetListFixedInvoice()
        {
            List<FixedInvoice> listFixedInvoice = new()
            {
                GetFixedInvoice()
            };

            return listFixedInvoice;
        }

        public FixedInvoice GetFixedInvoice()
        {
            return new FixedInvoice()
            {
                Id = 0,
                Name = "Teste",
                Account = "Teste2",
                Frequence = "Mensal",
                InitialMonthYear = "Janeiro/2024",
                FynallyMonthYear = null,
                Category = "Teste",
                Value = 200,
                BestPayDay = 25,
                CreationDate = new DateTime(2023, 12, 31, 0, 0, 0, kind: DateTimeKind.Local),
                HasRegistration = false,
                LastChangeDate = null

            };
        }

        public IList<BillToPay> GetListBillToPay()
        {
            List<BillToPay> listBillToPay = new()
            {
                GetBillToPay()
            };

            return listBillToPay;
        }

        public BillToPay GetBillToPay()
        {
            return new BillToPay
            {
                Id = Guid.NewGuid(),
                IdFixedInvoice = 1,
                Account = "",
                Name = "",
                Category = "",
                Value = 0,
                DueDate = new DateTime(2023, 12, 31, 0, 0, 0, kind: DateTimeKind.Local),
                YearMonth = "",
                Frequence = "",
                PayDay = "",
                HasPay = false
            };
        }

        public BillToPayOptions GetBillToPayOptions()
        {
            return new BillToPayOptions()
            {
                HowManyMonthForward = 24,
                HowManyYearsForward = 2,
                RoutineWorker = new RoutineWorker()
                {
                    Enable = true,
                    StartTime = new TimeSpan(0, 10, 0),
                    AddDays = false
                }
            };
        }

        public CreateFixedInvoiceInput GetCreateFixedInvoiceInput()
        {
            var fixedInvoice = GetFixedInvoice();

            return new CreateFixedInvoiceInput()
            {
                Id = fixedInvoice.Id,
                Name = fixedInvoice.Name,
                Frequence = fixedInvoice.Frequence,
                InitialMonthYear = fixedInvoice.InitialMonthYear,
                FynallyMonthYear = fixedInvoice.FynallyMonthYear,
                Category = fixedInvoice.Category,
                Value = fixedInvoice.Value,
                CreationDate = fixedInvoice.CreationDate,
                HasRegistration = fixedInvoice.HasRegistration,
                LastChangeDate = fixedInvoice.LastChangeDate
            };
        }
    }
}
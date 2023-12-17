using Domain.Entities;
using Domain.Options;

namespace Application.UnitTests.Fixtures
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
                Name = "",
                Account = "",
                Frequence = "",
                InitialMonthYear = "",
                FynallyMonthYear = "",
                Category = "",
                Value = 0,
                BestPayDay = 0,
                CreationDate = new DateTime(2023, 10, 10),
                HasRegistration = true,
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
                DueDate = new DateTime(2023, 12, 31),
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
    }
}
using Application.Feature.BillToPayRegistration.CreateBillToPayRegistration;
using Domain.Entities;
using Domain.Options;

namespace FinanceOrganization.UnitTests.Application.Configs.Fixtures
{
    public class ModelFixture
    {
        public IList<BillToPayRegistration> GetListBillToPayRegistration()
        {
            List<BillToPayRegistration> listBillToPayRegistration = new()
            {
                GetBillToPayRegistration()
            };

            return listBillToPayRegistration;
        }

        public BillToPayRegistration GetBillToPayRegistration()
        {
            return new BillToPayRegistration()
            {
                Id = 0,
                Name = "Teste",
                Account = "Teste2",
                Frequence = "Mensal",
                RegistrationType = "",
                InitialMonthYear = "Janeiro/2024",
                FynallyMonthYear = null,
                Category = "Teste",
                Value = 200,
                PurchaseDate = DateTime.Now,
                BestPayDay = 25,
                AdditionalMessage = "",
                CreationDate = new DateTime(2023, 12, 31, 0, 0, 0, kind: DateTimeKind.Local),
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
                IdBillToPayRegistration = 1,
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

        public CreateBillToPayRegistrationInput GetCreateBillToPayRegistrationInput()
        {
            var billToPayRegistration = GetBillToPayRegistration();

            return new CreateBillToPayRegistrationInput()
            {
                Name = billToPayRegistration.Name,
                Account = billToPayRegistration.Account,
                Frequence = billToPayRegistration.Frequence,
                RegistrationType = billToPayRegistration.RegistrationType,
                InitialMonthYear = billToPayRegistration.InitialMonthYear,
                FynallyMonthYear = billToPayRegistration.FynallyMonthYear,
                Category = billToPayRegistration.Category,
                Value = billToPayRegistration.Value,
                PurchaseDate = billToPayRegistration.PurchaseDate,
                BestPayDay = billToPayRegistration.BestPayDay,
                AdditionalMessage = billToPayRegistration.AdditionalMessage,
                CreationDate = billToPayRegistration.CreationDate,
                LastChangeDate = billToPayRegistration.LastChangeDate
            };
        }
    }
}
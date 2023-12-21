namespace Application.Feature.BillToPay.PayBillToPay
{
    public class PayBillToPayInput
    {
        public Guid Id { get; set; }
        public string? PayDay { get; set; } = string.Empty;
        public bool HasPay { get; set; } = false;
        public DateTime LastChangeDate { get; set; }
    }
}
namespace App.Forms.ViewModel
{
    public class PayBillToPayViewModel
    {
        public Guid? Id { get; set; }
        public string? PayDay { get; set; }
        public bool HasPay { get; set; } = false;
        public DateTime LastChangeDate { get; set; }
        public string? YearMonth { get; set; }
        public string? Account { get; set; }
    }
}
namespace Application.Feature.BillToPay.EditBillToPay
{
    public class EditBillToPayInput
    {
        public Guid Id { get; set; }
        public int IdFixedInvoice { get; set; }
        public string? Account { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public decimal Value { get; set; }
        public DateTime DueDate { get; set; }
        public string? YearMonth { get; set; }
        public string? Frequence { get; set; }
        public string? PayDay { get; set; }
        public bool HasPay { get; set; }
        public DateTime LastChangeDate { get; set; }
    }
}
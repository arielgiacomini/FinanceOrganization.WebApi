namespace Application.Feature.CreateFixedInvoice
{
    public class CreateFixedInvoiceInput
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Frequence { get; set; }
        public string? InitialMonthYear { get; set; }
        public string? FynallyMonthYear { get; set; }
        public string? Category { get; set; }
        public decimal Value { get; set; }
        public DateTime CreationDate { get; set; }
        public bool? HasRegistration { get; set; }
        public DateTime? LastChangeDate { get; set; }
    }
}
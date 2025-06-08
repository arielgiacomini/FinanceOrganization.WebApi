namespace Application.Feature.BillToPay.EditBillToPay
{
    public class EditBillToPayInput
    {
        public Guid Id { get; set; }
        public int IdBillToPayRegistration { get; set; }
        public string Account { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        /// <summary>
        /// Este campo faz parte do processo de identificação do item, deixando as opções de compra livre ou conta fixa.
        /// </summary>
        public string? RegistrationType { get; set; }
        public decimal Value { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime DueDate { get; set; }
        public string YearMonth { get; set; }
        public string Frequence { get; set; }
        public string PayDay { get; set; }
        public bool HasPay { get; set; }
        public string AdditionalMessage { get; set; }
        public DateTime LastChangeDate { get; set; }
        /// <summary>
        /// Deve editar a conta de registro (pai) desta.
        /// </summary>
        public bool MustEditRegistrationAccount { get; set; } = true;
    }
}
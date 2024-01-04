namespace Domain.Entities
{
    /// <summary>
    /// [Faturas/Contas Fixas] - Cadastro, será realizado criações de registros periodicamente, afim de automatizar as contas fixas.
    /// </summary>
    public class FixedInvoice
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Account { get; set; }
        public string? Frequence { get; set; }
        public string? RegistrationType { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? InitialMonthYear { get; set; }
        public string? FynallyMonthYear { get; set; }
        public string? Category { get; set; }
        public decimal Value { get; set; }
        public int BestPayDay { get; set; }
        public string? AdditionalMessage { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastChangeDate { get; set; }
    }
}
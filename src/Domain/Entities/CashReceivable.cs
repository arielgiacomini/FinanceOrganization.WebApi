namespace Domain.Entities
{
    public class CashReceivable
    {
        public Guid Id { get; set; }
        public int IdCashReceivableRegistration { get; set; }
        public string? Name { get; set; }
        public string? Account { get; set; }
        public string? Frequence { get; set; }
        public string? RegistrationType { get; set; }
        /// <summary>
        /// Data do acordo de recebimento
        /// </summary>
        public DateTime? AgreementDate { get; set; }
        /// <summary>
        /// Data de Vencimento da Conta a Receber
        /// </summary>
        public DateTime? DueDate { get; set; }
        public string? YearMonth { get; set; }
        public string? Category { get; set; }
        public decimal Value { get; set; }
        /// <summary>
        /// Data de Recebimento
        /// </summary>
        public DateTime? DateReceived { get; set; }
        /// <summary>
        /// Indica se a conta foi recebida
        /// </summary>
        public bool HasReceived { get; set; }
        /// <summary>
        /// Informações adicionais
        /// </summary>
        public string? AdditionalMessage { get; set; }
        /// <summary>
        /// Inativação - Delete Lógico da Tabela
        /// </summary>
        public bool? Enabled { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastChangeDate { get; set; }
    }
}
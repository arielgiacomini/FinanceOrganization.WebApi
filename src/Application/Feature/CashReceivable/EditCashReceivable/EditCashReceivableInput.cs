namespace Application.Feature.CashReceivable.EditCashReceivable
{
    public class EditCashReceivableInput
    {
        public Guid Id { get; set; }
        public int IdCashReceivableRegistration { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string Frequence { get; set; }
        public string RegistrationType { get; set; }
        /// <summary>
        /// Data do acordo de recebimento
        /// </summary>
        public DateTime? AgreementDate { get; set; }
        /// <summary>
        /// Data de Vencimento da Conta a Receber
        /// </summary>
        public DateTime? DueDate { get; set; }
        public string YearMonth { get; set; }
        public string Category { get; set; }
        /// <summary>
        /// Valor oficial, sem alterações. É o valor recebido.
        /// </summary>
        public decimal Value { get; set; }
        /// <summary>
        /// Valor manipulado. A cada conta a pagar na conta associada, esse campo é alterado. Como se fosse o saldo.
        /// </summary>
        public decimal ManipulatedValue { get; set; }
        /// <summary>
        /// Data de Recebimento
        /// </summary>
        public string DateReceived { get; set; }
        /// <summary>
        /// Indica se a conta foi recebida
        /// </summary>
        public bool HasReceived { get; set; }
        /// <summary>
        /// Informações adicionais
        /// </summary>
        public string AdditionalMessage { get; set; }
        /// <summary>
        /// Inativação - Delete Lógico da Tabela
        /// </summary>
        public bool? Enabled { get; set; }
        public DateTime? LastChangeDate { get; set; }

        /// <summary>
        /// Deve editar a conta de registro (pai) desta.
        /// </summary>
        public bool MustEditRegistrationAccount { get; set; } = true;
    }
}
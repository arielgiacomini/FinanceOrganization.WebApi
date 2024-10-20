namespace Application.Feature.CashReceivableRegistration.CreateCashReceivableRegistration
{
    public class CreateCashReceivableRegistrationInput
    {
        public Guid Id { get; set; }
        public int IdFixedInvoice { get; set; }

        /// <summary>
        /// Conta vinculada, Ex: Itaú, Cartão de Crédito, VA, VR, etc...
        /// </summary>
        public string? Account { get; set; }

        public string? Name { get; set; }
        public string? Category { get; set; }
        public decimal Value { get; set; }

        /// <summary>
        /// Data de acordo para receber
        /// </summary>
        public DateTime? AgreementDate { get; set; }

        /// <summary>
        /// Data de Vencimento data limite máximo
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Mês/Ano de Referência para Contas a Receber
        /// </summary>
        public string? YearMonth { get; set; }

        public string? Frequence { get; set; }

        /// <summary>
        /// Este campo faz parte do processo de identificação do item, deixando as opções de recebimento livre ou conta fixa.
        /// </summary>
        public string? RegistrationType { get; set; }

        /// <summary>
        /// Data de Recebimento
        /// </summary>
        public string? DateReceipt { get; set; }

        public bool HasReceived { get; set; }
        public string? AdditionalMessage { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastChangeDate { get; set; }
    }
}
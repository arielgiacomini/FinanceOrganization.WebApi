namespace Domain.Entities
{
    /// <summary>
    /// [Contas a pagar] - Todos os registros de contas a pagar existente
    /// </summary>
    public class BillToPay
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
        /// Data de Compra do determinado item
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// Data de Vencimento
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Mês/Ano de Referência para Contas à pagar
        /// </summary>
        public string? YearMonth { get; set; }

        public string? Frequence { get; set; }

        /// <summary>
        /// Este campo faz parte do processo de identificação do item, deixando as opções de compra livre ou conta fixa.
        /// </summary>
        public string? RegistrationType { get; set; }

        /// <summary>
        /// Data de Pagamento
        /// </summary>
        public string? PayDay { get; set; }

        public bool HasPay { get; set; }
        public string? AdditionalMessage { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastChangeDate { get; set; }
    }
}
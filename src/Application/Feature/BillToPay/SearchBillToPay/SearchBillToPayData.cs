namespace Application.Feature.BillToPay.SearchBillToPay
{
    public class SearchBillToPayData
    {
        public Guid Id { get; set; }
        public int IdBillToPayRegistration { get; set; }
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
        /// <summary>
        /// Quantidade de registros encontrados relacionado a conta fixa
        /// </summary>
        public int DetailsQuantity { get; set; } = 0;
        /// <summary>
        /// Valor total somado das contas livres relacionadas a conta fixa + conta fixa existente (atual)
        /// </summary>
        public decimal DetailsAmount { get; set; }
        public IList<Details>? Details { get; set; }
    }

    public class Details
    {
        public Guid Id { get; set; }
        public int IdBillToPayRegistration { get; set; }
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
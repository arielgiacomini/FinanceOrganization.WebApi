namespace Application.Feature.Account.CreateAccount
{
    public class CreateAccountInput
    {
        /// <summary>
        /// Descrição da Conta
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Dia de Vencimento
        /// </summary>
        public int? DueDate { get; set; }
        /// <summary>
        /// Dia de Fechamento
        /// </summary>
        public int? ClosingDay { get; set; }
        /// <summary>
        /// Considera Pago?
        /// </summary>
        public bool? ConsiderPaid { get; set; }
        /// <summary>
        /// Identificador Número da Agência da Conta
        /// </summary>
        public string AccountAgency { get; set; }
        /// <summary>
        /// Identificador Número da Conta
        /// </summary>
        public string AccountNumber { get; set; }
        /// <summary>
        /// Identificador Digito da Conta
        /// </summary>
        public string AccountDigit { get; set; }
        /// <summary>
        /// Por segurança contém apenas os últimos 4 digitos do cartão de crédito
        /// </summary>
        public string CardNumber { get; set; }
        /// <summary>
        /// Caso o tipo de conta for de recebimento de comissão, aqui fica o % de comissão que será cálculado e recebido.
        /// </summary>
        public decimal CommissionPercentage { get; set; }
        /// <summary>
        /// Indica se o registro ativo
        /// </summary>
        public bool Enable { get; set; }
    }
}
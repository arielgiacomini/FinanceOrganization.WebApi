namespace Domain.Entities
{
    public class Account
    {
        /// <summary>
        /// Identificador único da conta
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Descrição da Conta
        /// </summary>
        public string? Name { get; set; }
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
        public string? AccountAgency { get; set; }
        /// <summary>
        /// Identificador Número da Conta
        /// </summary>
        public string? AccountNumber { get; set; }
        /// <summary>
        /// Identificador Digito da Conta
        /// </summary>
        public string? AccountDigit { get; set; }
        /// <summary>
        /// Por segurança contém apenas os últimos 4 digitos do cartão de crédito
        /// </summary>
        public string? CardNumber { get; set; }
        /// <summary>
        /// Caso o tipo de conta for de recebimento de comissão, aqui fica o % de comissão que será cálculado e recebido.
        /// </summary>
        public decimal? CommissionPercentage { get; set; }
        /// <summary>
        /// Indica se o registro ativo
        /// </summary>
        public bool Enable { get; set; }
        /// <summary>
        /// Data de criação do registro
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// Data de alteração do registro
        /// </summary>
        public DateTime? LastChangeDate { get; set; }
        /// <summary>
        /// Indica se a conta registrada é um Cartão de Crédito.
        /// </summary>
        public bool IsCreditCard
        {
            get
            {
                var nameContainsCredit = Name?.Contains("Crédito") ?? false;
                if (!string.IsNullOrEmpty(CardNumber) && nameContainsCredit)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Cores desta conta
        /// </summary>
        public AccountColor Colors { get; set; }
    }
}
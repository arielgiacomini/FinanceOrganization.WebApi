using Domain.Entities.Enums;

namespace Application.Feature.Payment.AdjustPayament
{
    public class PaymentAdjustmentInput
    {
        /// <summary>
        /// Indica se o registro é uma conta a pagar ou a receber.
        /// </summary>
        public AccountType AccountType { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// Se for uma conta a Pagar e vier true , significa que o pagamento foi considerado pago.
        /// </summary>
        public bool ConsideredPaid { get; set; }
        /// <summary>
        /// Tipo de egistro, se for uma conta a pagar , pode ser: "Conta Fatura Fixa".
        /// </summary>
        public string RegistrationType { get; set; }
        /// <summary>
        /// Quantidade de meses a adicionar ao ano/mês do pagamento.
        /// </summary>
        public int QuantityMonthsAdd { get; set; }
        /// <summary>
        /// Frequência de Pagamento, por exemplo: "Mensal", "Anual", etc.
        /// </summary>
        public string Frequence { get; set; }
        public string Account { get; set; }
        /// <summary>
        /// Categoria
        /// </summary>
        public string Category { get; set; }
        public string YearMonth { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Value { get; set; }
        public string AdditionalMessage { get; set; }
        public DateTime LastChangeDate { get; set; }
    }
}
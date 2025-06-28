namespace Application.Feature.BillToPay.PayBillToPay
{
    public class PayBillToPayInput
    {
        public Guid? Id { get; set; }
        public string? PayDay { get; set; } = string.Empty;
        public bool HasPay { get; set; } = false;
        public DateTime LastChangeDate { get; set; }
        public string? YearMonth { get; set; }
        public string? Account { get; set; }
        /// <summary>
        /// Ideal para adiantamento de pagamento de cartão de crédito por exemplo, a fatura fecha no dia X porém descido pagar antecipado.
        /// </summary>
        public bool? AdvancePayment { get; set; }
        /// <summary>
        /// Indicates if the payment should be considered for Naira credit card transactions.
        /// </summary>
        [Obsolete]
        public bool? ConsiderNairaCreditCard { get; set; }
    }
}
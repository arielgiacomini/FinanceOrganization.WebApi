namespace Application.Feature.CashReceivable.ReceiveCashReceivable
{
    public class ReceiveCashReceivableInput
    {
        public Guid? Id { get; set; }
        /// <summary>
        /// O campo é string para salvar no padrão de data que o cliente desejar
        /// </summary>
        public string DateReceived { get; set; }
        /// <summary>
        /// Ideal para adiantamento de pagamento de cartão de crédito por exemplo, a fatura fecha no dia X porém descido pagar antecipado.
        /// </summary>
        public bool? AdvancePayment { get; set; }
    }
}
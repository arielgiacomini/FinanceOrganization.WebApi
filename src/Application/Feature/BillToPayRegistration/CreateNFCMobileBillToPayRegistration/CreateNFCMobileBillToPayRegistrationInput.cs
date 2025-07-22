namespace Application.Feature.BillToPayRegistration.CreateCreditCardNFCMobileBillToPayRegistration
{
    public class CreateNFCMobileBillToPayRegistrationInput
    {
        public string Name { get; set; }
        /// <summary>
        /// Cartão de Crédito | Cartão de Débito | VR | VA | Itaú
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// Livre | Mensal | Mensal:Recorrente
        /// </summary>
        public string Frequence { get; set; }
        /// <summary>
        /// Compra Livre | Conta/Fatura Fixa
        /// </summary>
        public string RegistrationType { get; set; }
        public string Category { get; set; }
        public decimal Value { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string AdditionalMessage { get; set; }
    }
}
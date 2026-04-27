namespace Application.Feature.BillToPayRegistration.DisableBillToPayRegistration
{
    public class DisableBillToPayRegistrationInput
    {
        /// <summary>
        /// Id do cadastro da conta a pagar, necessário para realizar a desativação do mesmo.
        /// </summary>
        public int Id { get; set; }
    }
}
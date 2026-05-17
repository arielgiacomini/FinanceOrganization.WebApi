namespace Application.Feature.Wallet.CreateWallet
{
    public class CreateWalletInput
    {
        /// <summary>
        /// Identificador único da carteira
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// Chave da Carteira
        /// </summary>
        public string WalletKey { get; set; }
        /// <summary>
        /// Conteúdo da Carteira
        /// </summary>
        public string WalletValue { get; set; }
        /// <summary>
        /// Data de criação do registro
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
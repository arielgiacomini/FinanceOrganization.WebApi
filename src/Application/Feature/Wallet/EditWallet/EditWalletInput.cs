namespace Application.Feature.Wallet.EditWallet
{
    public class EditWalletInput
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
        /// <summary>
        /// Data de alteração do registro
        /// </summary>
        public DateTime? LastChangeDate { get; set; }
    }
}
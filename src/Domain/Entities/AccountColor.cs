namespace Domain.Entities
{
    public class AccountColor
    {
        /// <summary>
        /// Id único da cor da determinada conta
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Id da conta
        /// </summary>
        public int AccountId { get; set; }
        /// <summary>
        /// Uitlizado para indicar a cor de fundo da tela que usa a ceterminada conta
        /// </summary>
        public string BackgroundColorHexadecimal { get; set; }
        /// <summary>
        /// Utilizado para indicar a cor das fontes desta conta
        /// </summary>
        public string FonteColorHexadecimal { get; set; }
        /// <summary>
        /// Indica se o registro está ativo ou não
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
    }
}
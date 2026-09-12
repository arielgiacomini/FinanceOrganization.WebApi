namespace Application.Feature.BillToPayRegistration.CreateBillToPayRegistration
{

    public class CreateBillToPayRegistrationInput
    {
        /// <summary>
        /// Id de um cadastro de conta a pagar (CONTA_PAGAR_CADASTRO) já existente para associar esta conta a
        /// pagar. Quando informado, este registro não passa por CONTA_PAGAR_CADASTRO nem pela rotina em
        /// background: é cadastrado direto na tabela oficial CONTA_PAGAR, já vinculado a esse Id — útil para
        /// preencher rapidamente um mês (InitialMonthYear) que ficou faltando, sem repetir todos os dados.
        /// </summary>
        public int? IdBillToPayRegistration { get; set; }
        public string? Name { get; set; }
        public string? Account { get; set; }
        public string? Frequence { get; set; }

        /// <summary>
        /// Este campo faz parte do processo de identificação do item, deixando as opções de compra livre ou conta fixa.
        /// </summary>
        public string? RegistrationType { get; set; }

        public string? InitialMonthYear { get; set; }
        public string? FynallyMonthYear { get; set; }
        public string? Category { get; set; }
        public decimal Value { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public int? BestPayDay { get; set; }
        public string? AdditionalMessage { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastChangeDate { get; set; }
        /// <summary>
        /// Gets or sets the country associated with the entity.
        /// </summary>
        public string? Country { get; set; }
    }
}
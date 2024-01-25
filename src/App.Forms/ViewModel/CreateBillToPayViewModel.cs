using App.Forms.Enums;

namespace App.Forms.ViewModel
{
    public class CreateBillToPayViewModel
    {
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
        public bool RegistrationSuccess { get; set; }
        public RegistrationStatus Status { get; set; } = RegistrationStatus.AwaitRequestAPI;
    }
}
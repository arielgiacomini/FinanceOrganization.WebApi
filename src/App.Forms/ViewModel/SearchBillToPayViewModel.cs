namespace App.Forms.ViewModel
{
    public class SearchBillToPayViewModel
    {
        /// <summary>
        /// Id identificador do BillToPay
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Nome/Descrição da conta a pagar cadastrada.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Mês/Ano de referência, gastos do determinado período.
        /// </summary>
        public string? YearMonth { get; set; }

        /// <summary>
        /// Data de Vencimento
        /// </summary>
        public DateTime? DueDate { get; set; }
    }
}
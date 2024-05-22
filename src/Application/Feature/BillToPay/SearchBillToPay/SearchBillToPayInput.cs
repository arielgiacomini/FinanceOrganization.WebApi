namespace Application.Feature.BillToPay.SearchBillToPay
{
    public class SearchBillToPayInput
    {
        public Guid[]? Id { get; set; }
        public int[]? IdFixedInvoices { get; set; }

        /// <summary>
        /// Mês/Ano de referência, gastos do determinado período.
        /// </summary>
        public string? YearMonth { get; set; }
        /// <summary>
        /// A ideia deste campo é apresentar as informações não relacionadas diretamente. Ex.: Um gasto fixo de Padaria do mês de Maio/2025 total é de R$ 400,00 porém toda a lista de gasto que foi efetuada durante o período será apresentada quando essa propriedade for TRUE
        /// </summary>
        public bool? ShowDetails { get; set; }
    }
}
namespace Application.Feature.BillToPay.DeleteBillToPay
{
    public class DeleteBillToPayInput
    {
        public Guid[]? Id { get; set; }
        public int[]? IdFixedInvoices { get; set; }

        /// <summary>
        /// Se TRUE Apenas os registros não pagos
        /// </summary>
        public bool JustUnpaid { get; set; }
    }
}
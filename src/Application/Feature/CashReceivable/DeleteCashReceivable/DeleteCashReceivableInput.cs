namespace Application.Feature.CashReceivable.DeleteCashReceivable
{
    public class DeleteCashReceivableInput
    {
        public Guid[]? Id { get; set; }
        public int[]? IdCashReceivableRegistrations { get; set; }

        /// <summary>
        /// Se TRUE Apenas os registros não recebidos
        /// </summary>
        public bool OnlyNotReceivable { get; set; }
        /// <summary>
        /// Caso marcada como TRUE é para desconsiderar em eventos futuros que podem ser criados.
        /// </summary>
        public bool DisableCashReceivableRegistration { get; set; }
    }
}
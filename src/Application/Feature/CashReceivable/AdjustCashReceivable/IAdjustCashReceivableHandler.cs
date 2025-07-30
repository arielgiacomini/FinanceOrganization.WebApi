namespace Application.Feature.CashReceivable.AdjustCashReceivable
{
    public interface IAdjustCashReceivableHandler
    {
        Task Adjust<T>(T input) where T : class;
    }
}
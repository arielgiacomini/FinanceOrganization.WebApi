namespace Application.Feature.CashReceivableLogic
{
    public interface IAdjustCashReceivable
    {
        Task AdjustCashReceivableManipulatedValue<T>(T input) where T : class;
    }
}
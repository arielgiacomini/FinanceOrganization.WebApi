namespace Application.Feature.CashReceivable.ReceiveCashReceivable
{
    public interface IReceiveCashReceivableHandler
    {
        Task<ReceiveCashReceivableOutput> Handle(ReceiveCashReceivableInput input, CancellationToken cancellationToken);
    }
}
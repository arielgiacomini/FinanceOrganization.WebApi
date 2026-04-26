namespace Application.Feature.CashReceivable.DeleteCashReceivable
{
    public interface IDeleteCashReceivableHandler
    {
        Task<DeleteCashReceivableOutput> Handle(DeleteCashReceivableInput input, CancellationToken cancellationToken);
    }
}

namespace Application.Feature.CashReceivable.EditCashReceivable
{
    public interface IEditCashReceivableHandler
    {
        Task<EditCashReceivableOutput> Handle(EditCashReceivableInput input, CancellationToken cancellationToken);
    }
}

namespace Application.EventHandlers.CreateCashReceivableEvent
{
    public interface ICreateCashReceivableEventHandler
    {
        Task Handle(CreateCashReceivableEventInput input);
    }
}
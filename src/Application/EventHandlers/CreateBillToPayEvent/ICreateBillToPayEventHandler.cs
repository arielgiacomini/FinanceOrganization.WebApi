namespace Application.EventHandlers.CreateBillToPayEvent
{
    public interface ICreateBillToPayEventHandler
    {
        Task Handle(CreateBillToPayInput input);
    }
}
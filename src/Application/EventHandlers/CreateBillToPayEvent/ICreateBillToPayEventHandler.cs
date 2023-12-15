namespace Application.EventHandlers.CreateBillToPayEvent
{
    public interface ICreateBillToPayEventHandler
    {
        Task Handle(CreateBillToPayEventInput input);
    }
}
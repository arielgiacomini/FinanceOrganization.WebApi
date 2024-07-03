
namespace Application.EventHandlers.CreateCategoryEvent
{
    public interface ICreateCategoryEventHandler
    {
        Task Handle(CreateCategoryEventInput input);
    }
}
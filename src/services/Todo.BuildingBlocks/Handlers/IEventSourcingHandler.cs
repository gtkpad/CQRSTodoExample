using Todo.BuildingBlocks.Entities;

namespace Todo.BuildingBlocks.Handlers;

public interface IEventSourcingHandler<T> where T : Entity
{
    Task SaveAsync(T aggregate);
    Task<T> GetByIdAsync(Guid id);

    Task RepublishEventsAsync();
}
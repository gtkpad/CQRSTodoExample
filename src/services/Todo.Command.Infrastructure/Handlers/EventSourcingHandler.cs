using Todo.BuildingBlocks.Handlers;
using Todo.BuildingBlocks.Stores;

namespace Todo.Command.Infrastructure.Handlers;

public class EventSourcingHandler : IEventSourcingHandler<Domain.Entities.Todo>
{
    private readonly IEventStore _eventStore;

    public EventSourcingHandler(IEventStore eventStore)
    {
        _eventStore = eventStore;
    }

    public async Task SaveAsync(Domain.Entities.Todo aggregate)
    {
        await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges());
        aggregate.MarkChangesAsCommitted();
    }

    public async Task<Domain.Entities.Todo> GetByIdAsync(Guid id)
    {
        var aggregate = new Domain.Entities.Todo();
        var events = await _eventStore.GetEventsAsync(id);
        
        if (events == null || !events.Any()) return aggregate;
        
        aggregate.ReplayEvents(events);
         
        return aggregate;
    }

    public Task RepublishEventsAsync()
    {
        throw new NotImplementedException();
    }
}
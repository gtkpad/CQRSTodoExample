using Todo.BuildingBlocks.Events;
using Todo.BuildingBlocks.Repository;
using Todo.BuildingBlocks.Stores;

namespace Todo.Command.Infrastructure.Stores;

public class EventStore : IEventStore
{
    private readonly IEventStoreRepository _repository;

    public EventStore(IEventStoreRepository repository)
    {
        _repository = repository;
    }

    public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events)
    {
        var eventStream = await _repository.FindByAggregateId(aggregateId);

        foreach (var @event in events)
        {
            await _repository.AppendAsync(@event);
        }
    }

    public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
    {
        var eventStream = await _repository.FindByAggregateId(aggregateId);
        
        if (eventStream == null || !eventStream.Any())
        {
            throw new Exception("Invalid aggregate id");
        }

        return eventStream;
    }

    public Task<List<Guid>> GetAggregateIdsAsync()
    {
        throw new NotImplementedException();
    }
}
using Todo.BuildingBlocks.Events;

namespace Todo.BuildingBlocks.Stores;

public interface IEventStore
{
    Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events);
    Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);
    
    Task<List<Guid>> GetAggregateIdsAsync();
}
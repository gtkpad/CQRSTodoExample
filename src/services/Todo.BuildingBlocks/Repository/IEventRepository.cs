using Todo.BuildingBlocks.Entities;
using Todo.BuildingBlocks.Events;

namespace Todo.BuildingBlocks.Repository;

public interface IEventStoreRepository
{ Task AppendAsync(BaseEvent @event);
    Task<List<BaseEvent>> FindByAggregateId(Guid aggregateId);
}
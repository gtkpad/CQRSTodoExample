using Todo.BuildingBlocks.Events;

namespace Todo.Query.Infrastructure.Handlers;

public interface ITodoEventHandler 
{
    Task Subscribe();
    Task Handle(TodoCreatedEvent @event);
    Task Handle(TodoMarkedAsDone @event);
    Task Handle(TodoMarkedAsUndoneEvent @event);
    Task Handle(TodoNameUpdatedEvent @event);
}
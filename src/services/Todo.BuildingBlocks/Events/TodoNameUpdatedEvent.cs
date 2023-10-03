namespace Todo.BuildingBlocks.Events;

public class TodoNameUpdatedEvent : BaseEvent
{
    public string Name { get; set; }
}
namespace Todo.BuildingBlocks.Events;

public class TodoCreatedEvent : BaseEvent
{
    public string Name { get; set; }
    
    public bool Done { get; set; }
    public DateTime Date { get; set; }
}
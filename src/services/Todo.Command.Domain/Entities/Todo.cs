using Flunt.Validations;
using Todo.BuildingBlocks.Entities;
using Todo.BuildingBlocks.Events;

namespace Todo.Command.Domain.Entities;

public class Todo : Entity
{
    public string Name { get; private set; }
    public bool Done { get; private set; }
    public DateTime Date { get; private set; }
    
    public Todo()
    {
    }

    public Todo(string name, DateTime date)
    {
        RaiseEvent(new TodoCreatedEvent
        {
            Id = Guid.NewGuid(),
            Done = false,
            Name = name, 
            Date = date, 
        });
    }

    public void Apply(TodoCreatedEvent @event)
    {
        _id = @event.Id;
        Name = @event.Name;
        Date = @event.Date;
        Done = @event.Done;
        
        _validate();
    }

    private void _validate()
    {
        AddNotifications(
            new Contract<Todo>().Requires()
                .IsNotNullOrEmpty(Name, nameof(Name), "Name is required")
                .IsNotNull(Date, nameof(Date), "Date is required")
                .IsNotNullOrEmpty(Id.ToString(), nameof(Id), "Id is required")
        );
    }

    public void MarkAsDone()
    {
        if (Done)
            return;
        
        RaiseEvent(new TodoMarkedAsDone
        {
            Id = Id
        });
    }
    
    public void Apply(TodoMarkedAsDone @event)
    {
        Done = true;
        _validate();
    }

    public void MarkAsUndone()
    {
        if (!Done)
            return;
        
        RaiseEvent(new TodoMarkedAsUndoneEvent
        {
            Id = Id
        });
    }
    
    public void Apply(TodoMarkedAsUndoneEvent @event)
    {
        Done = false;
        _validate();
    }
    
    public void Rename(string name)
    {
        RaiseEvent(new TodoNameUpdatedEvent
        {
            Id = Id, Name = name
        });
    }
    
    public void Apply(TodoNameUpdatedEvent @event)
    {
        Name = @event.Name;
        _validate();
    }

}
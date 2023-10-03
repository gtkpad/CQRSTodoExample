using Flunt.Notifications;
using Todo.BuildingBlocks.Events;

namespace Todo.BuildingBlocks.Entities;

public abstract class Entity : Notifiable<Notification>
{
    protected Guid _id;
    
    public Guid Id => _id;
    
    private readonly List<BaseEvent> _changes = new();
    
    public IEnumerable<BaseEvent> GetUncommittedChanges() => _changes.AsEnumerable();
    
    public void MarkChangesAsCommitted() => _changes.Clear();
    
    private void ApplyChange(BaseEvent @event, bool isNew)
    {
        var method = this.GetType().GetMethod("Apply", new Type[] { @event.GetType() });    
        
        if (method == null)
        {
            throw new ArgumentNullException( nameof(method), $"No Apply method found for event {@event.GetType().Name}");
        }
        
        method.Invoke(this, new object[] { @event });
        
        if (isNew)
        {
            _changes.Add(@event);
        }
    }

    protected void RaiseEvent(BaseEvent @event)
    {
        ApplyChange(@event, true);
    }
    
    public void ReplayEvents(IEnumerable<BaseEvent> events)
    {
        foreach (var @event in events)
        {
            ApplyChange(@event, false);
        }
    }
}
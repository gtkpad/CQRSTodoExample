using Todo.BuildingBlocks.Entities;

namespace Todo.Query.Domain.Entities;

public class Todo
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public bool Done { get; private set; }
    public DateTime Date { get; private set; }
    
    public Todo()
    {
        
    }
    
    public Todo(Guid id, string name, bool done, DateTime date)
    {
        Id = id;
        Name = name;
        Done = done;
        Date = date;
    }
    
    public void MarkAsDone()
    {
        Done = true;
    }
    
    public void MarkAsUndone()
    {
        Done = false;
    }
    
    public void ChangeName(string name)
    {
        Name = name;
    }
}
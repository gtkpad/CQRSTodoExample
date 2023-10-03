using System.Collections.ObjectModel;

namespace Todo.BuildingBlocks.Errors;

public class EntityValidationError : Exception
{
    public ReadOnlyCollection<EntityFieldError> Errors { get; }
    
    public EntityValidationError(string message, IList<EntityFieldError> errors) : base(message)
    {
        Errors = new ReadOnlyCollection<EntityFieldError>(errors);
    }
}

public class EntityFieldError
{
    public string Field { get; set; }
    public string Message { get; set; }
    
    public EntityFieldError(string field, string message)
    {
        Field = field;
        Message = message;
    }
}
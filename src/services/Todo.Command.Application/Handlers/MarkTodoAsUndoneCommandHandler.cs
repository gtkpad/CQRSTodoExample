using Todo.BuildingBlocks.Commands;
using Todo.BuildingBlocks.Errors;
using Todo.BuildingBlocks.Handlers;
using Todo.Command.Domain.Commands;

namespace Todo.Command.Application.Handlers;

public class MarkTodoAsUndoneCommandHandler : ICommandHandler<MarkTodoAsUndoneCommand>
{
    private readonly IEventSourcingHandler<Domain.Entities.Todo> _eventSourcingHandler;

    public MarkTodoAsUndoneCommandHandler(IEventSourcingHandler<Domain.Entities.Todo> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }
    

    public async Task<bool> Handle(MarkTodoAsUndoneCommand command, CancellationToken cancellationToken)
    {
        var todo = await _eventSourcingHandler.GetByIdAsync(command.Id);
        todo.MarkAsUndone();
        
        if (!todo.IsValid)
        {
            var errors = todo.Notifications.Select(e => new EntityFieldError(e.Key, e.Message)).ToList();
            throw new EntityValidationError("Error on Mark Todo as Undone", errors);
        }

        await _eventSourcingHandler.SaveAsync(todo);

        return true;
    }
}
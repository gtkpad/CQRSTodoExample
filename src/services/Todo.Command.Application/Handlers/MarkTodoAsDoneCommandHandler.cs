using Todo.BuildingBlocks.Commands;
using Todo.BuildingBlocks.Errors;
using Todo.BuildingBlocks.Handlers;
using Todo.Command.Domain.Commands;

namespace Todo.Command.Application.Handlers;

public class MarkTodoAsDoneCommandHandler : ICommandHandler<MarkTodoAsDoneCommand>
{
    private readonly IEventSourcingHandler<Domain.Entities.Todo> _eventSourcingHandler;

    public MarkTodoAsDoneCommandHandler(IEventSourcingHandler<Domain.Entities.Todo> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }

    public async Task<bool> Handle(MarkTodoAsDoneCommand command, CancellationToken token)
    {
        var todo = await _eventSourcingHandler.GetByIdAsync(command.Id);
        todo.MarkAsDone();
        
        if (!todo.IsValid)
        {
            var errors = todo.Notifications.Select(e => new EntityFieldError(e.Key, e.Message)).ToList();
            throw new EntityValidationError("Error on Mark Todo as Done", errors);
        }
        
        await _eventSourcingHandler.SaveAsync(todo);

        return true;
    }
}
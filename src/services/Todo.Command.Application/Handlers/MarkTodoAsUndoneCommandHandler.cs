using Todo.BuildingBlocks.Commands;
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
        await _eventSourcingHandler.SaveAsync(todo);

        return true;
    }
}
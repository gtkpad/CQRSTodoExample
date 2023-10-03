using Todo.BuildingBlocks.Commands;
using Todo.BuildingBlocks.Handlers;
using Todo.Command.Domain.Commands;

namespace Todo.Command.Application.Handlers;

public class RenameTodoCommandHandler : ICommandHandler<RenameTodoCommand>
{
    private readonly IEventSourcingHandler<Domain.Entities.Todo> _eventSourcingHandler;

    public RenameTodoCommandHandler(IEventSourcingHandler<Domain.Entities.Todo> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }

    public async Task<bool> Handle(RenameTodoCommand command, CancellationToken token)
    {
        var todo = await _eventSourcingHandler.GetByIdAsync(command.Id);
        todo.Rename(command.Name);
        await _eventSourcingHandler.SaveAsync(todo);
        
        return true;
    }
}
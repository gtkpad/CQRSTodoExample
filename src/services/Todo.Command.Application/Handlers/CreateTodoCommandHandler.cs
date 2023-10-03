using Todo.BuildingBlocks.Commands;
using Todo.BuildingBlocks.Handlers;
using Todo.Command.Domain.Commands;

namespace Todo.Command.Application.Handlers;

public class CreateTodoCommandHandler : ICommandHandler<CreateTodoCommand>
{
    private readonly IEventSourcingHandler<Domain.Entities.Todo> _eventSourcingHandler;

    public CreateTodoCommandHandler(IEventSourcingHandler<Domain.Entities.Todo> eventSourcingHandler)
    {
        _eventSourcingHandler = eventSourcingHandler;
    }

    public async Task<bool> Handle(CreateTodoCommand command, CancellationToken token)
    {
        var todo = new Domain.Entities.Todo(command.Name, command.Date);
        await _eventSourcingHandler.SaveAsync(todo);
        
        return true;
    }
}
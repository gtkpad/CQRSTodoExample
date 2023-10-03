using MediatR;
using Todo.BuildingBlocks.Commands;

namespace Todo.Command.Domain.Commands;

public class MarkTodoAsUndoneCommand : BaseCommand
{
    public Guid Id { get; set; }
}
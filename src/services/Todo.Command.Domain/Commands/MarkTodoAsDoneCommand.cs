using MediatR;
using Todo.BuildingBlocks.Commands;

namespace Todo.Command.Domain.Commands;

public class MarkTodoAsDoneCommand : BaseCommand
{
    public Guid Id { get; set; }
}
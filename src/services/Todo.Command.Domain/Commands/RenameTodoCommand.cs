using Todo.BuildingBlocks.Commands;

namespace Todo.Command.Domain.Commands;

public class RenameTodoCommand : BaseCommand
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
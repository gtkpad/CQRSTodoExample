using Todo.BuildingBlocks.Commands;

namespace Todo.Command.Domain.Commands;

public class CreateTodoCommand : BaseCommand
{
    public string Name { get; set; }
    public DateTime Date { get; set; }
}
using MediatR;

namespace Todo.BuildingBlocks.Commands;

public interface ICommandHandler<TC> :  IRequestHandler<TC, bool> where TC : BaseCommand
{
}

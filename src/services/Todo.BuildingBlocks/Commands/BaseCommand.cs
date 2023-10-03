using MediatR;

namespace Todo.BuildingBlocks.Commands;

public abstract class BaseCommand : IRequest<bool>
{
    
}
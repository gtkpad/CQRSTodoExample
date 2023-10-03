using MediatR;

namespace Todo.BuildingBlocks.Queries;

public abstract class BaseQuery<T> : IRequest<T> 
{
    
}
using MediatR;
namespace Todo.BuildingBlocks.Queries;

public interface IQueryHandler<TC, Resp> :  IRequestHandler<TC, Resp> where TC : BaseQuery<Resp>
{
}

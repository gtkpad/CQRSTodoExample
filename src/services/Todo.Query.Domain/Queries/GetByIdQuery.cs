using Todo.BuildingBlocks.Queries;

namespace Todo.Query.Domain.Queries;

public class GetByIdQuery: BaseQuery<Entities.Todo>
{
    public Guid Id { get; set; }
}
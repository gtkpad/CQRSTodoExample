using Todo.BuildingBlocks.Queries;
using Todo.Query.Domain.Queries;
using Todo.Query.Domain.Repositories;

namespace Todo.Query.Application.Query.Handlers;

public class GetUndoneHandler : IQueryHandler<GetUndoneQuery, IList<Domain.Entities.Todo>>
{
    private readonly ITodoRepository _todoRepository;

    public GetUndoneHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<IList<Domain.Entities.Todo>> Handle(GetUndoneQuery request, CancellationToken cancellationToken)
    {
        return await _todoRepository.GetUndone();
    }
}
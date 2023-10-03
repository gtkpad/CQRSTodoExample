using Todo.BuildingBlocks.Queries;
using Todo.Query.Domain.Queries;
using Todo.Query.Domain.Repositories;

namespace Todo.Query.Application.Query.Handlers;

public class GetDoneHandler : IQueryHandler<GetDoneQuery, IList<Domain.Entities.Todo>>
{
    private readonly ITodoRepository _todoRepository;

    public GetDoneHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<IList<Domain.Entities.Todo>> Handle(GetDoneQuery request, CancellationToken cancellationToken)
    {
        return await _todoRepository.GetDone();
    }
}
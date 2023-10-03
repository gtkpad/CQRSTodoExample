using System.Collections;
using Todo.BuildingBlocks.Queries;
using Todo.Query.Domain.Queries;
using Todo.Query.Domain.Repositories;

namespace Todo.Query.Application.Query.Handlers;

public class GetAllHandler : IQueryHandler<GetAllQuery, IList<Domain.Entities.Todo>>
{
    private readonly ITodoRepository _todoRepository;

    public GetAllHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<IList<Domain.Entities.Todo>> Handle(GetAllQuery request, CancellationToken cancellationToken)
    {
        return await _todoRepository.GetAll();
    }
}
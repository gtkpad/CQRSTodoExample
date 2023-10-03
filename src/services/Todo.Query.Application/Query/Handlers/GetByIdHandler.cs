using System.Collections;
using Todo.BuildingBlocks.Queries;
using Todo.Query.Domain.Queries;
using Todo.Query.Domain.Repositories;

namespace Todo.Query.Application.Query.Handlers;

public class GetByIdHandler : IQueryHandler<GetByIdQuery, Domain.Entities.Todo>
{
    private readonly ITodoRepository _todoRepository;

    public GetByIdHandler(ITodoRepository todoRepository)
    {
        _todoRepository = todoRepository;
    }

    public async Task<Domain.Entities.Todo> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        return await _todoRepository.GetById(request.Id);
    }
}
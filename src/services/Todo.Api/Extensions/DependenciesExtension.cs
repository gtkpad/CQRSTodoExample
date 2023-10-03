using System.Collections;
using MediatR;
using Todo.BuildingBlocks.Commands;
using Todo.Command.Application.Handlers;
using Todo.Command.Domain.Commands;
using Todo.Query.Application.Query.Handlers;
using Todo.Query.Domain.Queries;

namespace Todo.Api.Extensions;

public static class DependenciesExtension
{
    public static void InjectCommandHandlers(this IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<CreateTodoCommand, bool>, CreateTodoCommandHandler>();
        services.AddScoped<IRequestHandler<MarkTodoAsDoneCommand, bool>, MarkTodoAsDoneCommandHandler>();
        services.AddScoped<IRequestHandler<MarkTodoAsUndoneCommand, bool>, MarkTodoAsUndoneCommandHandler>();
        services.AddScoped<IRequestHandler<RenameTodoCommand, bool>, RenameTodoCommandHandler>();
    }
    
    public static void InjectQueryHandlers(this IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<GetByIdQuery, Query.Domain.Entities.Todo>, GetByIdHandler>();
        
        services.AddScoped<IRequestHandler<GetAllQuery, IList<Query.Domain.Entities.Todo>>, GetAllHandler>();
        services.AddScoped<IRequestHandler<GetDoneQuery, IList<Query.Domain.Entities.Todo>>, GetDoneHandler>();
        services.AddScoped<IRequestHandler<GetUndoneQuery, IList<Query.Domain.Entities.Todo>>, GetUndoneHandler>();
    }
}
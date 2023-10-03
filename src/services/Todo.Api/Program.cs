using System.Reflection;
using EventStore.Client;
using MediatR;
using Todo.Api.Extensions;
using Todo.BuildingBlocks.Commands;
using Todo.BuildingBlocks.Handlers;
using Todo.BuildingBlocks.Repository;
using Todo.BuildingBlocks.Stores;
using Todo.Command.Application.Handlers;
using Todo.Command.Domain.Commands;
using Todo.Command.Infrastructure.Handlers;
using Todo.Command.Infrastructure.Repository;
using Todo.Query.Domain.Repositories;
using Todo.Query.Infrastructure.Config;
using Todo.Query.Infrastructure.Handlers;
using Todo.Query.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

builder.Services.Configure<MongodbConfig>(builder.Configuration.GetSection(nameof(MongodbConfig)));


var settings = EventStoreClientSettings
    .Create("esdb://localhost:2113?tls=false");
var client = new EventStoreClient(settings);

var settingsPersistent = EventStoreClientSettings
    .Create("esdb://localhost:2113?tls=false");
var persistentSubscriptionsClient = new EventStorePersistentSubscriptionsClient(settings);

builder.Services.AddSingleton<EventStoreClient>(_ => client);

builder.Services.AddSingleton<EventStorePersistentSubscriptionsClient>(_ => persistentSubscriptionsClient);
builder.Services.AddScoped<IEventStoreRepository, TodoEventStoreRepository>();
builder.Services.AddScoped<IEventStore, Todo.Command.Infrastructure.Stores.EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<Todo.Command.Domain.Entities.Todo>, EventSourcingHandler>();
builder.Services.AddSingleton<ITodoRepository, TodoRepository>();

builder.Services.AddSingleton<ITodoEventHandler, TodoEventHandler>();


builder.Services.InjectCommandHandlers();
builder.Services.InjectQueryHandlers();


builder.Services.AddHostedService<TodoEventHandlerHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
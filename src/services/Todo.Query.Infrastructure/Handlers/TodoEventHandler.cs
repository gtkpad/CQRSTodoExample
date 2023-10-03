

using System.Text;
using System.Text.Json;
using EventStore.Client;
using Todo.BuildingBlocks.Converters;
using Todo.BuildingBlocks.Events;
using Todo.Query.Domain.Entities;
using Todo.Query.Domain.Repositories;

namespace Todo.Query.Infrastructure.Handlers;

public class TodoEventHandler : ITodoEventHandler
{
    private readonly EventStorePersistentSubscriptionsClient _eventStoreClient;
    private readonly ITodoRepository _todoRepository;

    public TodoEventHandler(EventStorePersistentSubscriptionsClient eventStoreClient, ITodoRepository todoRepository)
    {
        _eventStoreClient = eventStoreClient;
        _todoRepository = todoRepository;
    }

    public async Task Subscribe()
    {
        
        var subscription = await _eventStoreClient.SubscribeToStreamAsync(
            "$ce-todo",
            "read-api",
            async (subscription, evnt, retryCount, cancellationToken) => {
                                
                
                var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };

                var json = Encoding.UTF8.GetString(evnt.Event.Metadata.ToArray());
                var @event = JsonSerializer.Deserialize<BaseEvent>(json, options);
                
                var handler = GetType().GetMethod("Handle", new Type[] { @event.GetType() });
                
                if (handler == null)
                {
                    throw new ArgumentNullException(nameof(handler), $"Could not find event handler method!");
                }
                
                await ((Task)handler.Invoke(this, new object[] { @event }))!;
                
                await subscription.Ack(evnt);
            });

    }

    public async Task Handle(TodoCreatedEvent @event)
    {
        Console.WriteLine("OI");
        await _todoRepository.Create(new Domain.Entities.Todo(
            @event.Id,
            @event.Name,
            @event.Done,
            @event.Date
            ));
    }

    public async Task Handle(TodoMarkedAsDone @event)
    {
        var todo = await _todoRepository.GetById(@event.Id);
        todo.MarkAsDone();
        
        await _todoRepository.Update(todo);
    }

    public async Task Handle(TodoMarkedAsUndoneEvent @event)
    {
        var todo = await _todoRepository.GetById(@event.Id);
        todo.MarkAsUndone();
            
        await _todoRepository.Update(todo);
    }

    public async Task Handle(TodoNameUpdatedEvent @event)
    {
        var todo = await _todoRepository.GetById(@event.Id);
        
        todo.ChangeName(@event.Name);
        
        await _todoRepository.Update(todo);
    }
}
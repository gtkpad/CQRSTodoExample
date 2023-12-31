﻿using System.Text;
using System.Text.Json;
using EventStore.Client;
using Todo.BuildingBlocks.Converters;
using Todo.BuildingBlocks.Events;
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

    private async Task CreateSubscription()
    {
        try
        {
            var filter = StreamFilter.Prefix("todo-events-");

            var settings = new PersistentSubscriptionSettings();
            await _eventStoreClient.CreateToAllAsync(
                "read-api",
                filter,
                settings);
        }
        catch
        {
            // ignored
        }
    }

    public async Task Subscribe()
    {
        await CreateSubscription();
        var subscription = await _eventStoreClient.SubscribeToAllAsync(
            "read-api",
            async (subscription, evnt, retryCount, cancellationToken) =>
            {
                var options = new JsonSerializerOptions { Converters = { new EventJsonConverter(evnt.Event.EventType) } };

                var json = Encoding.UTF8.GetString(evnt.Event.Data.ToArray());
                
                var @event = JsonSerializer.Deserialize<BaseEvent>(json, options);

                var handler = GetType().GetMethod("Handle", new Type[] { @event.GetType() });

                if (handler == null)
                {
                    throw new ArgumentNullException(nameof(handler), $"Could not find event handler method!");
                }

                await ((Task)handler.Invoke(this, new object[] { @event }))!;

                await subscription.Ack(evnt);
            },
            (subscription, reason, ex) =>
            {
                Console.WriteLine($"Subscription dropped due to {reason}");
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
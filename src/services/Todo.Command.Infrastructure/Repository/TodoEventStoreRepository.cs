using System.Text;
using System.Text.Json;
using EventStore.Client;
using Todo.BuildingBlocks.Converters;
using Todo.BuildingBlocks.Events;
using Todo.BuildingBlocks.Repository;

namespace Todo.Command.Infrastructure.Repository;

public class TodoEventStoreRepository : IEventStoreRepository

{
    private readonly EventStoreClient _eventStoreClient;

    public TodoEventStoreRepository(EventStoreClient eventStoreClient)
    {
        _eventStoreClient = eventStoreClient;
    }

    private readonly string streamName  = "todo-events";

    private string getStreamName(Guid id)
    {
        return $"{streamName}-${id.ToString()}";
    }

    public async Task AppendAsync(BaseEvent @event)
    {            
        var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };

        var data = JsonSerializer.Serialize(@event, options);
        
        var eventData = new EventData(
            Uuid.NewUuid(),
            @event.GetType().Name,
            data: new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes(data))
            );
        
        
        await _eventStoreClient.AppendToStreamAsync(getStreamName(@event.Id), StreamState.Any, new List<EventData> { eventData });
    }

    public async Task<List<BaseEvent>> FindByAggregateId(Guid aggregateId)
    {
        try
        {
            var esEvents =  _eventStoreClient.ReadStreamAsync(Direction.Forwards, getStreamName(aggregateId), StreamPosition.Start);
            var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };

            return (await esEvents.Select(@event => JsonSerializer.Deserialize<BaseEvent>(@event.Event.Data.ToString(), options)).ToListAsync())!;
        }
        catch
        {
            return new List<BaseEvent>();
        }
    }
}
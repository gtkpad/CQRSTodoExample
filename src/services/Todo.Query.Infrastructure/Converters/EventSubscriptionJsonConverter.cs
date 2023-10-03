using System.Text.Json;
using System.Text.Json.Serialization;
using Todo.BuildingBlocks.Events;

namespace Todo.Query.Infrastructure.Converters;

public class EventSubscriptionJsonConverter : JsonConverter<BaseEvent>
{
    private readonly string _eventType;
    
    public EventSubscriptionJsonConverter(string eventType)
    {
        _eventType = eventType;
    }
    
    public override BaseEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!JsonDocument.TryParseValue(ref reader, out var doc))
        {
            throw new JsonException($"Failed to parse {nameof(JsonDocument)}");
        }

        var json = doc.RootElement.GetRawText();

        return _eventType switch
        {
            nameof(TodoCreatedEvent) => JsonSerializer.Deserialize<TodoCreatedEvent>(json, options),
            nameof(TodoMarkedAsDone) => JsonSerializer.Deserialize<TodoMarkedAsDone>(json, options),
            nameof(TodoMarkedAsUndoneEvent) => JsonSerializer.Deserialize<TodoMarkedAsUndoneEvent>(json, options),
            nameof(TodoNameUpdatedEvent) => JsonSerializer.Deserialize<TodoNameUpdatedEvent>(json, options),
            _ => throw new JsonException($"{_eventType} is not supported yet!")
        };
    }

    public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
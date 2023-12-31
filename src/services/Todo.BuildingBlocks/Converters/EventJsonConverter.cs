﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Todo.BuildingBlocks.Events;

namespace Todo.BuildingBlocks.Converters;

public class EventJsonConverter : JsonConverter<BaseEvent>
{
    private readonly string _eventType;
    
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableFrom(typeof(BaseEvent));
    }

    public EventJsonConverter(string eventType)
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
        switch (value.GetType().Name) 
        {
            case nameof(TodoCreatedEvent):
                JsonSerializer.Serialize<TodoCreatedEvent>(writer, (TodoCreatedEvent)value, options); break;
            case nameof(TodoMarkedAsDone):
                JsonSerializer.Serialize<TodoMarkedAsDone>(writer, (TodoMarkedAsDone)value, options); break;
            case nameof(TodoMarkedAsUndoneEvent):
                JsonSerializer.Serialize<TodoMarkedAsUndoneEvent>(writer, (TodoMarkedAsUndoneEvent)value, options); break;
            case nameof(TodoNameUpdatedEvent):
                JsonSerializer.Serialize<TodoNameUpdatedEvent>(writer, (TodoNameUpdatedEvent)value, options); break;
            default: throw new JsonException($"{value.GetType().Name} is not supported yet!");break;
        };
    }
}
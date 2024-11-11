using System.Text.Json.Serialization;

namespace XRFID.Demo.Modules.Mqtt.Events;

/// <summary>
/// Asynchronous Tag Events
/// </summary>
public class TagEvent
{
    /// <summary>
    /// Event type
    /// </summary>
    /// <value>Event type</value>
    [JsonPropertyName("type")]
    public required string Type { get; set; }

    /// <summary>
    /// Event timestamp
    /// </summary>
    /// <value>Event timestamp</value>
    [JsonPropertyName("timestamp")]
    public required DateTimeOffset Timestamp { get; set; }

    /// <summary>
    /// Event Data
    /// </summary>
    /// <value>Event Data</value>
    [JsonPropertyName("data")]
    public required object Data { get; set; }

    /// <summary>
    /// Tag Event Source Address
    /// </summary>
    [JsonPropertyName("source")]
    public string? Source { get; set; }
}

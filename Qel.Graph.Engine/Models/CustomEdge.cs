using System.Text.Json.Serialization;

namespace Qel.Graph.Engine.Models;

public class CustomEdge
{
    [JsonPropertyName("from")]
    public string? From { get; set; }

    [JsonPropertyName("to")]
    public string? To { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }
}

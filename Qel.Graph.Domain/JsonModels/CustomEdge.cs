using System.Text.Json.Serialization;

namespace Qel.Graph.Domain.Models;

public class CustomEdge
{
    [JsonPropertyName("from")]
    public string? from { get; set; }

    [JsonPropertyName("to")]
    public string? to { get; set; }

    [JsonPropertyName("label")]
    public string? label { get; set; }
}

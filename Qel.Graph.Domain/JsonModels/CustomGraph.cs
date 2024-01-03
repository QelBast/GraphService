using System.Text.Json.Serialization;

namespace Qel.Graph.Domain.Models;

public class CustomGraph
{
    [JsonPropertyName("nodes")]
    public required List<CustomNode> nodes { get; set; }
    [JsonPropertyName("edges")]
    public required List<CustomEdge> edges { get; set; }
    [JsonPropertyName("options")]
    public CustomOption? Options { get; set; }
}

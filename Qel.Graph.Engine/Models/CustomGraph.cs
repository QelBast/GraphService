using System.Text.Json.Serialization;

namespace Qel.Graph.Engine.Models;

public class CustomGraph
{
    [JsonPropertyName("Nodes")]
    public List<CustomNode>? Nodes { get; set; }
    [JsonPropertyName("Edges")]
    public List<CustomEdge>? Edges { get; set; }
    [JsonPropertyName("Options")]
    public CustomOption? Options { get; set; }
}

using System.Text.Json.Serialization;

namespace Qel.Graph.Engine.Models;

public class CustomOption
{
    [JsonPropertyName("edges_color")]
    public string? EdgesColor { get; set; }

    [JsonPropertyName("nodes_color")]
    public string? NodesColor { get; set; }

    [JsonPropertyName("directed")]
    public bool IsDirected { get; set; }
}

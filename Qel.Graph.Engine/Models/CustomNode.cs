using System.Text.Json.Serialization;

namespace Qel.Graph.Engine.Models;

public class CustomNode
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("form")]
    public string? Shape { get; set; }

    [JsonPropertyName("label")]
    public string? Label { get; set; }
}

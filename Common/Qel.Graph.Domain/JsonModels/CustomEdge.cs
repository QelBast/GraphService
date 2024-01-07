using System.Text.Json.Serialization;

namespace Qel.Graph.Domain.Models;

/// <summary>
/// Представляет связь между узлами
/// </summary>
public class CustomEdge
{
    /// <summary>
    /// Откуда ведётся связь
    /// </summary>
    [JsonPropertyName("from")]
    public CustomNode? From { get; set; }

    /// <summary>
    /// Куда ведётся связь
    /// </summary>
    [JsonPropertyName("to")]
    public CustomNode? To { get; set; }

    /// <summary>
    /// Название связи
    /// </summary>
    [JsonPropertyName("label")]
    public required string Label { get; set; }

    /// <summary>
    /// Цвет связи
    /// </summary>
    [JsonPropertyName("color")]
    public string? Color { get; set; }
}

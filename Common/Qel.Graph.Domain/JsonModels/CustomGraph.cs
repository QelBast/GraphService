using System.Text.Json.Serialization;

namespace Qel.Graph.Domain.Models;

/// <summary>
/// Представляет схему
/// </summary>
public class CustomGraph
{
    /// <summary>
    /// Коллекция узлов
    /// </summary>
    [JsonPropertyName("nodes")]
    public required List<CustomNode> Nodes { get; set; }

    /// <summary>
    /// Коллекция связей
    /// </summary>
    [JsonPropertyName("edges")]
    public required List<CustomEdge> Edges { get; set; }

    /// <summary>
    /// Отдельные опции схемы и её элементов
    /// </summary>
    [JsonPropertyName("options")]
    public CustomOption? Options { get; set; }
}

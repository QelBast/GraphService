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
    public string? From { get; set; }

    /// <summary>
    /// Куда ведётся связь
    /// </summary>
    [JsonPropertyName("to")]
    public string? To { get; set; }

    /// <summary>
    /// Название связи
    /// </summary>
    [JsonPropertyName("label")]
    public string? Label { get; set; }
}

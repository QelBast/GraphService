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
    public string? from { get; set; }

    /// <summary>
    /// Куда ведётся связь
    /// </summary>
    [JsonPropertyName("to")]
    public string? to { get; set; }

    /// <summary>
    /// Название связи
    /// </summary>
    [JsonPropertyName("label")]
    public string? label { get; set; }
}

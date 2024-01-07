using System.Text.Json.Serialization;

namespace Qel.Graph.Domain.Models;

/// <summary>
/// Представляет узел
/// </summary>
public class CustomNode
{
    /// <summary>
    /// Название узла. Если остутствует <see cref="Label"/> то является также и подписью
    /// </summary>
    [JsonPropertyName("text")]
    public required string Text { get; set; }

    /// <summary>
    /// Фигура узла
    /// </summary>
    [JsonPropertyName("form")]
    public string? Shape { get; set; }

    /// <summary>
    /// Подпись узла
    /// </summary>
    [JsonPropertyName("label")]
    public string? Label { get; set; }

    /// <summary>
    /// Цвет узла
    /// </summary>
    [JsonPropertyName("color")]
    public string? Color { get; set; }
}

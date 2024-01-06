using Qel.Graph.Domain.JsonModels;
using System.Text.Json.Serialization;

namespace Qel.Graph.Domain;

/// <summary>
/// Представляет сохраняемый или загружаемый файл проекта пользователя
/// </summary>
public class File
{
    /// <summary>
    /// Уникальный идентификатор пользовательского проекта
    /// </summary>
    [JsonPropertyName("project_guid")]
    public Guid Id { get; set; }

    /// <summary>
    /// Связи в проекте
    /// </summary>
    [JsonPropertyName("edges")]
    public required List<CustomEdgeAdvanced> Edges { get; set; }

    /// <summary>
    /// Текст, по которому были созданы связи
    /// </summary>
    [JsonPropertyName("text")]
    public required string Text { get; set; }

    /// <summary>
    /// Цвет связей
    /// </summary>
    [JsonPropertyName("edges_color")]
    public string? EdgesColor { get; set; }

    /// <summary>
    /// Цвет узлов
    /// </summary>
    [JsonPropertyName("nodes_color")]
    public string? NodesColor { get; set; }

    /// <summary>
    /// Флаг, отвечающий на вопрос направленными ли отображать связи
    /// </summary>
    [JsonPropertyName("directed")]
    public bool IsDirected { get; set; }

    /// <summary>
    /// Флаг, отвечающий на вопрос удалён проект или нет
    /// </summary>
    [JsonPropertyName("deleted")]
    public bool? IsDeleted { get; set;}
}

using Qel.Graph.Domain.JsonModels;
using Qel.Graph.Domain.Models;
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
    public required List<CustomEdge> Edges { get; set; }

    /// <summary>
    /// Текст, по которому были созданы связи
    /// </summary>
    [JsonPropertyName("text")]
    public required string Text { get; set; }

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

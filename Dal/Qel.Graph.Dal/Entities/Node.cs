using Qel.Graph.Dal.Entities.Common;

namespace Qel.Graph.Dal.Entities;

/// <summary>
/// Представляет сущность узла
/// </summary>
public class Node : BaseEntity
{
    /// <summary>
    /// Возвращает или задаёт цвет узла
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Возвращает или задаёт форму узла
    /// </summary>
    public string? Shape { get; set; }

    /// <summary>
    /// Возвращает или задаёт текст, на основе которого был создан узел
    /// </summary>
    public required string Text { get; set; }

    /// <summary>
    /// Возвращает или задаёт надпись на узле
    /// </summary>
    public string? Label { get; set; }
}

using Qel.Graph.Dal.Entities.Common;

namespace Qel.Graph.Dal.Entities;

/// <summary>
/// Представляет сущность связи
/// </summary>
public class Edge : BaseEntity
{
    /// <summary>
    /// Возвращает или задаёт цвет связи
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Возвращает или задаёт уникальный идентификатор связующего узла
    /// </summary>
    public long? FromNodeId { get; set;}

    /// <summary>
    /// Возвращает или задаёт уникальный идентификатор связуемого узла
    /// </summary>
    public long? ToNodeId { get; set;}

    /// <summary>
    /// 
    /// </summary>
    public Guid? FileId { get; set; }

    /// <summary>
    /// Возвращает или задаёт надпись на связи
    /// </summary>
    public string? Label { get; set;}

    /// <summary>
    /// Возвращает или задаёт сущность связующего узла
    /// </summary>
    public Node? FromNode { get; set; }

    /// <summary>
    /// Возвращает или задаёт сущность связуемого узла
    /// </summary>
    public Node? ToNode { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public File? File { get; set; }
}

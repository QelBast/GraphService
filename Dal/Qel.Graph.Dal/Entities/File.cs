using Qel.Graph.Dal.Entities.Common;

namespace Qel.Graph.Dal.Entities;

/// <summary>
/// Представляет сущность файла, создаваемого пользователем
/// </summary>
public class File : BaseGuidEntity, ICreateAndModifyProperties, ISoftDelete
{
    /// <summary>
    ///  Возвращает или задаёт связи между гранями
    /// </summary>
    public required string Edges { get; set; }

    /// <summary>
    /// Возвращает или задаёт текст, по которому составлялась схема
    /// </summary>
    public required string Text { get; set; }

    /// <summary>
    /// Возвращает или задаёт цвет связей 
    /// </summary>
    public string? EdgesColor { get; set; }

    /// <summary>
    /// Возвращает или задаёт цвет узлов
    /// </summary>
    public string? NodesColor { get; set; }

    /// <summary>
    /// Возвращает или задаёт флаг, обозначающий направленность связей
    /// </summary>
    public bool IsDirected { get; set; }

    ///<inheritdoc/>
    public DateTime? CreationDateTime { get; set; }

    ///<inheritdoc/>
    public DateTime? ModifyDateTime { get; set; }

    ///<inheritdoc/>
    public bool IsDeleted { get; set; }
}

using Qel.Graph.Dal.Entities.Common;

namespace Qel.Graph.Dal.Entities;

/// <summary>
/// Представляет сущность файла, создаваемого пользователем
/// </summary>
public class File : BaseGuidEntity, ICreateAndModifyProperties, ISoftDelete
{
    ///<inheritdoc/>
    public DateTime? CreationDateTime { get; set; }

    ///<inheritdoc/>
    public DateTime? ModifyDateTime { get; set; }

    ///<inheritdoc/>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Возвращает или задаёт текст, по которому составлялась схема
    /// </summary>
    public required string Text { get; set; }

    /// <summary>
    /// Возвращает или задаёт флаг, обозначающий направленность связей
    /// </summary>
    public bool IsDirected { get; set; }

    /// <summary>
    /// Возвращает или задаёт сущность схемы
    /// </summary>
    public GraphEdgesCollection? GraphEdges { get; set; }

    /// <summary>
    /// Возвращает или задаёт коллекцию связей
    /// </summary>
    public List<Edge>? Edges { get; set; }

    #region Foreign Keys
    /// <summary>
    ///  Возвращает или задаёт идентификатор сущности, хранящей информацию о связях и узлах
    /// </summary>
    public long? GraphId { get; set; }
    #endregion
}

using Qel.Graph.Dal.Entities.Common;

namespace Qel.Graph.Dal.Entities;

/// <summary>
/// Представляет сущность схемы основные настройки схемы
/// </summary>
public class GraphEdgesCollection : BaseEntity
{
    /// <summary>
    /// 
    /// </summary>
    public long? EdgeId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Edge? Edge { get; set; }
}

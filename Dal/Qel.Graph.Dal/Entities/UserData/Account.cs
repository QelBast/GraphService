using Qel.Graph.Dal.Entities.Common;

namespace Qel.Graph.Dal.Entities.UserData;

public class Account : BaseEntity, ICreateAndModifyProperties, ISoftDelete
{
    public string? Name { get; set; }

    /// <inheritdoc />
    public DateTime? ModifyDateTime { get; set; }

    /// <inheritdoc />
    public DateTime? CreationDateTime { get; set; }

    /// <inheritdoc />
    public bool IsDeleted { get; set; }
}

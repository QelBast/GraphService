using Qel.Graph.Dal.Entities.Common;

namespace Qel.Graph.Dal.Entities;

public class File : ICreateAndModifyProperties, ISoftDelete
{
    public long Id {  get; set; }

    public string? Path { get; set; }

    public string? Json { get; set; }
    public DateTime? CreationDateTime { get; set; }
    public DateTime? ModifyDateTime { get; set; }
    public bool IsDeleted { get; set; }
}

using Domain.Common;

namespace Domain.Entities;

public class ProjectImage : BaseEntity
{
    public string ImageUrl { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = null!;
}

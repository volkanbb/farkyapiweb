using Domain.Common;

namespace Domain.Entities;

public class TeamMember : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string? Role { get; set; }
    public string? ImageUrl { get; set; }
    public string? FacebookUrl { get; set; }
    public string? TwitterUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public int SortOrder { get; set; }
}

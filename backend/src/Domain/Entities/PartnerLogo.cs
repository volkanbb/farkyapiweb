using Domain.Common;

namespace Domain.Entities;

public class PartnerLogo : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string? WebsiteUrl { get; set; }
    public int SortOrder { get; set; }
}

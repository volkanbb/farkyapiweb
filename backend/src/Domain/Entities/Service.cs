using Domain.Common;

namespace Domain.Entities;

public class Service : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public string? IconName { get; set; }
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }
}

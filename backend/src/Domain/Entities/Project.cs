using Domain.Common;

namespace Domain.Entities;

public class Project : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? ClientName { get; set; }
    public DateTime? ProjectDate { get; set; }
    public string? Location { get; set; }
    
    // SEO
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }

    public Domain.Enums.ProjectStatus Status { get; set; } = Domain.Enums.ProjectStatus.Ongoing;

    public int DisplayOrder { get; set; } = 0;

    public ICollection<ProjectImage> Images { get; set; } = new List<ProjectImage>();
}

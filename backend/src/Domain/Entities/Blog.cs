using Domain.Common;

namespace Domain.Entities;

public class Blog : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Content { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? Author { get; set; }
    
    // SEO
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }
}

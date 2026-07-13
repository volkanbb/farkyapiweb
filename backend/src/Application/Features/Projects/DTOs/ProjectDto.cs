namespace Application.Features.Projects.DTOs;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? CategoryName { get; set; }
    public Guid? CategoryId { get; set; }
    public DateTime? ProjectDate { get; set; }
    public Domain.Enums.ProjectStatus Status { get; set; }
}

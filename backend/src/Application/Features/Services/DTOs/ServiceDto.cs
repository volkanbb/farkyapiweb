using Domain.Entities;

namespace Application.Features.Services.DTOs;

public class ServiceDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public string? IconName { get; set; }
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
}

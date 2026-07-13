using Domain.Common;

namespace Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    
    // Type of category (Blog, Project, Service etc)
    public string Type { get; set; } = string.Empty; 

    // Navigation
    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
    public ICollection<Service> Services { get; set; } = new List<Service>();
}

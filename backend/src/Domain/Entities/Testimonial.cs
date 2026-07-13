using Domain.Common;

namespace Domain.Entities;

public class Testimonial : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string? Title { get; set; } // e.g. CEO of Company
    public string? Company { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int Rating { get; set; } = 5;
    public bool IsApproved { get; set; } = true;
}

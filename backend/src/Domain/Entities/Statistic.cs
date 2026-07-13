using Domain.Common;

namespace Domain.Entities;

public class Statistic : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public int Value { get; set; }
    public string? Suffix { get; set; } // e.g., "+", "%"
    public string? IconName { get; set; }
    public int SortOrder { get; set; }
}

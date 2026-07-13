namespace Application.Features.Projects.DTOs;

public class ProjectDetailDto : ProjectDto
{
    public List<ProjectImageDto> Images { get; set; } = new();
}

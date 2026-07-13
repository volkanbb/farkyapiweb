using Application.Features.Projects.DTOs;
using MediatR;

namespace Application.Features.Projects.Queries.GetProjectBySlug;

public class GetProjectBySlugQuery : IRequest<ProjectDetailDto>
{
    public string Slug { get; set; } = string.Empty;
}

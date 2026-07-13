using Application.Features.Projects.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using MediatR;

namespace Application.Features.Projects.Queries.GetProjectBySlug;

public class GetProjectBySlugQueryHandler : IRequestHandler<GetProjectBySlugQuery, ProjectDetailDto>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IMapper _mapper;

    public GetProjectBySlugQueryHandler(IProjectRepository projectRepository, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _mapper = mapper;
    }

    public async Task<ProjectDetailDto> Handle(GetProjectBySlugQuery request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetProjectBySlugWithImagesAsync(request.Slug);

        if (project == null)
        {
            throw new Exception($"Project with slug {request.Slug} not found.");
        }

        return _mapper.Map<ProjectDetailDto>(project);
    }
}

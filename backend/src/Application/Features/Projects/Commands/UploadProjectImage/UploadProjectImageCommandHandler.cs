using Application.Features.Projects.DTOs;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Projects.Commands.UploadProjectImage;

public class UploadProjectImageCommandHandler : IRequestHandler<UploadProjectImageCommand, ProjectImageDto>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;

    public UploadProjectImageCommandHandler(IProjectRepository projectRepository, IFileService fileService, IMapper mapper)
    {
        _projectRepository = projectRepository;
        _fileService = fileService;
        _mapper = mapper;
    }

    public async Task<ProjectImageDto> Handle(UploadProjectImageCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetProjectByIdWithImagesAsync(request.ProjectId);
        if (project == null)
            throw new Exception("Project not found.");

        var imageUrl = await _fileService.UploadFileAsync(request.File, "projects");

        int nextSortOrder = project.Images.Any() ? project.Images.Max(i => i.SortOrder) + 1 : 0;

        var projectImage = new ProjectImage
        {
            Id = Guid.Empty,
            ProjectId = project.Id,
            ImageUrl = imageUrl,
            SortOrder = nextSortOrder
        };

        project.Images.Add(projectImage);
        await _projectRepository.UpdateAsync(project);

        return _mapper.Map<ProjectImageDto>(projectImage);
    }
}

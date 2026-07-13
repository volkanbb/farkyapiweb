using Application.Features.Projects.DTOs;
using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Application.Interfaces.Services;

namespace Application.Features.Projects.Commands;

public class CreateProjectCommand : IRequest<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; } // Kept for legacy/fallback if needed, though form will use file
    public IFormFile? CoverImageFile { get; set; }
    public List<IFormFile>? GalleryImages { get; set; }
    public Guid? CategoryId { get; set; }
    public Domain.Enums.ProjectStatus Status { get; set; } = Domain.Enums.ProjectStatus.Ongoing;
}

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Guid>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IFileService _fileService;

    public CreateProjectCommandHandler(IProjectRepository projectRepository, IFileService fileService)
    {
        _projectRepository = projectRepository;
        _fileService = fileService;
    }

    public async Task<Guid> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        string? coverImageUrl = request.CoverImageUrl;

        if (request.CoverImageFile != null && request.CoverImageFile.Length > 0)
        {
            coverImageUrl = await _fileService.UploadFileAsync(request.CoverImageFile, "projects");
        }

        var project = new Project
        {
            Title = request.Title,
            Slug = string.IsNullOrEmpty(request.Slug) ? request.Title.ToLower().Replace(" ", "-") : request.Slug,
            ShortDescription = request.ShortDescription,
            Description = request.Description,
            CoverImageUrl = coverImageUrl,
            CategoryId = request.CategoryId,
            Status = request.Status
        };

        if (request.GalleryImages != null && request.GalleryImages.Any())
        {
            int sortOrder = 0;
            foreach (var file in request.GalleryImages)
            {
                if (file.Length > 0)
                {
                    var imageUrl = await _fileService.UploadFileAsync(file, "projects");
                    project.Images.Add(new ProjectImage
                    {
                        ImageUrl = imageUrl,
                        SortOrder = sortOrder++
                    });
                }
            }
        }

        await _projectRepository.AddAsync(project);
        return project.Id;
    }
}

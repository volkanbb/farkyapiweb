using Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Application.Interfaces.Services;

namespace Application.Features.Projects.Commands;

public class UpdateProjectCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public string? CoverImageUrl { get; set; }
    public IFormFile? CoverImageFile { get; set; }
    public Guid? CategoryId { get; set; }
    public Domain.Enums.ProjectStatus Status { get; set; }
}

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, bool>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IFileService _fileService;

    public UpdateProjectCommandHandler(IProjectRepository projectRepository, IFileService fileService)
    {
        _projectRepository = projectRepository;
        _fileService = fileService;
    }

    public async Task<bool> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id);
        if (project == null) return false;

        string? coverImageUrl = request.CoverImageUrl;

        if (request.CoverImageFile != null && request.CoverImageFile.Length > 0)
        {
            coverImageUrl = await _fileService.UploadFileAsync(request.CoverImageFile, "projects");
            
            // Delete old cover image if it exists and is different from the new one.
            if (!string.IsNullOrEmpty(project.CoverImageUrl) && project.CoverImageUrl != coverImageUrl)
            {
                // Optionally call _fileService.DeleteFileAsync(project.CoverImageUrl) here if implemented.
            }
        }

        project.Title = request.Title;
        project.Slug = request.Slug;
        project.ShortDescription = request.ShortDescription;
        project.Description = request.Description;
        project.CoverImageUrl = coverImageUrl ?? project.CoverImageUrl; // Keep old image if no new url/file is provided? Actually, if coverImageUrl is explicitly empty, maybe they want to delete it. But if it's null, we'll assign it.
        project.CategoryId = request.CategoryId;
        project.Status = request.Status;

        await _projectRepository.UpdateAsync(project);
        return true;
    }
}

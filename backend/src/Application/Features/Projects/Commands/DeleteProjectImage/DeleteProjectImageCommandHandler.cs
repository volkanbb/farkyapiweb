using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using MediatR;

namespace Application.Features.Projects.Commands.DeleteProjectImage;

public class DeleteProjectImageCommandHandler : IRequestHandler<DeleteProjectImageCommand, bool>
{
    private readonly IProjectRepository _projectRepository;
    private readonly IFileService _fileService;

    public DeleteProjectImageCommandHandler(IProjectRepository projectRepository, IFileService fileService)
    {
        _projectRepository = projectRepository;
        _fileService = fileService;
    }

    public async Task<bool> Handle(DeleteProjectImageCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetProjectByIdWithImagesAsync(request.ProjectId);
        if (project == null)
            throw new Exception("Project not found.");

        var image = project.Images.FirstOrDefault(i => i.Id == request.ImageId);
        if (image == null)
            throw new Exception("Image not found.");

        // Delete from file system
        _fileService.DeleteFile(image.ImageUrl);

        // Remove from DB
        project.Images.Remove(image);
        await _projectRepository.UpdateAsync(project);

        return true;
    }
}

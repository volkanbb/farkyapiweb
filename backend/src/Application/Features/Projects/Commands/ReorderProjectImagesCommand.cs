using Application.Interfaces.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Application.Features.Projects.Commands;

public class ReorderProjectImagesCommand : IRequest<bool>
{
    public Guid ProjectId { get; set; }
    public List<Guid> ImageIds { get; set; } = new List<Guid>();
}

public class ReorderProjectImagesCommandHandler : IRequestHandler<ReorderProjectImagesCommand, bool>
{
    private readonly IProjectRepository _projectRepository;

    public ReorderProjectImagesCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<bool> Handle(ReorderProjectImagesCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.ProjectId);
        if (project == null) return false;

        for (int i = 0; i < request.ImageIds.Count; i++)
        {
            var imageId = request.ImageIds[i];
            var image = project.Images.FirstOrDefault(x => x.Id == imageId);
            if (image != null)
            {
                image.SortOrder = i;
            }
        }

        await _projectRepository.UpdateAsync(project);
        return true;
    }
}

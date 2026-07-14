using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Projects.Commands;

public class ProjectOrderDto
{
    public Guid Id { get; set; }
    public int DisplayOrder { get; set; }
}

public class ReorderProjectsCommand : IRequest<bool>
{
    public List<ProjectOrderDto> Projects { get; set; } = new();
}

public class ReorderProjectsCommandHandler : IRequestHandler<ReorderProjectsCommand, bool>
{
    private readonly IProjectRepository _projectRepository;

    public ReorderProjectsCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<bool> Handle(ReorderProjectsCommand request, CancellationToken cancellationToken)
    {
        var projects = await _projectRepository.GetAllAsync();

        foreach (var projectOrder in request.Projects)
        {
            var project = projects.FirstOrDefault(p => p.Id == projectOrder.Id);
            if (project != null && project.DisplayOrder != projectOrder.DisplayOrder)
            {
                project.DisplayOrder = projectOrder.DisplayOrder;
                await _projectRepository.UpdateAsync(project);
            }
        }

        return true;
    }
}

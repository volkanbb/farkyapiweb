using Application.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Projects.Commands;

public class DeleteProjectCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, bool>
{
    private readonly IProjectRepository _projectRepository;

    public DeleteProjectCommandHandler(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<bool> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await _projectRepository.GetByIdAsync(request.Id);
        if (project == null) return false;

        await _projectRepository.DeleteAsync(project);
        return true;
    }
}

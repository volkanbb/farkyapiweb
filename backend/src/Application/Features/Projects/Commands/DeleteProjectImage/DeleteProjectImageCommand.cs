using MediatR;

namespace Application.Features.Projects.Commands.DeleteProjectImage;

public class DeleteProjectImageCommand : IRequest<bool>
{
    public Guid ProjectId { get; set; }
    public Guid ImageId { get; set; }
}

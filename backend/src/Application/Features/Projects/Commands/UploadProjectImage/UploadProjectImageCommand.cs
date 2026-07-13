using Application.Features.Projects.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Projects.Commands.UploadProjectImage;

public class UploadProjectImageCommand : IRequest<ProjectImageDto>
{
    public Guid ProjectId { get; set; }
    public IFormFile File { get; set; } = null!;
}

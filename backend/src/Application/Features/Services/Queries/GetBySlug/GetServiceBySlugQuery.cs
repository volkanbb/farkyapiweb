using Application.Features.Services.DTOs;
using MediatR;

namespace Application.Features.Services.Queries.GetBySlug;

public record GetServiceBySlugQuery : IRequest<ServiceDto>
{
    public string Slug { get; set; } = string.Empty;
}

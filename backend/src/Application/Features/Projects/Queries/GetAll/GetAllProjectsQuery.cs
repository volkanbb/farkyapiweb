using Application.Features.Projects.DTOs;
using MediatR;

namespace Application.Features.Projects.Queries.GetAll;

public record GetAllProjectsQuery : IRequest<List<ProjectDto>>;

using Application.Features.Services.DTOs;
using MediatR;

namespace Application.Features.Services.Queries.GetAll;

public record GetAllServicesQuery : IRequest<List<ServiceDto>>;

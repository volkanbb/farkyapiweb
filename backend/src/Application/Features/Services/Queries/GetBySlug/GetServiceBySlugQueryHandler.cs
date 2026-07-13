using Application.Features.Services.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Services.Queries.GetBySlug;

public class GetServiceBySlugQueryHandler : IRequestHandler<GetServiceBySlugQuery, ServiceDto>
{
    private readonly IRepository<Service> _repository;
    private readonly IMapper _mapper;

    public GetServiceBySlugQueryHandler(IRepository<Service> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ServiceDto> Handle(GetServiceBySlugQuery request, CancellationToken cancellationToken)
    {
        var services = await _repository.GetAsync(s => s.Slug == request.Slug);
        var service = services.FirstOrDefault();
        
        if (service == null)
            return null!; // Handle not found properly in a real app, maybe with exceptions

        return _mapper.Map<ServiceDto>(service);
    }
}

using Application.Features.Services.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.Services.Queries.GetAll;

public class GetAllServicesQueryHandler : IRequestHandler<GetAllServicesQuery, List<ServiceDto>>
{
    private readonly IRepository<Service> _repository;
    private readonly IMapper _mapper;

    public GetAllServicesQueryHandler(IRepository<Service> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<ServiceDto>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
    {
        var services = await _repository.GetAllAsync();
        return _mapper.Map<List<ServiceDto>>(services.OrderBy(s => s.SortOrder));
    }
}

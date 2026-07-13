using Application.Features.SiteSettings.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.SiteSettings.Queries;

public record GetSiteSettingsQuery : IRequest<SiteSettingDto>;

public class GetSiteSettingsQueryHandler : IRequestHandler<GetSiteSettingsQuery, SiteSettingDto>
{
    private readonly IRepository<SiteSetting> _repository;
    private readonly IMapper _mapper;

    public GetSiteSettingsQueryHandler(IRepository<SiteSetting> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SiteSettingDto> Handle(GetSiteSettingsQuery request, CancellationToken cancellationToken)
    {
        var settingsList = await _repository.GetAllAsync();
        var settings = settingsList.FirstOrDefault();
        
        if (settings == null)
        {
            settings = new SiteSetting();
            await _repository.AddAsync(settings);
        }

        return _mapper.Map<SiteSettingDto>(settings);
    }
}

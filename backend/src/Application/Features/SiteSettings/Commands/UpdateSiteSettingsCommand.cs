using Application.Features.SiteSettings.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Features.SiteSettings.Commands;

public class UpdateSiteSettingsCommand : IRequest<SiteSettingDto>
{
    public string? SiteName { get; set; }
    public string? LogoUrl { get; set; }
    public string? DarkLogoUrl { get; set; }
    public string? FaviconUrl { get; set; }
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    
    public string? Phone { get; set; }
    public string? Whatsapp { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? GoogleMapsUrl { get; set; }
    
    public string? FacebookUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? YoutubeUrl { get; set; }
    public string? TwitterUrl { get; set; }

    public string? HeroTitle { get; set; }
    public string? HeroSubtitle { get; set; }
    public string? HeroDescription { get; set; }
    public string? HeroButtonText { get; set; }
    public string? HeroButtonUrl { get; set; }
    public string? HeroImageUrl { get; set; }

    public string? AboutTitle { get; set; }
    public string? AboutDescription { get; set; }
    public string? AboutImageUrl { get; set; }
}

public class UpdateSiteSettingsCommandHandler : IRequestHandler<UpdateSiteSettingsCommand, SiteSettingDto>
{
    private readonly IRepository<SiteSetting> _repository;
    private readonly IMapper _mapper;

    public UpdateSiteSettingsCommandHandler(IRepository<SiteSetting> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<SiteSettingDto> Handle(UpdateSiteSettingsCommand request, CancellationToken cancellationToken)
    {
        var settingsList = await _repository.GetAllAsync();
        var settings = settingsList.FirstOrDefault();
        
        if (settings == null)
        {
            settings = new SiteSetting();
            _mapper.Map(request, settings);
            await _repository.AddAsync(settings);
        }
        else
        {
            _mapper.Map(request, settings);
            await _repository.UpdateAsync(settings);
        }

        return _mapper.Map<SiteSettingDto>(settings);
    }
}

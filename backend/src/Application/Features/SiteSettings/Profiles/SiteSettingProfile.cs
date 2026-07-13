using Application.Features.SiteSettings.Commands;
using Application.Features.SiteSettings.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.SiteSettings.Profiles;

public class SiteSettingProfile : Profile
{
    public SiteSettingProfile()
    {
        CreateMap<SiteSetting, SiteSettingDto>().ReverseMap();
        CreateMap<UpdateSiteSettingsCommand, SiteSetting>();
    }
}

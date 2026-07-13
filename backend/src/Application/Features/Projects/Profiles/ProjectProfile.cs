using Application.Features.Projects.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Projects.Profiles;

public class ProjectProfile : Profile
{
    public ProjectProfile()
    {
        CreateMap<Project, ProjectDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));
        
        CreateMap<ProjectImage, ProjectImageDto>();
        CreateMap<Project, ProjectDetailDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));
    }
}

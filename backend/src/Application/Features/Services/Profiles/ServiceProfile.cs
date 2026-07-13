using Application.Features.Services.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Services.Profiles;

public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<Service, ServiceDto>();
        CreateMap<Application.Features.Services.Commands.Create.CreateServiceCommand, Service>();
        CreateMap<Application.Features.Services.Commands.Update.UpdateServiceCommand, Service>();
    }
}

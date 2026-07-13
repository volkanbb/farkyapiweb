using Application.Features.Services.DTOs;
using Application.Interfaces.Repositories;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Text.RegularExpressions;

namespace Application.Features.Services.Commands.Update;

public class UpdateServiceCommand : IRequest<ServiceDto>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }
    public string? IconName { get; set; }
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
}

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, ServiceDto>
{
    private readonly IRepository<Service> _repository;
    private readonly IMapper _mapper;

    public UpdateServiceCommandHandler(IRepository<Service> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ServiceDto> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _repository.GetByIdAsync(request.Id);
        if (service == null)
            throw new Exception("Service not found");

        _mapper.Map(request, service);
        service.Slug = GenerateSlug(request.Title);

        await _repository.UpdateAsync(service);
        return _mapper.Map<ServiceDto>(service);
    }

    private string GenerateSlug(string phrase)
    {
        if (string.IsNullOrEmpty(phrase)) return "";
        string str = phrase.ToLower();
        str = str.Replace("ğ", "g").Replace("ü", "u").Replace("ş", "s").Replace("ı", "i").Replace("ö", "o").Replace("ç", "c");
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        str = Regex.Replace(str, @"\s+", " ").Trim();
        str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
        str = Regex.Replace(str, @"\s", "-");
        return str;
    }
}

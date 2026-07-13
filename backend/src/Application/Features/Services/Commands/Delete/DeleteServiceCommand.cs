using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Features.Services.Commands.Delete;

public class DeleteServiceCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, bool>
{
    private readonly IRepository<Service> _repository;

    public DeleteServiceCommandHandler(IRepository<Service> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _repository.GetByIdAsync(request.Id);
        if (service == null) return false;

        await _repository.DeleteAsync(service);
        return true;
    }
}

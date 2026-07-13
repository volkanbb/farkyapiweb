using Domain.Entities;

namespace Application.Interfaces.Repositories;

public interface IProjectRepository : IRepository<Project>
{
    Task<IReadOnlyList<Project>> GetProjectsWithCategoryAsync();
    Task<Project?> GetProjectBySlugWithImagesAsync(string slug);
    Task<Project?> GetProjectByIdWithImagesAsync(Guid id);
}

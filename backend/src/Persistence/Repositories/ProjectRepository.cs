using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories;

public class ProjectRepository : Repository<Project>, IProjectRepository
{
    public ProjectRepository(FarkYapiDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<IReadOnlyList<Project>> GetProjectsWithCategoryAsync()
    {
        return await _dbContext.Projects
            .Include(p => p.Category)
            .Where(p => !p.IsDeleted)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Project?> GetProjectBySlugWithImagesAsync(string slug)
    {
        return await _dbContext.Projects
            .Include(p => p.Category)
            .Include(p => p.Images.OrderBy(i => i.SortOrder))
            .Where(p => !p.IsDeleted && p.Slug == slug)
            .FirstOrDefaultAsync();
    }

    public async Task<Project?> GetProjectByIdWithImagesAsync(Guid id)
    {
        return await _dbContext.Projects
            .Include(p => p.Images.OrderBy(i => i.SortOrder))
            .Where(p => !p.IsDeleted && p.Id == id)
            .FirstOrDefaultAsync();
    }
}

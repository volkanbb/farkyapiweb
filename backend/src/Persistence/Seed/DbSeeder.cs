using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Seed;

public static class DbSeeder
{
    public static async Task SeedAsync(FarkYapiDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        // 1. Roles
        string[] roles = { "Admin", "Editor", "User" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = role, Description = $"{role} Role" });
            }
        }

        // 2. Admin User
        var adminEmail = "admin@farkyapi.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "System",
                LastName = "Admin",
                EmailConfirmed = true,
                IsActive = true
            };
            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // 3. Site Settings
        if (!await context.SiteSettings.AnyAsync())
        {
            await context.SiteSettings.AddAsync(new SiteSetting
            {
                SiteName = "Fark Yapı Web",
                Phone = "+90 0 532 500 91 04",
                Email = "faruk.cengel@hotmail.com",
                Address = "Süleymaniye, 16400 İnegöl/Bursa",
                InstagramUrl = "https://www.instagram.com/fark_yapi16/",
                HeroTitle = "Hayallerinizi İnşa Ediyoruz",
                HeroSubtitle = "Mimarlık & Müteahhitlik",
                HeroDescription = "En iyi mimari çözümleri sunuyoruz.",
                HeroButtonText = "Projelerimiz",
                HeroButtonUrl = "/projelerimiz"
            });
            await context.SaveChangesAsync();
        }

        // 4. Demo Categories
        if (!await context.Categories.AnyAsync())
        {
            var servicesCategory = new Category { Name = "Services", Slug = "services", Type = "Service" };
            var projectsCategory = new Category { Name = "Commercial", Slug = "commercial", Type = "Project" };
            
            await context.Categories.AddRangeAsync(servicesCategory, projectsCategory);
            await context.SaveChangesAsync();
        }

        // 5. Demo Services
        if (!await context.Services.AnyAsync())
        {
            var cat = await context.Categories.FirstOrDefaultAsync(c => c.Type == "Service");
            await context.Services.AddAsync(new Service
            {
                Title = "Architectural Design",
                Slug = "architectural-design",
                ShortDescription = "Modern and functional designs.",
                CategoryId = cat?.Id
            });
            await context.SaveChangesAsync();
        }

        // 6. Demo Projects
        if (!await context.Projects.AnyAsync())
        {
            var cat = await context.Categories.FirstOrDefaultAsync(c => c.Type == "Project");
            await context.Projects.AddAsync(new Project
            {
                Title = "Skyline Office",
                Slug = "skyline-office",
                ShortDescription = "A modern office building in the city center.",
                CategoryId = cat?.Id,
                ProjectDate = DateTime.UtcNow.AddMonths(-2),
                ClientName = "Skyline Corp"
            });
            await context.SaveChangesAsync();
        }
    }
}

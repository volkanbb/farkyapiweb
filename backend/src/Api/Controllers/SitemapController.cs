using Application.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Xml;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class SitemapController : ControllerBase
{
    private readonly IRepository<Project> _projectRepository;
    private readonly string _baseUrl = "https://farkyapi.com";

    public SitemapController(IRepository<Project> projectRepository)
    {
        _projectRepository = projectRepository;
    }

    [HttpGet("sitemap.xml")]
    [Produces("application/xml")]
    public async Task<IActionResult> GetSitemap()
    {
        var projects = await _projectRepository.GetAsync(p => !p.IsDeleted);

        var sb = new StringBuilder();
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        sb.AppendLine("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">");

        // Add static pages
        AddUrl(sb, "/", "1.0", "daily");
        AddUrl(sb, "/hakkimizda", "0.8", "monthly");
        AddUrl(sb, "/hizmetlerimiz", "0.8", "monthly");
        AddUrl(sb, "/projelerimiz", "0.9", "weekly");
        AddUrl(sb, "/iletisim", "0.7", "yearly");

        // Add dynamic projects
        foreach (var project in projects)
        {
            var lastMod = project.UpdatedAt?.ToString("yyyy-MM-dd") ?? project.CreatedAt.ToString("yyyy-MM-dd");
            AddUrl(sb, $"/projelerimiz/{project.Slug}", "0.8", "weekly", lastMod);
        }

        sb.AppendLine("</urlset>");

        return Content(sb.ToString(), "application/xml", Encoding.UTF8);
    }

    private void AddUrl(StringBuilder sb, string path, string priority, string changeFreq, string? lastMod = null)
    {
        sb.AppendLine("  <url>");
        sb.AppendLine($"    <loc>{_baseUrl}{path}</loc>");
        
        if (!string.IsNullOrEmpty(lastMod))
        {
            sb.AppendLine($"    <lastmod>{lastMod}</lastmod>");
        }
        else
        {
            sb.AppendLine($"    <lastmod>{DateTime.UtcNow.ToString("yyyy-MM-dd")}</lastmod>");
        }

        sb.AppendLine($"    <changefreq>{changeFreq}</changefreq>");
        sb.AppendLine($"    <priority>{priority}</priority>");
        sb.AppendLine("  </url>");
    }
}

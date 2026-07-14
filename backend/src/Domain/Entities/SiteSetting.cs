using Domain.Common;

namespace Domain.Entities;

public class SiteSetting : BaseEntity
{
    // A key-value approach or flat approach
    // We will use flat approach for strongly typed settings
    
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
    
    // Social Links
    public string? FacebookUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? YoutubeUrl { get; set; }
    public string? TwitterUrl { get; set; }

    // Hero Section
    public string? HeroTitle { get; set; }
    public string? HeroSubtitle { get; set; }
    public string? HeroDescription { get; set; }
    public string? HeroButtonText { get; set; }
    public string? HeroButtonUrl { get; set; }
    public string? HeroImageUrl { get; set; }

    // About Section
    public string? AboutTitle { get; set; }
    public string? AboutDescription { get; set; }
    public string? AboutImageUrl { get; set; }

    // Activity Region Section
    public string? ActivityRegionText { get; set; }
}

import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';

export interface SiteSetting {
  id?: string;
  siteName?: string;
  logoUrl?: string;
  darkLogoUrl?: string;
  faviconUrl?: string;
  metaTitle?: string;
  metaDescription?: string;
  
  phone?: string;
  whatsapp?: string;
  email?: string;
  address?: string;
  googleMapsUrl?: string;
  
  facebookUrl?: string;
  instagramUrl?: string;
  linkedInUrl?: string;
  youtubeUrl?: string;
  twitterUrl?: string;

  heroTitle?: string;
  heroSubtitle?: string;
  heroDescription?: string;
  heroButtonText?: string;
  heroButtonUrl?: string;
  heroImageUrl?: string;

  aboutTitle?: string;
  aboutDescription?: string;
  aboutImageUrl?: string;

  activityRegionText?: string;
}

@Injectable({
  providedIn: 'root'
})
export class SiteSettingsService {
  constructor(private apiService: ApiService) {}

  getSettings(): Observable<SiteSetting> {
    return this.apiService.get<SiteSetting>('SiteSettings');
  }

  updateSettings(settings: SiteSetting): Observable<SiteSetting> {
    return this.apiService.put<SiteSetting>('SiteSettings', settings);
  }
}

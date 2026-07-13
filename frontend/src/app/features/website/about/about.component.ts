import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SiteSettingsService, SiteSetting } from '../../../core/services/site-settings.service';
import { SeoService } from '../../../core/services/seo.service';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './about.component.html',
  styleUrl: './about.component.scss'
})
export class AboutComponent implements OnInit {
  siteSettingsService = inject(SiteSettingsService);
  seo = inject(SeoService);
  
  settings: SiteSetting | null = null;

  ngOnInit() {
    this.seo.setSeoData('Hakkımızda', 'Fark Yapı - Kalite, güven ve müşteri memnuniyeti ile modern yaşam alanları.');
    
    this.siteSettingsService.getSettings().subscribe({
      next: (data) => {
        this.settings = data;
      },
      error: (err) => {
        console.error('Failed to load settings in about page', err);
      }
    });
  }
}

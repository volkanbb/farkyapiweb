import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SiteSettingsService, SiteSetting } from '../../../core/services/site-settings.service';
import { SeoService } from '../../../core/services/seo.service';

@Component({
  selector: 'app-contact',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.scss'
})
export class ContactComponent implements OnInit {
  settings: SiteSetting | null = null;
  private seoService = inject(SeoService);

  constructor(private siteSettingsService: SiteSettingsService) {}

  ngOnInit(): void {
    this.seoService.setSeoData('İletişim', 'Fark Yapı Mimarlık ile iletişime geçin.');
    this.siteSettingsService.getSettings().subscribe({
      next: (data) => {
        this.settings = data;
      },
      error: (err) => {
        console.error('Failed to load settings in contact page', err);
      }
    });
  }

  get whatsappLink(): string {
    if (!this.settings?.phone) return '';
    const cleanNumber = this.settings.phone.replace(/\D/g, '');
    return `https://wa.me/${cleanNumber}`;
  }
}

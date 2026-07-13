import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SiteSettingsService, SiteSetting } from '../../../../core/services/site-settings.service';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './footer.component.html',
  styleUrl: './footer.component.scss'
})
export class FooterComponent implements OnInit {
  settings: SiteSetting | null = null;

  constructor(private siteSettingsService: SiteSettingsService) {}

  ngOnInit(): void {
    this.siteSettingsService.getSettings().subscribe({
      next: (data) => {
        this.settings = data;
      },
      error: (err) => {
        console.error('Failed to load settings in footer', err);
      }
    });
  }

  get whatsappLink(): string {
    if (!this.settings?.phone) return '';
    // Removes all non-numeric characters
    const cleanNumber = this.settings.phone.replace(/\D/g, '');
    // If it starts with 0 after country code, WhatsApp might still route it, but standard is without leading 0.
    // e.g. "0532" -> "90532" (assuming TR). If the user enters +90 532, \D removes + and spaces.
    return `https://wa.me/${cleanNumber}`;
  }
}

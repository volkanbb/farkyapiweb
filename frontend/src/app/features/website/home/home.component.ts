import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SeoService } from '../../../core/services/seo.service';
import { SiteSettingsService, SiteSetting } from '../../../core/services/site-settings.service';
import { ServiceService } from '../../../core/services/service.service';
import { Service } from '../../../core/models/service.model';
import { ProjectService } from '../../../core/services/project.service';
import { Project } from '../../../core/models/project.model';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {
  seo = inject(SeoService);
  siteSettingsService = inject(SiteSettingsService);
  serviceService = inject(ServiceService);
  projectService = inject(ProjectService);
  
  settings: SiteSetting | null = null;
  services: Service[] = [];
  projects: Project[] = [];

  ngOnInit() {
    this.seo.setSeoData(
      'Ana Sayfa',
      'Fark Yapı Mimarlık, modern mimari, lüks iç mekanlar ve sürdürülebilir ticari yapılarla geleceği şekillendiren ödüllü bir mimarlık firmasıdır.'
    );

    this.siteSettingsService.getSettings().subscribe({
      next: (data) => {
        this.settings = data;
      },
      error: (err) => {
        console.error('Failed to load settings in home page', err);
      }
    });

    this.serviceService.getServices().subscribe({
      next: (data) => {
        this.services = data;
      },
      error: (err) => {
        console.error('Failed to load services in home page', err);
      }
    });

    this.projectService.getProjects().subscribe({
      next: (data) => {
        this.projects = data;
      },
      error: (err) => {
        console.error('Failed to load projects in home page', err);
      }
    });
  }

  get whatsappLink(): string {
    if (!this.settings?.phone) return '';
    const cleanNumber = this.settings.phone.replace(/\D/g, '');
    return `https://wa.me/${cleanNumber}`;
  }

  getStatusText(status: number): string {
    switch(status) {
      case 0: return 'Devam Ediyor';
      case 1: return 'Tamamlandı';
      case 2: return 'Gelecek Proje';
      default: return '';
    }
  }

  getStatusClass(status: number): string {
    switch(status) {
      case 0: return 'status-ongoing';
      case 1: return 'status-completed';
      case 2: return 'status-upcoming';
      default: return '';
    }
  }
}

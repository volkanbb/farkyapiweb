import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ServiceService } from '../../../core/services/service.service';
import { Service } from '../../../core/models/service.model';
import { SeoService } from '../../../core/services/seo.service';

@Component({
  selector: 'app-services',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './services.component.html',
  styleUrl: './services.component.scss'
})
export class ServicesComponent implements OnInit {
  serviceService = inject(ServiceService);
  seo = inject(SeoService);

  services: Service[] = [];

  ngOnInit() {
    this.seo.setSeoData('Hizmetlerimiz', 'Fark Yapı hizmetleri.');
    
    this.serviceService.getServices().subscribe({
      next: (data) => {
        this.services = data;
      },
      error: (err) => {
        console.error('Failed to load services', err);
      }
    });
  }
}

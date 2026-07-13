import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ProjectService } from '../../../core/services/project.service';
import { ServiceService } from '../../../core/services/service.service';
import { Project } from '../../../core/models/project.model';
import { Service } from '../../../core/models/service.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  private projectService = inject(ProjectService);
  private serviceService = inject(ServiceService);

  totalProjects: number = 0;
  totalServices: number = 0;
  recentProjects: Project[] = [];
  loading = true;

  ngOnInit() {
    this.projectService.getProjects().subscribe({
      next: (projects) => {
        this.totalProjects = projects.length;
        // Get the latest 5 projects based on ID (assuming newer has higher ID, or we can just reverse)
        this.recentProjects = projects.slice().reverse().slice(0, 5);
        this.checkLoading();
      },
      error: (err) => {
        console.error('Error fetching projects', err);
        this.checkLoading();
      }
    });

    this.serviceService.getServices().subscribe({
      next: (services) => {
        this.totalServices = services.length;
        this.checkLoading();
      },
      error: (err) => {
        console.error('Error fetching services', err);
        this.checkLoading();
      }
    });
  }

  checkLoading() {
    // Basic loading check, could be better but good enough for this simple dashboard
    if (this.totalProjects !== undefined && this.totalServices !== undefined) {
      this.loading = false;
    }
  }

  getStatusText(status: number): string {
    switch(status) {
      case 0: return 'Devam Ediyor';
      case 1: return 'Tamamlandı';
      case 2: return 'Gelecek Proje';
      default: return 'Bilinmiyor';
    }
  }

  getStatusBadgeClass(status: number): string {
    switch(status) {
      case 0: return 'badge-warning';
      case 1: return 'badge-success';
      case 2: return 'badge-info';
      default: return 'badge-secondary';
    }
  }
}

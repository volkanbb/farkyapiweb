import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ProjectService } from '../../../core/services/project.service';
import { Project } from '../../../core/models/project.model';

import { SeoService } from '../../../core/services/seo.service';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './projects.component.html',
  styleUrl: './projects.component.scss'
})
export class ProjectsComponent implements OnInit {
  private projectService = inject(ProjectService);
  private seoService = inject(SeoService);
  projects: Project[] = [];
  filteredProjects: Project[] = [];
  paginatedProjects: Project[] = [];
  loading = true;
  activeFilter: number | null = null; // null means "Tümü"
  
  currentPage = 1;
  pageSize = 3;
  totalPages = 1;
  pageNumbers: number[] = [];

  ngOnInit() {
    this.seoService.setSeoData('Projelerimiz', 'Fark Yapı Mimarlık projeleri ve çalışmaları.');
    this.projectService.getProjects().subscribe({
      next: (data) => {
        this.projects = data;
        this.filterProjects(null);
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load projects', err);
        this.loading = false;
      }
    });
  }

  filterProjects(status: number | null) {
    this.activeFilter = status;
    if (status === null) {
      this.filteredProjects = this.projects;
    } else {
      this.filteredProjects = this.projects.filter(p => p.status === status);
    }
    
    this.currentPage = 1;
    this.updatePagination();
  }
  
  updatePagination() {
    this.totalPages = Math.ceil(this.filteredProjects.length / this.pageSize) || 1;
    this.pageNumbers = Array.from({ length: this.totalPages }, (_, i) => i + 1);
    
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.paginatedProjects = this.filteredProjects.slice(startIndex, endIndex);
  }
  
  changePage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.updatePagination();
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
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

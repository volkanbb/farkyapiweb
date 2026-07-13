import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ApiService, ProjectDto } from '../../../core/services/api.service';

@Component({
  selector: 'app-project-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './project-detail.component.html',
  styleUrl: './project-detail.component.scss'
})
export class ProjectDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private api = inject(ApiService);

  project: ProjectDto | null = null;
  loading = true;
  
  allImages: string[] = [];
  currentImageIndex = 0;

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const slug = params.get('slug');
      if (slug) {
        this.loadProject(slug);
      }
    });
  }

  loadProject(slug: string) {
    this.loading = true;
    this.api.getProjectBySlug(slug).subscribe({
      next: (data: ProjectDto) => {
        this.project = data;
        
        // Populate slider images
        this.allImages = [];
        if (data.coverImageUrl) {
          this.allImages.push(data.coverImageUrl);
        }
        if (data.images && data.images.length > 0) {
          data.images.forEach(img => {
            if (img.imageUrl) {
              this.allImages.push(img.imageUrl);
            }
          });
        }
        
        this.loading = false;
      },
      error: (err: any) => {
        console.error('Failed to load project details', err);
        this.loading = false;
      }
    });
  }

  nextImage() {
    if (this.allImages.length > 0) {
      this.currentImageIndex = (this.currentImageIndex + 1) % this.allImages.length;
    }
  }

  prevImage() {
    if (this.allImages.length > 0) {
      this.currentImageIndex = (this.currentImageIndex - 1 + this.allImages.length) % this.allImages.length;
    }
  }
  
  setCurrentImage(index: number) {
    this.currentImageIndex = index;
  }
}

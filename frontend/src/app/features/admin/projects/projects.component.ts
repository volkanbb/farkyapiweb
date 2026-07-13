import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { ApiService, ProjectDto, ProjectImageDto } from '../../../core/services/api.service';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, DragDropModule],
  templateUrl: './projects.component.html',
  styleUrl: './projects.component.scss'
})
export class ProjectsComponent implements OnInit {
  api = inject(ApiService);
  fb = inject(FormBuilder);
  
  projects: ProjectDto[] = [];
  loading = true;
  
  showForm = false;
  isEditing = false;
  projectForm: FormGroup;
  currentProjectId: string | null = null;
  saving = false;

  constructor() {
    this.projectForm = this.fb.group({
      title: ['', Validators.required],
      slug: ['', Validators.required],
      shortDescription: [''],
      description: [''],
      coverImageUrl: [''],
      status: [0, Validators.required]
    });
  }

  ngOnInit() {
    this.loadProjects();
  }

  loadProjects() {
    this.loading = true;
    this.api.getProjects().subscribe({
      next: (data) => {
        this.projects = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load projects', err);
        this.loading = false;
      }
    });
  }

  coverImageFile: File | null = null;
  galleryFiles: File[] = [];
  projectImages: ProjectImageDto[] = [];
  uploadingImage = false;

  openAddForm() {
    this.isEditing = false;
    this.currentProjectId = null;
    this.coverImageFile = null;
    this.galleryFiles = [];
    this.projectImages = [];
    this.projectForm.reset();
    this.showForm = true;
  }

  openEditForm(project: ProjectDto) {
    this.isEditing = true;
    this.currentProjectId = project.id;
    this.coverImageFile = null;
    this.galleryFiles = [];
    this.projectForm.patchValue({
      title: project.title,
      slug: project.slug,
      shortDescription: project.shortDescription,
      description: project.description,
      coverImageUrl: project.coverImageUrl,
      status: project.status || 0
    });
    this.loadProjectImages(project.slug);
    this.showForm = true;
  }

  cancelForm() {
    this.showForm = false;
  }

  onCoverImageSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.coverImageFile = file;
    }
  }

  onGalleryFilesSelected(event: any) {
    if (this.isEditing && this.currentProjectId) {
      // In edit mode, upload instantly
      const files = Array.from(event.target.files) as File[];
      if (files.length > 0) {
        // Upload one by one or we only have a single file endpoint for now
        // Loop and upload
        this.uploadingImage = true;
        let uploadedCount = 0;
        files.forEach(file => {
          this.api.uploadProjectImage(this.currentProjectId!, file).subscribe({
            next: () => {
              uploadedCount++;
              if (uploadedCount === files.length) {
                this.uploadingImage = false;
                this.loadProjectImagesBySlug(); // refresh images
              }
            },
            error: (err) => {
              console.error('Image upload failed', err);
              this.uploadingImage = false;
            }
          });
        });
      }
    } else {
      // In create mode, just store them
      if (event.target.files && event.target.files.length > 0) {
        this.galleryFiles = Array.from(event.target.files);
      }
    }
  }

  loadProjectImages(slug: string) {
    this.api.getProjectBySlug(slug).subscribe({
      next: (data) => {
        // Sort by sortOrder if present
        this.projectImages = (data.images || []).sort((a, b) => (a.sortOrder || 0) - (b.sortOrder || 0));
      },
      error: (err) => console.error('Failed to load project images', err)
    });
  }

  loadProjectImagesBySlug() {
    const slug = this.projectForm.value.slug;
    if (slug) {
      this.loadProjectImages(slug);
    }
  }

  saveProject() {
    if (this.projectForm.invalid) return;
    
    this.saving = true;
    const val = this.projectForm.value;
    const formData = new FormData();
    
    formData.append('title', val.title);
    formData.append('slug', val.slug);
    if (val.shortDescription) formData.append('shortDescription', val.shortDescription);
    if (val.description) formData.append('description', val.description);
    if (val.coverImageUrl) formData.append('coverImageUrl', val.coverImageUrl);
    if (this.coverImageFile) formData.append('coverImageFile', this.coverImageFile);
    formData.append('status', val.status.toString());

    if (this.isEditing && this.currentProjectId) {
      formData.append('id', this.currentProjectId);
      this.api.put(`Projects/${this.currentProjectId}`, formData).subscribe({
        next: () => {
          this.loadProjects();
          this.showForm = false;
          this.saving = false;
          this.coverImageFile = null;
        },
        error: (err) => {
          console.error('Update failed', err);
          this.saving = false;
        }
      });
    } else {
      // Create mode
      if (this.galleryFiles.length > 0) {
        this.galleryFiles.forEach(file => {
          formData.append('GalleryImages', file); // Needs to match backend property name case-insensitively
        });
      }

      this.api.post('Projects', formData).subscribe({
        next: () => {
          this.loadProjects();
          this.showForm = false;
          this.saving = false;
          this.coverImageFile = null;
          this.galleryFiles = [];
        },
        error: (err) => {
          console.error('Create failed', err);
          this.saving = false;
        }
      });
    }
  }

  deleteProject(id: string) {
    if (confirm('Bu projeyi silmek istediğinize emin misiniz?')) {
      this.api.delete(`Projects/${id}`).subscribe({
        next: () => this.loadProjects(),
        error: (err) => console.error('Delete failed', err)
      });
    }
  }

  // --- Image Management for Edit Mode ---

  deleteImage(imageId: string) {
    if (!this.currentProjectId) return;
    if (confirm('Bu görseli silmek istediğinize emin misiniz?')) {
      this.api.deleteProjectImage(this.currentProjectId, imageId).subscribe({
        next: () => {
          this.loadProjectImagesBySlug();
        },
        error: (err) => console.error('Delete image failed', err)
      });
    }
  }

  drop(event: any) {
    if (event.previousIndex === event.currentIndex) {
      return; // No change
    }
    
    // We import moveItemInArray manually or dynamically. Wait, we should import it properly.
    // Instead of moveItemInArray, we can just splice for now to avoid import issues if DragDropModule isn't perfectly set up.
    const movedItem = this.projectImages[event.previousIndex];
    this.projectImages.splice(event.previousIndex, 1);
    this.projectImages.splice(event.currentIndex, 0, movedItem);

    // Save the new order
    if (this.currentProjectId) {
      const imageIds = this.projectImages.map(img => img.id);
      this.api.reorderProjectImages(this.currentProjectId, imageIds).subscribe({
        next: () => console.log('Reordered successfully'),
        error: (err) => console.error('Reorder failed', err)
      });
    }
  }
}

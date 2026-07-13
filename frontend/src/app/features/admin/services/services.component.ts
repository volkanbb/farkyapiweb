import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ServiceService } from '../../../core/services/service.service';
import { Service } from '../../../core/models/service.model';

@Component({
  selector: 'app-admin-services',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './services.component.html',
  styleUrl: './services.component.scss'
})
export class ServicesComponent implements OnInit {
  serviceService = inject(ServiceService);
  fb = inject(FormBuilder);
  
  services: Service[] = [];
  loading = true;
  
  showForm = false;
  isEditing = false;
  serviceForm: FormGroup;
  currentServiceId: string | null = null;
  saving = false;

  constructor() {
    this.serviceForm = this.fb.group({
      title: ['', Validators.required],
      shortDescription: [''],
      description: [''],
      iconName: [''],
      imageUrl: [''],
      sortOrder: [0]
    });
  }

  ngOnInit() {
    this.loadServices();
  }

  loadServices() {
    this.loading = true;
    this.serviceService.getServices().subscribe({
      next: (data: Service[]) => {
        this.services = data;
        this.loading = false;
      },
      error: (err: any) => {
        console.error('Failed to load services', err);
        this.loading = false;
      }
    });
  }

  openAddForm() {
    this.isEditing = false;
    this.currentServiceId = null;
    this.serviceForm.reset();
    this.showForm = true;
  }

  openEditForm(service: Service) {
    this.isEditing = true;
    this.currentServiceId = service.id || null;
    this.serviceForm.patchValue({
      title: service.title,
      shortDescription: service.shortDescription,
      description: service.description,
      iconName: service.iconName,
      imageUrl: service.imageUrl,
      sortOrder: service.sortOrder
    });
    this.showForm = true;
  }

  cancelForm() {
    this.showForm = false;
  }

  saveService() {
    if (this.serviceForm.invalid) return;
    
    this.saving = true;
    const val = this.serviceForm.value;

    if (this.isEditing && this.currentServiceId) {
      this.serviceService.updateService(this.currentServiceId, val).subscribe({
        next: () => {
          this.loadServices();
          this.showForm = false;
          this.saving = false;
        },
        error: (err: any) => {
          console.error('Update failed', err);
          this.saving = false;
        }
      });
    } else {
      this.serviceService.createService(val).subscribe({
        next: () => {
          this.loadServices();
          this.showForm = false;
          this.saving = false;
        },
        error: (err: any) => {
          console.error('Create failed', err);
          this.saving = false;
        }
      });
    }
  }

  deleteService(id: string) {
    if (confirm('Bu hizmeti silmek istediğinize emin misiniz?')) {
      this.serviceService.deleteService(id).subscribe({
        next: () => this.loadServices(),
        error: (err: any) => console.error('Delete failed', err)
      });
    }
  }
}

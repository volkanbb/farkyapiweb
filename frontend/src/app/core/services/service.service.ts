import { Injectable, inject } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { Service } from '../models/service.model';

@Injectable({
  providedIn: 'root'
})
export class ServiceService {
  private api = inject(ApiService);
  private endpoint = 'services';

  getServices(): Observable<Service[]> {
    return this.api.get<Service[]>(this.endpoint);
  }

  getServiceBySlug(slug: string): Observable<Service> {
    return this.api.get<Service>(`${this.endpoint}/slug/${slug}`);
  }

  createService(service: Partial<Service>): Observable<Service> {
    return this.api.post<Service>(this.endpoint, service);
  }

  updateService(id: string, service: Partial<Service>): Observable<Service> {
    return this.api.put<Service>(`${this.endpoint}/${id}`, service);
  }

  deleteService(id: string): Observable<boolean> {
    return this.api.delete<boolean>(`${this.endpoint}/${id}`);
  }
}

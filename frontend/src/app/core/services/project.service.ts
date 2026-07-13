import { Injectable, inject } from '@angular/core';
import { ApiService } from './api.service';
import { Observable } from 'rxjs';
import { Project } from '../models/project.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private api = inject(ApiService);
  private endpoint = 'projects';

  getProjects(): Observable<Project[]> {
    return this.api.get<Project[]>(this.endpoint);
  }

  getProjectBySlug(slug: string): Observable<Project> {
    return this.api.get<Project>(`${this.endpoint}/slug/${slug}`);
  }
}

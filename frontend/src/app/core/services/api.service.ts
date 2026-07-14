import { Injectable, inject, PLATFORM_ID } from '@angular/core';
import { isPlatformServer } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ProjectImageDto {
  id: string;
  imageUrl: string;
  sortOrder: number;
}

export interface ProjectDto {
  id: string;
  title: string;
  slug: string;
  shortDescription?: string;
  description?: string;
  coverImageUrl?: string;
  categoryName?: string;
  categoryId?: string;
  projectDate?: string;
  status: number;
  displayOrder?: number;
  images?: ProjectImageDto[];
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private http = inject(HttpClient);
  private platformId = inject(PLATFORM_ID);
  
  private get apiUrl(): string {
    if (isPlatformServer(this.platformId)) {
      return 'http://backend:8080/api/v1';
    }
    return '/api/v1';
  }

  getProjects(): Observable<ProjectDto[]> {
    return this.http.get<ProjectDto[]>(`${this.apiUrl}/projects`);
  }

  getProjectBySlug(slug: string): Observable<ProjectDto> {
    return this.http.get<ProjectDto>(`${this.apiUrl}/projects/slug/${slug}`);
  }

  uploadProjectImage(projectId: string, file: File): Observable<ProjectImageDto> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<ProjectImageDto>(`${this.apiUrl}/projects/${projectId}/images`, formData);
  }

  deleteProjectImage(projectId: string, imageId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/projects/${projectId}/images/${imageId}`);
  }

  reorderProjectImages(projectId: string, imageIds: string[]): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/projects/${projectId}/images/reorder`, imageIds);
  }

  reorderProjects(projects: { id: string; displayOrder: number }[]): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/projects/reorder`, { projects });
  }

  get<T>(endpoint: string): Observable<T> {
    return this.http.get<T>(`${this.apiUrl}/${endpoint}`);
  }

  put<T>(endpoint: string, body: any): Observable<T> {
    return this.http.put<T>(`${this.apiUrl}/${endpoint}`, body);
  }

  post<T>(endpoint: string, body: any): Observable<T> {
    return this.http.post<T>(`${this.apiUrl}/${endpoint}`, body);
  }

  delete<T>(endpoint: string): Observable<T> {
    return this.http.delete<T>(`${this.apiUrl}/${endpoint}`);
  }
}

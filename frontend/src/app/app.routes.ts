import { Routes } from '@angular/router';
import { AdminLayoutComponent } from './layout/admin-layout/admin-layout.component';
import { DashboardComponent } from './features/admin/dashboard/dashboard.component';
import { ProjectsComponent } from './features/admin/projects/projects.component';
import { WebsiteLayoutComponent } from './layout/website-layout/website-layout.component';
import { AuthGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'admin',
    component: AdminLayoutComponent,
    canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'projects', component: ProjectsComponent },
      { 
        path: 'services', 
        loadComponent: () => import('./features/admin/services/services.component').then(m => m.ServicesComponent)
      },
      { 
        path: 'settings', 
        loadComponent: () => import('./features/admin/settings/settings.component').then(m => m.SettingsComponent)
      }
    ]
  },
  {
    path: '',
    component: WebsiteLayoutComponent,
    children: [
      {
        path: '',
        loadComponent: () => import('./features/website/home/home.component').then(m => m.HomeComponent)
      },
      {
        path: 'hakkimizda',
        loadComponent: () => import('./features/website/about/about.component').then(m => m.AboutComponent)
      },
      {
        path: 'hizmetlerimiz',
        loadComponent: () => import('./features/website/services/services.component').then(m => m.ServicesComponent)
      },
      {
        path: 'projelerimiz',
        loadComponent: () => import('./features/website/projects/projects.component').then(m => m.ProjectsComponent)
      },
      {
        path: 'projelerimiz/:slug',
        loadComponent: () => import('./features/website/project-detail/project-detail.component').then(m => m.ProjectDetailComponent)
      },
      {
        path: 'iletisim',
        loadComponent: () => import('./features/website/contact/contact.component').then(m => m.ContactComponent)
      }
    ]
  },
  {
    path: '**',
    redirectTo: ''
  }
];

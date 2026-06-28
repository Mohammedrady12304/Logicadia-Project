import { Component, OnInit , ChangeDetectorRef} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {
  private apiUrl = 'https://localhost:7213/api';

  stats = [
    { label: 'Users',        value: 0, icon: 'bi bi-people-fill',    route: '/admin/users',        color: '#1a1654' },
    { label: 'Levels',       value: 0, icon: 'bi bi-bar-chart-fill',  route: '/admin/levels',       color: '#2d2899' },
    { label: 'Stories',      value: 0, icon: 'bi bi-book-fill',       route: '/admin/stories',      color: '#3d37c9' },
    { label: 'Scenarios',    value: 0, icon: 'bi bi-diagram-3-fill',  route: '/admin/scenarios',    color: '#5a54d4' },
    { label: 'Choices',      value: 0, icon: 'bi bi-ui-checks',       route: '/admin/choices',      color: '#7a75de' },
    { label: 'Achievements', value: 0, icon: 'bi bi-trophy-fill',     route: '/admin/achievements', color: '#9b97e8' },
  ];

  loading = true;

  constructor(private http: HttpClient, private auth: AuthService ,  private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.loadStats();
    
  }

  private getHeaders() {
    return new HttpHeaders({ Authorization: `Bearer ${this.auth.getToken()}` });
  }

  loadStats() {
    const headers = this.getHeaders();
    const endpoints = [
      'AdminUsers', 'AdminLevels', 'AdminStories',
      'AdminScenarios', 'AdminChoices', 'AdminAchievements'
    ];

    let completed = 0;

    endpoints.forEach((ep, index) => {
      this.http.get<any[]>(`${this.apiUrl}/${ep}`, { headers }).subscribe({
        next: (data) => {
          this.stats[index].value = Array.isArray(data) ? data.length : 0;
          completed++;
          if (completed === endpoints.length) this.loading = false;
          this.cdr.detectChanges();
        },
        error: () => {
          completed++;
          if (completed === endpoints.length) this.loading = false;
        }
      });
    });
  }
}
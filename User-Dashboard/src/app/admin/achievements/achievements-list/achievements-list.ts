import { Component, OnInit  , ChangeDetectorRef} from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../../../core/services/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-achievements-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './achievements-list.html',
  styleUrl: './achievements-list.css'
})
export class AchievementsList implements OnInit {
  private apiUrl = 'https://localhost:44342/api/AdminAchievements';

  achievements: any[] = [];
  loading = true;
  error = '';

  // Pagination
  pageNumber = 1;
  pageSize = 10;
  totalCount = 0;

  // Create Modal
  showCreateModal = false;
  createForm = { title: '', description: '', iconUrl: '', triggerType: '', triggerValue: 0 };

  // Edit Modal
  showEditModal = false;
  selectedAchievement: any = null;
  editForm: any = {};

  // Delete Modal
  showDeleteModal = false;
  achievementToDelete: any = null;

  triggerTypes = ['LevelsCompleted', 'StoriesCompleted', 'XpEarned', 'CorrectAnswers', 'ScenariosPlayed'];

  constructor(private http: HttpClient, private auth: AuthService , private cdr : ChangeDetectorRef) {}

  ngOnInit() { this.loadAchievements(); }

  private getHeaders() {
    return new HttpHeaders({ Authorization: `Bearer ${this.auth.getToken()}` });
  }

  loadAchievements() {
    this.loading = true;
    this.http.get<any>(`${this.apiUrl}/paged?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`, { headers: this.getHeaders() })
      .subscribe({
        next: (res) => {
          if (Array.isArray(res)) {
            this.achievements = res;
            this.totalCount = res.length;
          } else {
            this.achievements = res.data ?? res.items ?? [];
            this.totalCount = res.totalCount ?? 0;
          }
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: () => {
          this.error = 'Failed to load achievements.';
          this.loading = false;
        }
      });
  }

  get totalPages() { return Math.ceil(this.totalCount / this.pageSize); }

  changePage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.pageNumber = page;
    this.loadAchievements();
  }

  // Create
  openCreate() {
    this.createForm = { title: '', description: '', iconUrl: '', triggerType: '', triggerValue: 0 };
    this.showCreateModal = true;
  }

  saveCreate() {
    this.http.post(this.apiUrl, this.createForm, { headers: this.getHeaders() }).subscribe({
      next: () => { this.showCreateModal = false; this.loadAchievements(); },
      error: () => alert('Failed to create achievement.')
    });
  }

  // Edit
  openEdit(achievement: any) {
    this.selectedAchievement = achievement;
    this.editForm = { ...achievement };
    this.showEditModal = true;
  }

  saveEdit() {
    this.http.put(`${this.apiUrl}/${this.selectedAchievement.id}`, this.editForm, { headers: this.getHeaders() }).subscribe({
      next: () => { this.showEditModal = false; this.loadAchievements(); },
      error: () => alert('Failed to update achievement.')
    });
  }

  // Delete
  openDelete(achievement: any) {
    this.achievementToDelete = achievement;
    this.showDeleteModal = true;
  }

  confirmDelete() {
    this.http.delete(`${this.apiUrl}/${this.achievementToDelete.id}`, { headers: this.getHeaders() }).subscribe({
      next: () => { this.showDeleteModal = false; this.loadAchievements(); },
      error: () => alert('Failed to delete achievement.')
    });
  }
}
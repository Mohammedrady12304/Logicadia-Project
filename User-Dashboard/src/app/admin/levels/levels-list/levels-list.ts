import { Component, OnInit , ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../../../core/services/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-levels-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './levels-list.html',
  styleUrl: './levels-list.css'
})
export class LevelsList implements OnInit {
  private apiUrl = 'https://localhost:7213/api/AdminLevels';

  levels: any[] = [];
  loading = true;
  error = '';

  // Pagination
  pageNumber = 1;
  pageSize = 10;
  totalCount = 0;

  // Create Modal
  showCreateModal = false;
  createForm = { title: '', description: '', orderIndex: 0, xpReward: 0 };

  // Edit Modal
  showEditModal = false;
  selectedLevel: any = null;
  editForm: any = {};

  // Delete Modal
  showDeleteModal = false;
  levelToDelete: any = null;

  constructor(private http: HttpClient, private auth: AuthService, private cdr : ChangeDetectorRef) {}

  ngOnInit() { this.loadLevels(); }

  private getHeaders() {
    return new HttpHeaders({ Authorization: `Bearer ${this.auth.getToken()}` });
  }

  loadLevels() {
    this.loading = true;
    const headers = this.getHeaders();
    this.http.get<any>(`${this.apiUrl}/paged?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`, { headers })
      .subscribe({
        next: (res) => {
          if (Array.isArray(res)) {
            this.levels = res;
            this.totalCount = res.length;
          } else {
            this.levels = res.data ?? res.items ?? [];
            this.totalCount = res.totalCount ?? 0;
          }
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: () => {
          this.error = 'Failed to load levels.';
          this.loading = false;
        }
      });
  }

  get totalPages() { return Math.ceil(this.totalCount / this.pageSize); }

  changePage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.pageNumber = page;
    this.loadLevels();
  }

  // Create
  openCreate() {
    this.createForm = { title: '', description: '', orderIndex: 0, xpReward: 0 };
    this.showCreateModal = true;
  }

  saveCreate() {
    const headers = this.getHeaders();
    this.http.post(this.apiUrl, this.createForm, { headers })
      .subscribe({
        next: () => { this.showCreateModal = false; this.loadLevels(); },
        error: () => alert('Failed to create level.')
      });
  }

  // Edit
  openEdit(level: any) {
    this.selectedLevel = level;
    this.editForm = { ...level };
    this.showEditModal = true;
  }

  saveEdit() {
    const headers = this.getHeaders();
    this.http.put(`${this.apiUrl}/${this.selectedLevel.id}`, this.editForm, { headers })
      .subscribe({
        next: () => { this.showEditModal = false; this.loadLevels(); },
        error: () => alert('Failed to update level.')
      });
  }

  // Delete
  openDelete(level: any) {
    this.levelToDelete = level;
    this.showDeleteModal = true;
  }

  confirmDelete() {
    const headers = this.getHeaders();
    this.http.delete(`${this.apiUrl}/${this.levelToDelete.id}`, { headers })
      .subscribe({
        next: () => { this.showDeleteModal = false; this.loadLevels(); },
        error: () => alert('Failed to delete level.')
      });
  }
}
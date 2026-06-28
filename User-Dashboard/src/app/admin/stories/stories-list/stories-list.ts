import { Component, OnInit , ChangeDetectorRef} from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../../../core/services/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-stories-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './stories-list.html',
  styleUrl: './stories-list.css'
})
export class StoriesList implements OnInit {
  private apiUrl = 'https://localhost:44342/api/AdminStories';
  private levelsUrl = 'https://localhost:44342/api/AdminLevels';

  stories: any[] = [];
  levels: any[] = [];
  loading = true;
  error = '';

  // Filter
  selectedLevelId: number | null = null;

  // Pagination
  pageNumber = 1;
  pageSize = 10;
  totalCount = 0;

  // Create Modal
  showCreateModal = false;
  createForm = { levelId: 0, title: '', narrativeText: '', orderIndex: 0 };

  // Edit Modal
  showEditModal = false;
  selectedStory: any = null;
  editForm: any = {};

  // Delete Modal
  showDeleteModal = false;
  storyToDelete: any = null;

  constructor(private http: HttpClient, private auth: AuthService,private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.loadLevels();
    this.loadStories();
  }

  private getHeaders() {
    return new HttpHeaders({ Authorization: `Bearer ${this.auth.getToken()}` });
  }

  loadLevels() {
    this.http.get<any[]>(this.levelsUrl, { headers: this.getHeaders() })
      .subscribe({ next: (res) => this.levels = res });
  }

  loadStories() {
    this.loading = true;
    const headers = this.getHeaders();
    const url = this.selectedLevelId
      ? `${this.apiUrl}/paged/by-level/${this.selectedLevelId}?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`
      : `${this.apiUrl}/paged?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`;

    this.http.get<any>(url, { headers }).subscribe({
      next: (res) => {
        if (Array.isArray(res)) {
          this.stories = res;
          this.totalCount = res.length;
        } else {
          this.stories = res.data ?? res.items ?? [];
          this.totalCount = res.totalCount ?? 0;
        }
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Failed to load stories.';
        this.loading = false;
      }
    });
  }

  onLevelFilter() {
    this.pageNumber = 1;
    this.loadStories();
  }

  clearFilter() {
    this.selectedLevelId = null;
    this.pageNumber = 1;
    this.loadStories();
  }

  getLevelTitle(levelId: number) {
    return this.levels.find(l => l.id === levelId)?.title ?? '—';
  }

  get totalPages() { return Math.ceil(this.totalCount / this.pageSize); }

  changePage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.pageNumber = page;
    this.loadStories();
  }

  // Create
  openCreate() {
    this.createForm = { levelId: this.selectedLevelId ?? 0, title: '', narrativeText: '', orderIndex: 0 };
    this.showCreateModal = true;
  }

  saveCreate() {
    const headers = this.getHeaders();
    this.http.post(this.apiUrl, this.createForm, { headers }).subscribe({
      next: () => { this.showCreateModal = false; this.loadStories(); },
      error: () => alert('Failed to create story.')
    });
  }

  // Edit
  openEdit(story: any) {
    this.selectedStory = story;
    this.editForm = { ...story };
    this.showEditModal = true;
  }

  saveEdit() {
    const headers = this.getHeaders();
    this.http.put(`${this.apiUrl}/${this.selectedStory.id}`, this.editForm, { headers }).subscribe({
      next: () => { this.showEditModal = false; this.loadStories(); },
      error: () => alert('Failed to update story.')
    });
  }

  // Delete
  openDelete(story: any) {
    this.storyToDelete = story;
    this.showDeleteModal = true;
  }

  confirmDelete() {
    const headers = this.getHeaders();
    this.http.delete(`${this.apiUrl}/${this.storyToDelete.id}`, { headers }).subscribe({
      next: () => { this.showDeleteModal = false; this.loadStories(); },
      error: () => alert('Failed to delete story.')
    });
  }
}
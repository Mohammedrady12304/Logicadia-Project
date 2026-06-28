import { Component, OnInit , ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../../../core/services/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-scenarios-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './scenarios-list.html',
  styleUrl: './scenarios-list.css'
})
export class ScenariosList implements OnInit {
  private apiUrl = 'https://localhost:7213/api/AdminScenarios';
  private storiesUrl = 'https://localhost:7213/api/AdminStories';

  scenarios: any[] = [];
  stories: any[] = [];
  loading = true;
  error = '';

  // Filter
  selectedStoryId: number | null = null;

  // Pagination
  pageNumber = 1;
  pageSize = 10;
  totalCount = 0;

  // Create Modal
  showCreateModal = false;
  createForm = { storyId: 0, title: '', description: '', orderIndex: 0 };

  // Edit Modal
  showEditModal = false;
  selectedScenario: any = null;
  editForm: any = {};

  // Delete Modal
  showDeleteModal = false;
  scenarioToDelete: any = null;

  constructor(private http: HttpClient, private auth: AuthService , private cdr : ChangeDetectorRef) {}

  ngOnInit() {
    this.loadStories();
    this.loadScenarios();
  }

  private getHeaders() {
    return new HttpHeaders({ Authorization: `Bearer ${this.auth.getToken()}` });
  }

  loadStories() {
    this.http.get<any[]>(this.storiesUrl, { headers: this.getHeaders() })
      .subscribe({ next: (res) => this.stories = res });
  }

  loadScenarios() {
    this.loading = true;
    const headers = this.getHeaders();
    const url = this.selectedStoryId
      ? `${this.apiUrl}/paged/by-story/${this.selectedStoryId}?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`
      : `${this.apiUrl}/paged?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`;

    this.http.get<any>(url, { headers }).subscribe({
      next: (res) => {
        if (Array.isArray(res)) {
          this.scenarios = res;
          this.totalCount = res.length;
        } else {
          this.scenarios = res.data ?? res.items ?? [];
          this.totalCount = res.totalCount ?? 0;
        }
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Failed to load scenarios.';
        this.loading = false;
      }
    });
  }

  onStoryFilter() {
    this.pageNumber = 1;
    this.loadScenarios();
  }

  clearFilter() {
    this.selectedStoryId = null;
    this.pageNumber = 1;
    this.loadScenarios();
  }

  getStoryTitle(storyId: number) {
    return this.stories.find(s => s.id === storyId)?.title ?? '—';
  }

  get totalPages() { return Math.ceil(this.totalCount / this.pageSize); }

  changePage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.pageNumber = page;
    this.loadScenarios();
  }

  // Create
  openCreate() {
    this.createForm = { storyId: this.selectedStoryId ?? 0, title: '', description: '', orderIndex: 0 };
    this.showCreateModal = true;
  }

  saveCreate() {
    this.http.post(this.apiUrl, this.createForm, { headers: this.getHeaders() }).subscribe({
      next: () => { this.showCreateModal = false; this.loadScenarios(); },
      error: () => alert('Failed to create scenario.')
    });
  }

  // Edit
  openEdit(scenario: any) {
    this.selectedScenario = scenario;
    this.editForm = { ...scenario };
    this.showEditModal = true;
  }

  saveEdit() {
    this.http.put(`${this.apiUrl}/${this.selectedScenario.id}`, this.editForm, { headers: this.getHeaders() }).subscribe({
      next: () => { this.showEditModal = false; this.loadScenarios(); },
      error: () => alert('Failed to update scenario.')
    });
  }

  // Delete
  openDelete(scenario: any) {
    this.scenarioToDelete = scenario;
    this.showDeleteModal = true;
  }

  confirmDelete() {
    this.http.delete(`${this.apiUrl}/${this.scenarioToDelete.id}`, { headers: this.getHeaders() }).subscribe({
      next: () => { this.showDeleteModal = false; this.loadScenarios(); },
      error: () => alert('Failed to delete scenario.')
    });
  }
}
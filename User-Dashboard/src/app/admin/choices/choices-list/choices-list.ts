import { Component, OnInit , ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../../../core/services/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-choices-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './choices-list.html',
  styleUrl: './choices-list.css'
})
export class ChoicesList implements OnInit {
  private apiUrl = 'https://localhost:7213/api/AdminChoices';
  private scenariosUrl = 'https://localhost:7213/api/AdminScenarios';

  choices: any[] = [];
  scenarios: any[] = [];
  loading = true;
  error = '';

  // Filter
  selectedScenarioId: number | null = null;

  // Pagination
  pageNumber = 1;
  pageSize = 10;
  totalCount = 0;

  // Create Modal
  showCreateModal = false;
  createForm = { scenarioId: 0, choiceText: '', isCorrect: false, feedback: '', xpValue: 0 };

  // Edit Modal
  showEditModal = false;
  selectedChoice: any = null;
  editForm: any = {};

  // Delete Modal
  showDeleteModal = false;
  choiceToDelete: any = null;

  constructor(private http: HttpClient, private auth: AuthService , private cdr : ChangeDetectorRef) {}

  ngOnInit() {
    this.loadScenarios();
    this.loadChoices();
  }

  private getHeaders() {
    return new HttpHeaders({ Authorization: `Bearer ${this.auth.getToken()}` });
  }

  loadScenarios() {
    this.http.get<any[]>(this.scenariosUrl, { headers: this.getHeaders() })
      .subscribe({ next: (res) => this.scenarios = res });
  }

  loadChoices() {
    this.loading = true;
    const url = this.selectedScenarioId
      ? `${this.apiUrl}/paged/by-scenario/${this.selectedScenarioId}?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`
      : `${this.apiUrl}/paged?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`;

    this.http.get<any>(url, { headers: this.getHeaders() }).subscribe({
      next: (res) => {
        if (Array.isArray(res)) {
          this.choices = res;
          this.totalCount = res.length;
        } else {
          this.choices = res.data ?? res.items ?? [];
          this.totalCount = res.totalCount ?? 0;
        }
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Failed to load choices.';
        this.loading = false;
      }
    });
  }

  onScenarioFilter() {
    this.pageNumber = 1;
    this.loadChoices();
  }

  clearFilter() {
    this.selectedScenarioId = null;
    this.pageNumber = 1;
    this.loadChoices();
  }

  getScenarioTitle(scenarioId: number) {
    return this.scenarios.find(s => s.id === scenarioId)?.title ?? '—';
  }

  get totalPages() { return Math.ceil(this.totalCount / this.pageSize); }

  changePage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.pageNumber = page;
    this.loadChoices();
  }

  // Create
  openCreate() {
    this.createForm = { scenarioId: this.selectedScenarioId ?? 0, choiceText: '', isCorrect: false, feedback: '', xpValue: 0 };
    this.showCreateModal = true;
  }

  saveCreate() {
    this.http.post(this.apiUrl, this.createForm, { headers: this.getHeaders() }).subscribe({
      next: () => { this.showCreateModal = false; this.loadChoices(); },
      error: () => alert('Failed to create choice.')
    });
  }

  // Edit
  openEdit(choice: any) {
    this.selectedChoice = choice;
    this.editForm = { ...choice };
    this.showEditModal = true;
  }

  saveEdit() {
    this.http.put(`${this.apiUrl}/${this.selectedChoice.id}`, this.editForm, { headers: this.getHeaders() }).subscribe({
      next: () => { this.showEditModal = false; this.loadChoices(); },
      error: () => alert('Failed to update choice.')
    });
  }

  // Delete
  openDelete(choice: any) {
    this.choiceToDelete = choice;
    this.showDeleteModal = true;
  }

  confirmDelete() {
    this.http.delete(`${this.apiUrl}/${this.choiceToDelete.id}`, { headers: this.getHeaders() }).subscribe({
      next: () => { this.showDeleteModal = false; this.loadChoices(); },
      error: () => alert('Failed to delete choice.')
    });
  }
}
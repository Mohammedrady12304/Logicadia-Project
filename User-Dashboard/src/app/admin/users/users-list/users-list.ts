import { Component, OnInit ,ChangeDetectorRef} from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../../../core/services/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-users-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './users-list.html',
  styleUrl: './users-list.css'
})
export class UsersList implements OnInit {
  private apiUrl = 'https://localhost:7213/api/AdminUsers';

  users: any[] = [];
  loading = true;
  error = '';

  // Pagination
  pageNumber = 1;
  pageSize = 10;
  totalCount = 0;

  // Edit Modal
  showEditModal = false;
  selectedUser: any = null;
  editForm: any = {};

  // Delete Modal
  showDeleteModal = false;
  userToDelete: any = null;

  constructor(private http: HttpClient, private auth: AuthService  ,  private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.loadUsers();
  }

  private getHeaders() {
    return new HttpHeaders({ Authorization: `Bearer ${this.auth.getToken()}` });
  }

  loadUsers() {
  this.loading = true;
  const headers = this.getHeaders();
  this.http.get<any>(`${this.apiUrl}/paged?pageNumber=${this.pageNumber}&pageSize=${this.pageSize}`, { headers })
    .subscribe({
      next: (res) => {
        // API بترجع array مباشرة
        if (Array.isArray(res)) {
          this.users = res;
          
          this.totalCount = res.length;
        } else {
          this.users = res.data ?? [];
          this.totalCount = res.totalCount ?? 0;
            console.log(res);
        }
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Failed to load users.';
        this.loading = false;
      }
    });
}

  get totalPages() {
    return Math.ceil(this.totalCount / this.pageSize);
  }

  changePage(page: number) {
    if (page < 1 || page > this.totalPages) return;
    this.pageNumber = page;
    this.loadUsers();
  }

  // Edit
  openEdit(user: any) {
    this.selectedUser = user;
    this.editForm = { ...user };
    this.showEditModal = true;
  }

  saveEdit() {
    const headers = this.getHeaders();
    this.http.put(`${this.apiUrl}/${this.selectedUser.id}`, this.editForm, { headers })
      .subscribe({
        next: () => {
          this.showEditModal = false;
          this.loadUsers();
        },
        error: () => alert('Failed to update user.')
      });
  }

  // Delete
  openDelete(user: any) {
    this.userToDelete = user;
    this.showDeleteModal = true;
  }

  confirmDelete() {
    const headers = this.getHeaders();
    this.http.delete(`${this.apiUrl}/${this.userToDelete.id}`, { headers })
      .subscribe({
        next: () => {
          this.showDeleteModal = false;
          this.loadUsers();
        },
        error: () => alert('Failed to delete user.')
      });
  }

  // Ban / Unban
  toggleBan(user: any) {
  const headers = this.getHeaders();
  const action = user.isLocked ? 'unban' : 'ban';
  this.http.put(`${this.apiUrl}/${user.id}/${action}`, {}, { headers })
    .subscribe({
      next: () => this.loadUsers(),
      error: () => alert(`Failed to ${action} user.`)
    });
}
}
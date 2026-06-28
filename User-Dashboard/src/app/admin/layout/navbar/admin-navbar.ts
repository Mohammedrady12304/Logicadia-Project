import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-admin-navbar',
  standalone: true,
  templateUrl: './admin-navbar.html',
  styleUrl: './admin-navbar.css'
})
export class AdminNavbar {
  @Output() toggleSidebar = new EventEmitter<void>();

  constructor(private auth: AuthService) {}

  get userName() {
    return this.auth.getUserName() ?? 'Admin';
  }

  logout() {
    this.auth.logout();
  }
}
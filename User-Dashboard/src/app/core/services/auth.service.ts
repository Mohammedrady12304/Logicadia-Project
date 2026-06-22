import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:44342/api';
  private tokenKey = 'token';

  constructor(private http: HttpClient, private router: Router) {}

  login(Username: string, Password: string) {
    return this.http.post<{ token: string; role: string }>(`${this.apiUrl}/auth/login`, { Username, Password });
  }

  saveToken(token: string, role: string) {
    localStorage.setItem(this.tokenKey, token);
    localStorage.setItem('role', role);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  getRole(): string | null {
    return localStorage.getItem('role');
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;
    try {
      const payload = token.split('.')[1];
      const decoded = JSON.parse(atob(payload));
      return decoded['exp'] ? Date.now() < decoded['exp'] * 1000 : false;
    } catch {
      return false;
    }
  }

  isUser(): boolean {
    return this.getRole() === 'User';
  }

  isAdmin(): boolean {
    return this.getRole() === 'Admin';
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem('role');
    this.router.navigate(['/login']);
  }
}
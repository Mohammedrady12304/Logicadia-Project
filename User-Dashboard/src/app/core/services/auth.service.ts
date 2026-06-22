import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5000/api';
  private tokenKey = 'token';

  constructor(private http: HttpClient, private router: Router) {}

  login(email: string, password: string) {
    return this.http.post<{ token: string }>(`${this.apiUrl}/auth/login`, { email, password });
  }

  saveToken(token: string) {
    localStorage.setItem(this.tokenKey, token);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  private decodeToken(): any {
    const token = this.getToken();
    if (!token) return null;
    try {
      const payload = token.split('.')[1];
      return JSON.parse(atob(payload));
    } catch {
      return null;
    }
  }

  getRole(): string | null {
    const decoded = this.decodeToken();
    if (!decoded) return null;
    return decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? null;
  }

  isLoggedIn(): boolean {
    const decoded = this.decodeToken();
    if (!decoded) return false;
    return decoded['exp'] ? Date.now() < decoded['exp'] * 1000 : false;
  }

  isUser(): boolean {
    return this.getRole() === 'User';
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    this.router.navigate(['/login']);
  }
}
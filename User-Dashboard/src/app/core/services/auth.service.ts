import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap } from 'rxjs';

export interface LoginDto {
  username: string;
  password: string;
}

export interface RegisterParentDto {
  email: string;
  password: string;
  fullName: string;
  phoneNumber: string;
}

export interface RegisterChildDto {
  name: string;
  age: number;
  password: string;
}

export interface AuthResultDto {
  token: string;
  role: string;
  isSuccess: boolean;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);

  private apiUrl = 'https://localhost:44342/api';
  private tokenKey = 'token';

  login(credentials: LoginDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/auth/login`, credentials).pipe(
      tap(response => {
        const token = response.token || response.Token;
        const role = response.role || response.Role;
        const isSuccess = response.isSuccess !== undefined ? response.isSuccess : true;

        if (isSuccess && token) {
          this.saveToken(token);
          this.saveRole(role);
          if (role) {
            localStorage.setItem('role', role);
          }
        }
      })
    );
  }

  registerParent(parentData: RegisterParentDto): Observable<AuthResultDto> {
    return this.http.post<AuthResultDto>(`${this.apiUrl}/auth/register-parent`, parentData);
  }

  registerChild(childData: RegisterChildDto): Observable<AuthResultDto> {
    return this.http.post<AuthResultDto>(`${this.apiUrl}/auth/register-child`, childData);
  }

  private saveToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  private saveRole(role: string): void {
    localStorage.setItem('userRole', role);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  getRole(): string | null {
    return localStorage.getItem('userRole') || localStorage.getItem('role');
  }

  getUserName(): string | null {
    return localStorage.getItem('userName');
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;
    try {
      const payload = token.split('.')[1];
      const decoded = JSON.parse(atob(payload));
      return decoded['exp'] ? Date.now() < decoded['exp'] * 1000 : true;
    } catch {
      return false;
    }
  }

  isUser(): boolean {
    return this.getRole() === 'Child';
  }

  isAdmin(): boolean {
    return this.getRole() === 'Admin';
  }

  isParent(): boolean {
    return this.getRole() === 'Parent';
  }

  isChild(): boolean {
    return this.getRole() === 'Child';
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem('userRole');
    localStorage.removeItem('role');
    this.router.navigate(['/login']);
  }
}
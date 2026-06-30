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
  
  private apiUrl = 'https://localhost:7213/api/auth'; 

  // تسجيل الدخول المعدل لقراءة الـ Response بأي حالة أحرف (Capital or Small)
  login(credentials: LoginDto): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials).pipe(
      tap(response => {
        // تأمين قراءة التوكن والـ Role بأي طريقة مبعوتة بها من الـ .NET
        const token = response.token || response.Token;
        const role = response.role || response.Role;
        const isSuccess = response.isSuccess !== undefined ? response.isSuccess : true; 

        if (isSuccess && token) {
          this.saveToken(token);
          this.saveRole(role);
        }
      })
    );
  }

  registerParent(parentData: RegisterParentDto): Observable<AuthResultDto> {
    return this.http.post<AuthResultDto>(`${this.apiUrl}/register-parent`, parentData);
  }

  registerChild(childData: RegisterChildDto): Observable<AuthResultDto> {
    return this.http.post<AuthResultDto>(`${this.apiUrl}/register-child`, childData);
  }

  private saveToken(token: string): void {
    localStorage.setItem('token', token);
  }

  private saveRole(role: string): void {
    localStorage.setItem('userRole', role);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getRole(): string | null {
    return localStorage.getItem('userRole');
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
    localStorage.removeItem('token');
    localStorage.removeItem('userRole');
    this.router.navigate(['/login']);
  }
}
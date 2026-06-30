import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private authService = inject(AuthService);
  private router = inject(Router);

  Username = '';
  Password = '';
  
  errorMessage = '';
  isLoading = false;

  login() {
    if (!this.Username || !this.Password) {
      this.errorMessage = 'Please enter both username and password.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const credentials = { username: this.Username, password: this.Password };

    this.authService.login(credentials).subscribe({
      next: (response: any) => {
        // نضمن دائماً إغلاق الـ Spinner عند استقبال الرد بنجاح
        this.isLoading = false;

        // استخراج الـ Role بشكل آمن مع مراعاة حالة الأحرف المتوقعة من الـ API
        const userRole = response.role || response.Role;

        if (userRole === 'Admin') {
          this.router.navigate(['/admin']); 
        } else if (userRole === 'Parent') {
          this.router.navigate(['/parent/children']); 
        } else if (userRole === 'Child') {
          this.router.navigate(['/levels']); 
        } else {
          this.errorMessage = 'You are not authorized to access this area.';
          this.authService.logout();
        }
      },
      error: (err: any) => {
        this.isLoading = false;
        // عرض رسالة الخطأ القادمة من الـ API (مثل Invalid username/email...) بدلاً من الرسالة الثابتة
        this.errorMessage = err.error?.message || 'Invalid username or password.';
      }
    });
  }
}
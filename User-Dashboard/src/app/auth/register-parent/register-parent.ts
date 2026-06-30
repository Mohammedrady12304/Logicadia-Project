import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-register-parent',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterLink],
  templateUrl: './register-parent.html',
  styleUrl: './register-parent.css'
})
export class RegisterParentComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  // 1. رجعنا المسميات القديمة هنا لكي لا يشتكي الـ HTML أو الـ Validation
  registerModel = {
    email: '',
    password: '',
    fullName: '',
    phoneNumber: ''
  };

  errorMessage = '';
  successMessage = '';
  isLoading = false;

  onRegister() {
    if (!this.registerModel.email || !this.registerModel.password || !this.registerModel.fullName || !this.registerModel.phoneNumber) {
      this.errorMessage = 'All fields are required.';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    // ✨ الخدعة الذكية هنا:
    // نقوم بإنشاء الأوبجكت الذي ينتظره الباك-إند بالأسماء التي يحبها (username و phone)
    // ونمرره كـ "any" لكي نتحايل على قيود التايب سكريبت الصارمة في الـ Service
    const payloadToSend: any = {
      username: this.registerModel.email,       // وضعنا الإيميل في الـ username للباك-إند
      password: this.registerModel.password,
      fullName: this.registerModel.fullName,
      phone: this.registerModel.phoneNumber     // وضعنا رقم الهاتف في الـ phone للباك-إند
    };

    // نرسل الـ payloadToSend المعدل بدلاً من الأوبجكت الأصلي
    this.authService.registerParent(payloadToSend).subscribe({
      next: (response: any) => {
        if (response.isSuccess) {
          this.successMessage = 'Account created successfully! Redirecting to login...';
          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 2000);
        } else {
          this.errorMessage = response.message || 'Registration failed. Please try again.';
        }
        this.isLoading = false;
      },
      error: (err: any) => {
        this.errorMessage = err.error?.message || 'An error occurred during registration. Please try again later.';
        this.isLoading = false;
      }
    });
  }
}
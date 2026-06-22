import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [FormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  Username = '';
  Password = '';
  errorMessage = '';
  isLoading = false;

  constructor(private authService: AuthService, private router: Router) {}

  login() {
    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login(this.Username, this.Password).subscribe({
      next: (response) => {
        this.authService.saveToken(response.token, response.role);
        if (this.authService.isUser()) {
          this.router.navigate(['/levels']);
        } else {
          this.errorMessage = 'You are not authorized to access this dashboard.';
          this.authService.logout();
        }
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Invalid Username or password.';
        this.isLoading = false;
      }
    });
  }
}
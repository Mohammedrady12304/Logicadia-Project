import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [FormsModule ],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  email = '';
  password = '';
  errorMessage = '';
  isLoading = false;

  constructor(private authService: AuthService, private router: Router) {}

  login() {
    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login(this.email, this.password).subscribe({
      next: (response) => {
        this.authService.saveToken(response.token);
        if (this.authService.isUser()) {
          this.router.navigate(['/levels']);
        } else {
          this.errorMessage = 'You are not authorized to access this dashboard.';
          this.authService.logout();
        }
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Invalid email or password.';
        this.isLoading = false;
      }
    });
}}

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
    console.log('login called', this.Username, this.Password);
    this.isLoading = true;
    this.errorMessage = '';

    this.authService.login(this.Username, this.Password).subscribe({
      next: (response) => {
        console.log('response', response);
        this.authService.saveToken(response.token, response.role);
        console.log('role is:', response.role);

        if (response.role === 'Admin') {
          console.log('navigating to admin');
          this.router.navigate(['/admin']);
        } else if (response.role === 'Parent') {
          console.log('navigating to parent');
          this.router.navigate(['/parent/children']);
        } else if (response.role === 'Child') {
          console.log('navigating to levels');
          this.router.navigate(['/levels']);
        } else {
          console.log('not authorized');
          this.errorMessage = 'You are not authorized.';
          this.authService.logout();
        }

        this.isLoading = false;
      },
      error: (err) => {
        console.log('error', err);
        this.errorMessage = 'Invalid Username or password.';
        this.isLoading = false;
      }
    });
  }
}
import { Component, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-register-child',
  imports: [CommonModule, FormsModule],
  templateUrl: './register-child.html',
  styleUrl: './register-child.css',
})
export class RegisterChild {

  model = {
    username: '',
    password: '',
    name: '',
    age: 0
  };

  successMessage = '';
  errorMessage = '';

  constructor(
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  registerChild() {

    this.successMessage = '';
    this.errorMessage = '';

    this.authService.registerChild(this.model).subscribe({

      next: (res: any) => {

        console.log('Success', res);

        this.successMessage = res.message || '🎉 Child registered successfully!';

        this.cdr.detectChanges();

        setTimeout(() => {

          this.model = {
            username: '',
            password: '',
            name: '',
            age: 0
          };

          this.cdr.detectChanges();

        }, 100);

      },

      error: (err) => {

        console.error(err);

        this.errorMessage =
          err?.error?.message ||
          err?.error ||
          'Something went wrong';

        this.cdr.detectChanges();

      }

    });

  }

}
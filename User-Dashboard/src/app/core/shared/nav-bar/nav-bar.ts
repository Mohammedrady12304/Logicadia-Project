import { Component, ChangeDetectorRef, OnInit, OnDestroy } from '@angular/core';
import { RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { interval, Subscription } from 'rxjs';

@Component({
  selector: 'app-nav-bar',
  imports: [RouterModule],
  templateUrl: './nav-bar.html',
  styleUrl: './nav-bar.css',
})
export class NavBar implements OnInit, OnDestroy {
  isLoggedIn: boolean = false;
  userName: string = '';
  private sub!: Subscription;

  constructor(
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    // تابع التغييرات كل 300ms
    this.sub = interval(300).subscribe(() => {
      const newLogin = this.authService.isLoggedIn();
      const newName = this.authService.getUserName() ?? '';

      if (newLogin !== this.isLoggedIn || newName !== this.userName) {
        this.isLoggedIn = newLogin;
        this.userName = newName;
        this.cdr.markForCheck();
      }
    });
  }

  logout() {
    this.authService.logout();
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }
}
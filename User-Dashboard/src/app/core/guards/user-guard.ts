import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const userGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isLoggedIn()) {

    const role = localStorage.getItem('role'); 
    
    if (role === 'Child' || role === 'Parent') { 
      return true;
    }
    
   
  }

  router.navigate(['/login']);
  return false;
};
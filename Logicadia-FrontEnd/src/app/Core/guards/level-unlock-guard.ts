import { CanActivateFn } from '@angular/router';

export const levelUnlockGuard: CanActivateFn = (route, state) => {
  return true;
};

import { TestBed } from '@angular/core/testing';
import { CanActivateFn } from '@angular/router';

import { levelUnlockGuard } from './level-unlock-guard';

describe('levelUnlockGuard', () => {
  const executeGuard: CanActivateFn = (...guardParameters) =>
    TestBed.runInInjectionContext(() => levelUnlockGuard(...guardParameters));

  beforeEach(() => {
    TestBed.configureTestingModule({});
  });

  it('should be created', () => {
    expect(executeGuard).toBeTruthy();
  });
});

import { TestBed } from '@angular/core/testing';

import { StorySevice } from './story.sevice';

describe('StorySevice', () => {
  let service: StorySevice;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StorySevice);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

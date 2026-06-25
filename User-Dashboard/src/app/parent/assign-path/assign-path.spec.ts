import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignPath } from './assign-path';

describe('AssignPath', () => {
  let component: AssignPath;
  let fixture: ComponentFixture<AssignPath>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AssignPath],
    }).compileComponents();

    fixture = TestBed.createComponent(AssignPath);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

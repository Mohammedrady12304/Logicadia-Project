import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterParent } from './register-parent';

describe('RegisterParent', () => {
  let component: RegisterParent;
  let fixture: ComponentFixture<RegisterParent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegisterParent],
    }).compileComponents();

    fixture = TestBed.createComponent(RegisterParent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

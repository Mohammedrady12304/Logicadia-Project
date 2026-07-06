import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterChild } from './register-child';

describe('RegisterChild', () => {
  let component: RegisterChild;
  let fixture: ComponentFixture<RegisterChild>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RegisterChild],
    }).compileComponents();

    fixture = TestBed.createComponent(RegisterChild);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

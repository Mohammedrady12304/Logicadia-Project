import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChildProgress } from './child-progress';

describe('ChildProgress', () => {
  let component: ChildProgress;
  let fixture: ComponentFixture<ChildProgress>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChildProgress],
    }).compileComponents();

    fixture = TestBed.createComponent(ChildProgress);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

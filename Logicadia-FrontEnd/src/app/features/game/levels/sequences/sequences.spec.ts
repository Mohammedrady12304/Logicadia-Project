import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Sequences } from './sequences';

describe('Sequences', () => {
  let component: Sequences;
  let fixture: ComponentFixture<Sequences>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Sequences],
    }).compileComponents();

    fixture = TestBed.createComponent(Sequences);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

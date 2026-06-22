import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LevelDetail } from './level-detail';

describe('LevelDetail', () => {
  let component: LevelDetail;
  let fixture: ComponentFixture<LevelDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LevelDetail],
    }).compileComponents();

    fixture = TestBed.createComponent(LevelDetail);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

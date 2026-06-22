import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LevelList } from './level-list';

describe('LevelList', () => {
  let component: LevelList;
  let fixture: ComponentFixture<LevelList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LevelList],
    }).compileComponents();

    fixture = TestBed.createComponent(LevelList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StoryDetail } from './story-detail';

describe('StoryDetail', () => {
  let component: StoryDetail;
  let fixture: ComponentFixture<StoryDetail>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StoryDetail],
    }).compileComponents();

    fixture = TestBed.createComponent(StoryDetail);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

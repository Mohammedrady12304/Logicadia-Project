import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScenarioPlay } from './scenario-play';

describe('ScenarioPlay', () => {
  let component: ScenarioPlay;
  let fixture: ComponentFixture<ScenarioPlay>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ScenarioPlay],
    }).compileComponents();

    fixture = TestBed.createComponent(ScenarioPlay);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

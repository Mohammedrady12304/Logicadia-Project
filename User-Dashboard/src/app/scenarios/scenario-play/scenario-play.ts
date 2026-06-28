import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ScenarioService } from '../../core/services/scenario.service';
import { ScenarioPlayDto, ChoiceOptionDto, SubmitResultDto } from '../../core/models/scenario.model';

interface StoryTheme {
  hero: string;
  destination: string;
  bg: string;
  floor: string;
  topic: string;
  description: string;
}

@Component({
  selector: 'app-scenario-play',
  imports: [CommonModule],
  templateUrl: './scenario-play.html',
  styleUrl: './scenario-play.css',
})
export class ScenarioPlay implements OnInit {
  scenario: ScenarioPlayDto | null = null;
  selectedChoiceId: number | null = null;
  result: SubmitResultDto | null = null;
  isLoading = false;
  isSubmitting = false;
  errorMessage = '';
  heroPosition = 30;
  correctCount = 0;
  totalScenarios = 3;
  currentScenarioIndex = 0;

  private storyThemes: StoryTheme[] = [
    { hero: '🧒', destination: '🚪', bg: '#1a1654', floor: '#2d2a6e', topic: 'Sequences', description: 'Follow the steps in order!' },
    { hero: '🔄', destination: '⛰️', bg: '#0a2e0a', floor: '#1a4e1a', topic: 'Loops', description: 'Repeat to reach the goal!' },
    { hero: '🤔', destination: '🌉', bg: '#2e1a0a', floor: '#4e2d1a', topic: 'Conditions', description: 'Choose the right path!' },
    { hero: '🎒', destination: '💎', bg: '#2e0a0a', floor: '#4e1a1a', topic: 'Variables', description: 'Collect and store!' },
    { hero: '🧙', destination: '⭐', bg: '#0a0a2e', floor: '#1a1a4e', topic: 'Functions', description: 'Use your magic powers!' },
  ];

  theme: StoryTheme = this.storyThemes[0];

  constructor(
    private scenarioService: ScenarioService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    const storyId = Number(this.route.snapshot.queryParamMap.get('storyId') || 0);
    this.theme = this.getTheme(storyId);
    this.loadScenario(id);
  }

  getTheme(storyId: number): StoryTheme {
    return this.storyThemes[storyId % this.storyThemes.length];
  }

  getHeroLeft(): string {
    const positions = [30, 120, 210, 280];
    return positions[Math.min(this.correctCount, positions.length - 1)] + 'px';
  }

  getProgressPercent(): number {
    return Math.round((this.currentScenarioIndex / this.totalScenarios) * 100);
  }

  loadScenario(id: number) {
    this.isLoading = true;
    this.scenarioService.getScenarioById(id).subscribe({
      next: (data) => {
        this.scenario = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Failed to load scenario.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }

  selectChoice(choice: ChoiceOptionDto) {
    if (this.result) return;
    this.selectedChoiceId = choice.id;
  }

  submitChoice() {
    if (!this.scenario || !this.selectedChoiceId) return;
    this.isSubmitting = true;
    this.scenarioService.submitChoice(this.scenario.id, {
      scenarioId: this.scenario.id,
      chosenChoiceId: this.selectedChoiceId
    }).subscribe({
      next: (res) => {
        this.result = res;
        if (res.isCorrect) this.correctCount++;
        this.currentScenarioIndex++;
        this.isSubmitting = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Failed to submit answer.';
        this.isSubmitting = false;
      }
    });
  }

  continueNext() {
    const nextId = this.result?.nextScenarioId;
    this.result = null;
    this.selectedChoiceId = null;

    if (nextId) {
      const storyId = Number(this.route.snapshot.queryParamMap.get('storyId') || 0);
      this.router.navigate(['/scenarios', nextId], { queryParams: { storyId } });
      this.loadScenario(nextId);
    } else {
      this.router.navigate(['/levels']);
    }
  }
}
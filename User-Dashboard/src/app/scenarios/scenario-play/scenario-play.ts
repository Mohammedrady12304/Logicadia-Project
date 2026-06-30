import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { NavBar } from '../../core/shared/nav-bar/nav-bar';
import { ScenarioService } from '../../core/services/scenario.service';
import { ScenarioPlayDto, ChoiceOptionDto, SubmitResultDto } from '../../core/models/scenario.model';

type GameState = 'playing' | 'feedback' | 'concept' | 'recap';

interface StoryTheme {
  hero: string;
  destination: string;
  bg: string;
  floor: string;
  topic: string;
  topicName: string;
  description: string;
  conceptExplanation: string;
}

@Component({
  selector: 'app-scenario-play',
  imports: [CommonModule, NavBar],
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
  correctCount = 0;
  totalScenarios = 2;
  currentScenarioIndex = 0;
  gameState: GameState = 'playing';

  private storyThemes: StoryTheme[] = [
    {
      hero: '🤖',
      destination: '🏠',
      bg: '#1a1654',
      floor: '#2d2a6e',
      topic: 'Sequences',
      topicName: 'SEQUENCE',
      description: 'Help Rex follow the steps in order!',
      conceptExplanation: 'A SEQUENCE means doing things in the right order, one step at a time — just like you helped Rex!'
    },
    {
      hero: '🧙‍♀️',
      destination: '🌸',
      bg: '#0a2e0a',
      floor: '#1a4e1a',
      topic: 'Loops',
      topicName: 'LOOP',
      description: 'Help Luna repeat her magic spell!',
      conceptExplanation: 'A LOOP means repeating the same action many times automatically — just like Luna watered all her flowers!'
    },
    {
      hero: '🧭',
      destination: '💎',
      bg: '#2e1a0a',
      floor: '#4e2d1a',
      topic: 'Conditions',
      topicName: 'CONDITION',
      description: 'Help Max choose the right path!',
      conceptExplanation: 'A CONDITION means checking IF something is true, THEN doing something — just like Max chose his path!'
    },
    {
      hero: '👩‍🍳',
      destination: '🍲',
      bg: '#2e0a0a',
      floor: '#4e1a1a',
      topic: 'Variables',
      topicName: 'VARIABLE',
      description: 'Help Zara store her ingredients!',
      conceptExplanation: 'A VARIABLE is like a magic box that stores information — just like Zara stored her ingredients!'
    },
    {
      hero: '👩‍🔬',
      destination: '⭐',
      bg: '#0a0a2e',
      floor: '#1a1a4e',
      topic: 'Functions',
      topicName: 'FUNCTION',
      description: 'Help Pip use her magic spells!',
      conceptExplanation: 'A FUNCTION is a reusable magic spell — give it a name and call it whenever you need it!'
    },
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
    this.theme = this.storyThemes[storyId % this.storyThemes.length];
    this.loadScenario(id);
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
    this.gameState = 'playing';
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
    if (this.gameState !== 'playing') return;
    this.selectedChoiceId = choice.id;
  }

  submitChoice() {
    if (!this.scenario || !this.selectedChoiceId) return;
    this.isSubmitting = true;

    this.scenarioService.submitChoice(this.scenario.id, {
      scenarioId: this.scenario.id,
      choiceId: this.selectedChoiceId!
    }).subscribe({
      next: (res) => {
        this.result = res;
        if (res.isCorrect) this.correctCount++;
        this.currentScenarioIndex++;
        this.isSubmitting = false;
        this.gameState = 'feedback';
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Failed to submit answer.';
        this.isSubmitting = false;
      }
    });
  }

  showConcept() {
    if (this.result?.isCorrect) {
      this.gameState = 'concept';
    } else {
      this.tryAgain();
    }
  }

  tryAgain() {
    this.result = null;
    this.selectedChoiceId = null;
    this.currentScenarioIndex--;
    this.gameState = 'playing';
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
      this.gameState = 'recap';
    }
  }

  goToLevels() {
    this.router.navigate(['/levels']);
  }
}
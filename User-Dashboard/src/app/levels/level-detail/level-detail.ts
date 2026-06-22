import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { LevelService } from '../../core/services/level.service';
import { LevelDetailDto, StoryDto } from '../../core/models/level.models';
@Component({
  selector: 'app-level-detail',
  imports: [CommonModule],
  templateUrl: './level-detail.html',
  styleUrl: './level-detail.css',
})
export class LevelDetail   implements OnInit{
  level: LevelDetailDto | null = null;
  isLoading = false;
  errorMessage = '';

  constructor(
    private levelService: LevelService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    const levelId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadLevel(levelId);
  }
loadLevel(levelId: number) {
    this.isLoading = true;
    this.levelService.getLevelById(levelId).subscribe({
      next: (data) => {
        this.level = data;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Level not found or not unlocked.';
        this.isLoading = false;
      }
    });
  }

  openStory(story: StoryDto) {
    this.router.navigate(['/stories', story.id]);
  }

  goBack() {
    this.router.navigate(['/levels']);
  }
}

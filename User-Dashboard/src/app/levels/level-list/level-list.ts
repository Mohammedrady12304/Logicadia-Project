import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { LevelService } from '../../core/services/level.service';
import { LevelDto } from '../../core/models/level.models';
import { NavBar } from '../../core/shared/nav-bar/nav-bar';
@Component({
  selector: 'app-level-list',
  standalone: true,
  imports: [CommonModule, NavBar],
  templateUrl: './level-list.html',
  styleUrl: './level-list.css'
})
export class LevelList implements OnInit {
  levels: any[] = [];
  isLoading = false;
  errorMessage = '';
  totalXp = 0;
  progressPercent = 0;

  constructor(private levelService: LevelService, private router: Router) {}

  ngOnInit() {
    this.loadLevels();
  }

  loadLevels() {
    this.isLoading = true;
    this.levelService.getAllLevels().subscribe({
      next: (data) => {
        console.log(data);
        this.levels = data;
        this.calculateProgress();
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Failed to load levels.';
        this.isLoading = false;
      }
    });
  }

  calculateProgress() {
    const unlockedCount = this.levels.filter(l => l.isUnlocked).length;
    this.progressPercent = this.levels.length > 0
      ? Math.round((unlockedCount / this.levels.length) * 100)
      : 0;
  }

  openLevel(level: LevelDto) {
    if (!level.isUnlocked) return;
    this.router.navigate(['/levels', level.id]);
  }

  getStatus(level: LevelDto, index: number): 'completed' | 'active' | 'locked' {
    if (!level.isUnlocked) return 'locked';
    const nextLocked = this.levels[index + 1] && !this.levels[index + 1].isUnlocked;
    const isLast = index === this.levels.length - 1;
    if (nextLocked || isLast) return 'active';
    return 'completed';
  }

  // إحداثيات المسار المتعرج لكل نقطة (نفس نمط الـ SVG)
  getNodePosition(index: number): { x: number; y: number } {
    const positions = [
      { x: 40, y: 60 },
      { x: 160, y: 130 },
      { x: 280, y: 200 },
      { x: 400, y: 130 },
      { x: 520, y: 60 }
    ];
    return positions[index % positions.length];
  }
}
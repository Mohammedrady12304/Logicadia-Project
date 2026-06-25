import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ParentService } from '../../core/services/parent.service';
import { AssignPathDto } from '../../core/models/parent.model';
import { LevelService } from '../../core/services/level.service';
import { LevelDto } from '../../core/models/level.models';

@Component({
  selector: 'app-assign-path',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './assign-path.html',
  styleUrl: './assign-path.css'
})
export class AssignPath implements OnInit {
  childId!: number;
  levels: LevelDto[] = [];
  isLoading = false;
  isSubmitting = false;
  successMessage = '';
  errorMessage = '';

  dto: AssignPathDto = {
    levelId: 0,
    storyId: undefined,
    scenarioId: undefined
  };

  constructor(
    private parentService: ParentService,
    private levelService: LevelService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.childId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadLevels();
  }

  loadLevels() {
    this.isLoading = true;
    this.levelService.getAllLevels().subscribe({
      next: (data) => {
        this.levels = data;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Failed to load levels.';
        this.isLoading = false;
      }
    });
  }

  submit() {
    if (!this.dto.levelId) {
      this.errorMessage = 'Please select a level.';
      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.parentService.assignPath(this.childId, this.dto).subscribe({
      next: (res) => {
        this.successMessage = res.message;
        this.isSubmitting = false;
      },
      error: () => {
        this.errorMessage = 'Failed to assign path.';
        this.isSubmitting = false;
      }
    });
  }

  goBack() {
    this.router.navigate(['/parent/child', this.childId, 'progress']);
  }
}
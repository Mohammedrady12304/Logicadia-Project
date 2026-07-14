import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ParentService } from '../../core/services/parent.service';
import { AssignPathDto } from '../../core/models/parent.model';

@Component({
  selector: 'app-assign-path',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './assign-path.html',
  styleUrl: './assign-path.css'
})
export class AssignPath implements OnInit {

  childId!: number;
  isLoading = false;
  isSubmitting = false;
  successMessage = '';
  errorMessage = '';

  dto: AssignPathDto = {
    age: 0,
    interests: '',
    favoriteColor: '',
    favoriteAnimal: '',
    learningTopic: '',
    readingLevel: '',
    preferredLanguage: ''
  };

  constructor(
    private parentService: ParentService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    this.childId = Number(this.route.snapshot.paramMap.get('id'));
  }

  private isFormValid(): boolean {
    if (!this.dto.age || this.dto.age <= 0) {
      this.errorMessage = 'Please enter a valid age.';
      return false;
    }
    if (!this.dto.interests?.trim()) {
      this.errorMessage = 'Please enter interests.';
      return false;
    }
    if (!this.dto.favoriteColor?.trim()) {
      this.errorMessage = 'Please enter favorite color.';
      return false;
    }
    if (!this.dto.favoriteAnimal?.trim()) {
      this.errorMessage = 'Please enter favorite animal.';
      return false;
    }
    if (!this.dto.learningTopic?.trim()) {
      this.errorMessage = 'Please enter learning topic.';
      return false;
    }
    if (!this.dto.readingLevel?.trim()) {
      this.errorMessage = 'Please select reading level.';
      return false;
    }
    if (!this.dto.preferredLanguage?.trim()) {
      this.errorMessage = 'Please select preferred language.';
      return false;
    }
    return true;
  }

  submit(): void {

    this.errorMessage = '';
    this.successMessage = '';

    if (!this.isFormValid()) {
      this.cdr.detectChanges();
      return;
    }

    this.isSubmitting = true;
    this.cdr.detectChanges();

    this.parentService.assignPath(this.childId, this.dto).subscribe({

      next: (res) => {
        this.successMessage = res.message || 'Learning path assigned successfully.';
        this.errorMessage = '';
        this.isSubmitting = false;
        this.cdr.detectChanges();
      },

      error: (err) => {
        this.errorMessage = err?.error?.message || 'Failed to assign path.';
        this.successMessage = '';
        this.isSubmitting = false;
        this.cdr.detectChanges();
      }

    });

  }

  goBack(): void {
    this.router.navigate(['/parent/child', this.childId, 'progress']);
  }

}
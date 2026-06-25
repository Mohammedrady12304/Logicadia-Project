import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ParentService } from '../../core/services/parent.service';
import { ChildProgressDetailsDto } from '../../core/models/parent.model';

@Component({
  selector: 'app-child-progress',
  imports: [CommonModule],
  templateUrl: './child-progress.html',
  styleUrl: './child-progress.css',
})
export class ChildProgress implements OnInit {

  progress: ChildProgressDetailsDto | null = null;
  isLoading = false;
  errorMessage = '';

  constructor(
    private parentService: ParentService,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}


  ngOnInit() {

    const childId = Number(
      this.route.snapshot.paramMap.get('id')
    );

    this.loadProgress(childId);

  }



  loadProgress(childId: number) {

    this.isLoading = true;


    this.parentService.getChildProgress(childId).subscribe({

      next: (data) => {

        this.progress = data;

        this.isLoading = false;


        this.cdr.detectChanges();

      },


      error: () => {

        this.errorMessage = 'Failed to load child progress.';

        this.isLoading = false;


        this.cdr.detectChanges();

      }


    });

  }



  assignPath() {

    this.router.navigate([
      '/parent/child',
      this.progress?.childId,
      'assign-path'
    ]);

  }



  goBack() {

    this.router.navigate([
      '/parent/children'
    ]);

  }

}
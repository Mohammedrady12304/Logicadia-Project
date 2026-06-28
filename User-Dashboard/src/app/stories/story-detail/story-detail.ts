import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { StorySevice } from '../../core/services/story.sevice';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { StoryDetailDto } from '../../core/models/story.model';

@Component({
  selector: 'app-story-detail',
  imports: [CommonModule],
  templateUrl: './story-detail.html',
  styleUrl: './story-detail.css',
})
export class StoryDetail implements OnInit {

  story: StoryDetailDto | null = null;
  isLoading = false;
  errorMessage = '';

  constructor(
    private storyService: StorySevice,
    private route: ActivatedRoute,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    const storyId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadStory(storyId);
  }

  loadStory(storyId:number){

    this.isLoading = true;

    this.storyService.getStoryById(storyId).subscribe({

      next:(data)=>{

        this.story = data;
        this.isLoading = false;

        this.cdr.detectChanges();

      },

      error:()=>{

        this.errorMessage = 'Story not found.';
        this.isLoading = false;

        this.cdr.detectChanges();

      }

    });

  }


  startStory() {
  if (!this.story || this.story.scenarios.length === 0) return;
  const firstScenario = this.story.scenarios[0];
  this.router.navigate(['/scenarios', firstScenario.id], { 
    queryParams: { storyId: this.story.id } 
  });
}

  goBack(){

    this.router.navigate([
      '/levels',
      this.story?.levelId
    ]);

  }

}
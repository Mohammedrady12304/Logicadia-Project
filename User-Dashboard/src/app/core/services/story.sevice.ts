import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StoryDetailDto } from '../models/story.model';
@Injectable({
  providedIn: 'root',
})
export class StorySevice {
  private apiUrl = 'https://localhost:44342/api/stories';

  constructor(private http: HttpClient) {}

  getStoryById(storyId: number): Observable<StoryDetailDto> {
    return this.http.get<StoryDetailDto>(`${this.apiUrl}/${storyId}`);
  }
}

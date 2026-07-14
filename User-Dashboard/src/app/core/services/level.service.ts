import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LevelDetailDto, LevelDto } from '../models/level.models';

@Injectable({
  providedIn: 'root',
})
export class LevelService {
  private apiUrl = 'https://localhost:44342/api/levels';

  constructor(private http: HttpClient) {}

  getAllLevels(): Observable<LevelDto[]> {
    return this.http.get<LevelDto[]>(this.apiUrl);
  }

  getLevelById(levelId: number): Observable<LevelDetailDto> {
    return this.http.get<LevelDetailDto>(`${this.apiUrl}/${levelId}`);
  }

  completeLevel(levelId: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${levelId}/complete`, {});
  }
}
import { Injectable } from '@angular/core';
import { HttpClient} from '@angular/common/http';
import { Observable } from 'rxjs';
import { LevelDetailDto ,LevelDto  } from '../models/level.models';

@Injectable({
  providedIn: 'root',
})
export class LevelService {
  private apiUrl = 'https://localhost:7213/api/levels';

  constructor(private http: HttpClient) {}

  getAllLevels(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }

  getLevelById(levelId: number): Observable<LevelDetailDto> {
    return this.http.get<LevelDetailDto>(`${this.apiUrl}/${levelId}`);
  }
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ScenarioPlayDto, SubmitChoiceDto, SubmitResultDto } from '../models/scenario.model';

@Injectable({
  providedIn: 'root',
})
export class ScenarioService {
  private apiUrl = 'https://localhost:44342/api/scenarios';

  constructor(private http: HttpClient) {}

  getScenarioById(scenarioId: number): Observable<ScenarioPlayDto> {
    return this.http.get<ScenarioPlayDto>(`${this.apiUrl}/${scenarioId}`);
  }

  submitChoice(scenarioId: number, dto: SubmitChoiceDto): Observable<SubmitResultDto> {
    return this.http.post<SubmitResultDto>(`${this.apiUrl}/${scenarioId}/submit`, dto);
}
}
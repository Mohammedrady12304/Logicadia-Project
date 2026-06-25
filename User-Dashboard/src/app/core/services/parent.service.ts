import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ChildSummaryDto , ChildProgressDetailsDto , AssignPathDto } from '../models/parent.model';
@Injectable({
  providedIn: 'root',
})
export class ParentService {
  private apiUrl = 'https://localhost:44342/api/ParentDashboard';

  constructor(private http: HttpClient) {}

  getChildren(): Observable<ChildSummaryDto[]> {
    return this.http.get<ChildSummaryDto[]>(`${this.apiUrl}/children`);
  }

  getChildProgress(childId: number): Observable<ChildProgressDetailsDto> {
    return this.http.get<ChildProgressDetailsDto>(`${this.apiUrl}/child/${childId}/progress`);
  }

  assignPath(childId: number, dto: AssignPathDto): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(`${this.apiUrl}/child/${childId}/assign-path`, dto);
  }
}

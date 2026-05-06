import { Injectable } from '@angular/core';

import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { Bug } from '../models/bug';

import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BugService {

  private apiUrl = `${environment.apiUrl}/Bug`;

  constructor(private http: HttpClient) { }

  // GET ALL BUGS
  getAllBugs(): Observable<any> {
    return this.http.get(this.apiUrl);
  }

  // GET BUG BY ID
  getBugById(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/${id}`);
  }

  // CREATE BUG
  createBug(bug: Bug): Observable<any> {
    return this.http.post(this.apiUrl, bug);
  }

  // UPDATE BUG
  updateBug(id: number, bug: Bug): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, bug);
  }

  // DELETE BUG
  deleteBug(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

  // GET BY STATUS
  getBugsByStatus(status: string): Observable<any> {
    return this.http.get(`${this.apiUrl}/status/${status}`);
  }
}

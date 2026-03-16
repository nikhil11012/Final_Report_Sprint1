import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

export interface Rating {
  ratingId: number;
  doctorId: number;
  userId: number;
  ratingValue: number;
  comment?: string;
  createdAtUtc: string;
}

export interface CreateRatingRequest {
  doctorId: number;
  ratingValue: number;
  comment?: string;
}

@Injectable({
  providedIn: 'root'
})
export class RatingService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/ratings`;

  getDoctorRatings(doctorId: number): Observable<Rating[]> {
    return this.http.get<Rating[]>(`${this.apiUrl}?doctorId=${doctorId}`);
  }

  submitRating(request: CreateRatingRequest): Observable<Rating> {
    return this.http.post<Rating>(this.apiUrl, request);
  }
}

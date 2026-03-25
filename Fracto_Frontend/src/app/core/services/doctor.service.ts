import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable, tap } from 'rxjs';

export interface Specialization {
  specializationId: number;
  name: string;
  description: string;
}

export interface DoctorListItem {
  doctorId: number;
  fullName: string;
  city: string;
  averageRating: number | null;
  isActive: boolean;
  specializationId: number;
  specializationName: string;
  profileImagePath?: string | null;
}

export interface DoctorSearchParams {
  city?: string | null;
  specializationId?: number | null;
  minRating?: number | null;
}

@Injectable({
  providedIn: 'root'
})
export class DoctorService {
  constructor(private http: HttpClient) {}

  getCities(): Observable<string[]> {
    return this.http.get<string[]>(`${environment.apiUrl}/doctors/cities`);
  }

  getSpecializations(): Observable<Specialization[]> {
    return this.http.get<Specialization[]>(`${environment.apiUrl}/specializations`);
  }

  getDoctors(params: DoctorSearchParams = {}): Observable<DoctorListItem[]> {
    let httpParams = new HttpParams();
    
    if (params.city) {
      httpParams = httpParams.set('city', params.city);
    }
    if (params.specializationId) {
      httpParams = httpParams.set('specializationId', params.specializationId.toString());
    }
    if (params.minRating) {
      httpParams = httpParams.set('minRating', params.minRating.toString());
    }

    return this.http.get<DoctorListItem[]>(`${environment.apiUrl}/doctors`, { params: httpParams });
  }

  getDoctorById(id: number): Observable<DoctorListItem> {
    return this.http.get<DoctorListItem>(`${environment.apiUrl}/doctors/${id}`);
  }
}

import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

export interface AdminStats {
  totalUsers: number;
  totalDoctors: number;
  totalAppointments: number;
  pendingAppointments: number;
}

export interface AdminUser {
  id: number;
  username: string;
  email: string;
  fullName: string;
  role: number;
  profileImagePath?: string;
  createdAtUtc: string;
}

export interface AdminAppointment {
  appointmentId: number;
  appointmentDate: string;
  timeSlot: string;
  status: number;
  userId: number;
  userName: string;
  userEmail: string;
  doctorId: number;
  doctorName: string;
  doctorCity: string;
  specializationName: string;
}

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/admin`;

  // Stats
  getStats(): Observable<AdminStats> {
    return this.http.get<AdminStats>(`${this.apiUrl}/stats`);
  }

  // Users
  getUsers(): Observable<AdminUser[]> {
    return this.http.get<AdminUser[]>(`${this.apiUrl}/users`);
  }

  updateUser(id: number, data: any): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/users/${id}`, data);
  }

  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/users/${id}`);
  }

  // Doctors
  getDoctors(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/doctors`);
  }

  getDoctorById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/doctors/${id}`);
  }

  createDoctor(data: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/doctors`, data);
  }

  updateDoctor(id: number, data: any): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/doctors/${id}`, data);
  }

  toggleDoctorActive(id: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/doctors/${id}/toggle-active`, {});
  }

  deleteDoctor(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/doctors/${id}`);
  }

  uploadDoctorImage(file: File): Observable<{ imagePath: string }> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<{ imagePath: string }>(`${this.apiUrl}/doctors/upload-image`, formData);
  }

  // Appointments
  getAppointments(params?: any): Observable<AdminAppointment[]> {
    return this.http.get<AdminAppointment[]>(`${this.apiUrl}/appointments`, { params });
  }

  approveAppointment(id: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/appointments/${id}/approve`, {});
  }

  completeAppointment(id: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/appointments/${id}/complete`, {});
  }

  cancelAppointment(id: number): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/appointments/${id}/cancel`, {});
  }

  // Common / Specializations
  getSpecializations(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/specializations`);
  }

  createSpecialization(data: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/specializations`, data);
  }

  updateSpecialization(id: number, data: any): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/specializations/${id}`, data);
  }

  deleteSpecialization(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/specializations/${id}`);
  }
}

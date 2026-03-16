import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable } from 'rxjs';

export interface AppointmentDto {
  appointmentId: number;
  appointmentDate: string; // ISO Date string (YYYY-MM-DD)
  timeSlot: string; // "HH:mm-HH:mm"
  status: number; // 0=Booked, 1=Completed, 2=Cancelled
  doctorId: number;
  doctorName: string;
  doctorCity: string;
  specializationName: string;
}

export interface BookAppointmentRequest {
  doctorId: number;
  appointmentDate: string; // "YYYY-MM-DD"
  timeSlot: string;
}

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  constructor(private http: HttpClient) {}

  getAvailableSlots(doctorId: number, date: string): Observable<string[]> {
    return this.http.get<string[]>(`${environment.apiUrl}/doctors/${doctorId}/available-slots?date=${date}`);
  }

  bookAppointment(request: BookAppointmentRequest): Observable<AppointmentDto> {
    return this.http.post<AppointmentDto>(`${environment.apiUrl}/appointments`, request);
  }

  getMyAppointments(): Observable<AppointmentDto[]> {
    return this.http.get<AppointmentDto[]>(`${environment.apiUrl}/appointments/my`);
  }

  cancelAppointment(appointmentId: number): Observable<void> {
    return this.http.delete<void>(`${environment.apiUrl}/appointments/${appointmentId}`);
  }
}

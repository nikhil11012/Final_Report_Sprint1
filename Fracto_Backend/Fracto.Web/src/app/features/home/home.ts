import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { DoctorService, DoctorListItem, Specialization } from '../../core/services/doctor.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  private doctorService = inject(DoctorService);
  private router = inject(Router);
  env = environment;

  cities: string[] = [];
  specializations: Specialization[] = [];
  doctors: DoctorListItem[] = [];

  // Search state
  selectedCity: string = '';
  selectedSpecializationId: number | null = null;
  minRating: number | null = null;
  searchDate: string = ''; // For requirement 2
  
  isLoading = false;

  ngOnInit(): void {
    this.loadFilterData();
    // Set default date to today for the date picker
    const today = new Date();
    this.searchDate = today.toISOString().split('T')[0];
    // Initially load all active doctors
    this.searchDoctors();
  }

  loadFilterData() {
    this.doctorService.getCities().subscribe({
      next: (data) => this.cities = data,
      error: (err) => console.error('Failed to load cities', err)
    });

    this.doctorService.getSpecializations().subscribe({
      next: (data) => this.specializations = data,
      error: (err) => console.error('Failed to load specializations', err)
    });
  }

  searchDoctors() {
    this.isLoading = true;
    this.doctorService.getDoctors({
      city: this.selectedCity || null,
      specializationId: this.selectedSpecializationId || null,
      minRating: this.minRating || null
    }).subscribe({
      next: (data) => {
        this.doctors = data;
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to search doctors', err);
        this.isLoading = false;
      }
    });
  }

  bookAppointment(doctorId: number) {
    // Navigate to doctor details passing the selected date
    this.router.navigate(['/doctors', doctorId], { 
      queryParams: { date: this.searchDate } 
    });
  }
}


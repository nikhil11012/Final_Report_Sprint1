import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DoctorService, DoctorListItem } from '../../../core/services/doctor.service';
import { AppointmentService } from '../../../core/services/appointment.service';
import { AuthService } from '../../../core/services/auth.service';
import { AppointmentDto } from '../../../core/services/appointment.service';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-doctor-details',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './doctor-details.html',
  styleUrl: './doctor-details.css',
})
export class DoctorDetails implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private doctorService = inject(DoctorService);
  private appointmentService = inject(AppointmentService);
  private authService = inject(AuthService);
  env = environment;

  doctorId!: number;
  doctor: DoctorListItem | null = null;
  
  selectedDate: string = '';
  availableSlots: string[] = [];
  selectedSlot: string = '';
  
  isLoadingDoctor = true;
  isLoadingSlots = false;
  isBooking = false;
  bookingError = '';

  ngOnInit(): void {
    // 1. Get the Doctor ID from the URL path (/doctors/:id)
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.doctorId = +idParam;
      this.loadDoctorDetails();
    }

    // 2. Get the date from the query params (?date=YYYY-MM-DD) or default to today
    this.route.queryParams.subscribe(params => {
      this.selectedDate = params['date'] || new Date().toISOString().split('T')[0];
      if (this.doctorId) {
        this.loadAvailableSlots();
      }
    });
  }

  loadDoctorDetails() {
    this.isLoadingDoctor = true;
    this.doctorService.getDoctorById(this.doctorId).subscribe({
      next: (data) => {
        this.doctor = data;
        this.isLoadingDoctor = false;
      },
      error: (err) => {
        console.error('Failed to load doctor', err);
        this.isLoadingDoctor = false;
      }
    });
  }

  loadAvailableSlots() {
    if (!this.selectedDate) return;
    
    this.isLoadingSlots = true;
    this.selectedSlot = ''; // Reset selected slot when date changes
    
    this.appointmentService.getAvailableSlots(this.doctorId, this.selectedDate).subscribe({
      next: (slots) => {
        this.availableSlots = slots;
        this.isLoadingSlots = false;
      },
      error: (err) => {
        console.error('Failed to load slots', err);
        this.isLoadingSlots = false;
      }
    });
  }

  onDateChange() {
    // Update the URL query parameter without reloading the page
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { date: this.selectedDate },
      queryParamsHandling: 'merge',
    });
    // The queryParams.subscribe in ngOnInit will trigger loadAvailableSlots
  }

  selectSlot(slot: string) {
    this.selectedSlot = slot;
    this.bookingError = '';
  }

  bookAppointment() {
    if (!this.selectedSlot) {
      this.bookingError = 'Please select a time slot.';
      return;
    }

    // Check if user is logged in
    if (!this.authService.currentUser()) {
      // Redirect to login, heavily hinting they should come back
      this.router.navigate(['/login'], { queryParams: { returnUrl: this.router.url } });
      return;
    }

    this.isBooking = true;
    this.bookingError = '';

    this.appointmentService.bookAppointment({
      doctorId: this.doctorId,
      appointmentDate: this.selectedDate,
      timeSlot: this.selectedSlot
    }).subscribe({
      next: (appointment: AppointmentDto) => {
        this.isBooking = false;
        this.router.navigate(['/booking-confirmation', appointment.appointmentId], {
          state: { appointment: appointment }
        });
      },
      error: (err) => {
        console.error('Booking failed', err);
        this.isBooking = false;
        this.bookingError = err.error?.message || 'Failed to book appointment. The slot might be taken.';
      }
    });
  }
}


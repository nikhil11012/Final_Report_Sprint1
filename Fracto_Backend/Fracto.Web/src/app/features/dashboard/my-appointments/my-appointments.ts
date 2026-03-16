import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AppointmentService, AppointmentDto } from '../../../core/services/appointment.service';
import { RatingService } from '../../../core/services/rating.service';

@Component({
  selector: 'app-my-appointments',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './my-appointments.html',
  styleUrl: './my-appointments.css',
})
export class MyAppointments implements OnInit {
  private appointmentService = inject(AppointmentService);
  private ratingService = inject(RatingService);
  
  appointments: AppointmentDto[] = [];
  isLoading = true;
  error = '';

  // Rating Modal state
  showRatingModal = false;
  selectedDoctorId: number | null = null;
  selectedDoctorName: string = '';
  ratingValue: number = 5;
  ratingComment: string = '';
  isSubmittingRating = false;

  ngOnInit(): void {
    this.loadAppointments();
  }

  loadAppointments() {
    this.isLoading = true;
    this.error = '';
    this.appointmentService.getMyAppointments().subscribe({
      next: (data) => {
        // Sort by date descending
        this.appointments = data.sort((a, b) => new Date(b.appointmentDate).getTime() - new Date(a.appointmentDate).getTime());
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Failed to load appointments', err);
        this.error = 'Failed to load your appointments. Please try again.';
        this.isLoading = false;
      }
    });
  }

  cancelAppointment(id: number) {
    if (confirm('Are you sure you want to cancel this appointment?')) {
      this.appointmentService.cancelAppointment(id).subscribe({
        next: () => {
          // Update status in list
          const apt = this.appointments.find(a => a.appointmentId === id);
          if (apt) apt.status = 4; // Cancelled
          alert('Appointment cancelled successfully.');
        },
        error: (err) => {
          console.error('Failed to cancel', err);
          alert('Failed to cancel appointment.');
        }
      });
    }
  }

  openRatingModal(doctorId: number, doctorName: string) {
    this.selectedDoctorId = doctorId;
    this.selectedDoctorName = doctorName;
    this.ratingValue = 5;
    this.ratingComment = '';
    this.showRatingModal = true;
  }

  closeRatingModal() {
    this.showRatingModal = false;
    this.selectedDoctorId = null;
  }

  setRating(value: number) {
    this.ratingValue = value;
  }

  submitRating() {
    if (!this.selectedDoctorId) return;

    this.isSubmittingRating = true;
    this.ratingService.submitRating({
      doctorId: this.selectedDoctorId,
      ratingValue: this.ratingValue,
      comment: this.ratingComment
    }).subscribe({
      next: () => {
        alert('Thank you for your feedback!');
        this.isSubmittingRating = false;
        this.closeRatingModal();
      },
      error: (err) => {
        console.error('Failed to submit rating', err);
        alert('Failed to submit rating. Please try again.');
        this.isSubmittingRating = false;
      }
    });
  }

  getStatusClass(status: number): string {
    switch(status) {
      case 1: return 'bg-yellow-100 text-yellow-800'; // Booked/Pending
      case 2: return 'bg-green-100 text-green-800';   // Approved
      case 3: return 'bg-blue-100 text-blue-800';     // Completed
      case 4: return 'bg-red-100 text-red-800';      // Cancelled
      default: return 'bg-gray-100 text-gray-800';
    }
  }

  getStatusLabel(status: number): string {
    switch(status) {
      case 1: return 'Pending Approval';
      case 2: return 'Confirmed';
      case 3: return 'Completed';
      case 4: return 'Cancelled';
      default: return 'Unknown';
    }
  }
}


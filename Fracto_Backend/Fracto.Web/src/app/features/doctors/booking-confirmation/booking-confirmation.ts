import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AppointmentDto } from '../../../core/services/appointment.service';

@Component({
  selector: 'app-booking-confirmation',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './booking-confirmation.html',
  styleUrl: './booking-confirmation.css',
})
export class BookingConfirmation implements OnInit {
  private router = inject(Router);
  appointment: AppointmentDto | null = null;

  ngOnInit(): void {
    // Retrieve the appointment data passed via router state
    const navigation = this.router.getCurrentNavigation();
    if (navigation?.extras.state && navigation.extras.state['appointment']) {
      this.appointment = navigation.extras.state['appointment'];
    } else {
      // Fallback if accessed directly or state is lost (e.g. reload)
      // For a robust app, you might fetch it from an API here using the ID from the URL.
      // For this phase, we'll just show a generic success message or redirect.
      const stateHistory = history.state;
      if (stateHistory && stateHistory['appointment']) {
        this.appointment = stateHistory['appointment'];
      }
    }
  }
}


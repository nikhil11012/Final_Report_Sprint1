import { Component, inject, effect, OnInit } from '@angular/core';
import { RouterLink, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { NotificationService } from '../../services/notification.service';
import { environment } from '../../../../environments/environment';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [RouterLink, CommonModule],
  templateUrl: './header.html',
  styleUrl: './header.css'
})
export class Header implements OnInit {
  authService = inject(AuthService);
  notificationService = inject(NotificationService);
  router = inject(Router);
  env = environment;

  showNotifications = false;

  constructor() {
    // Watch for login/logout to start/stop SignalR
    effect(() => {
      if (this.authService.currentUser()) {
        this.notificationService.startConnection();
      } else {
        this.notificationService.stopConnection();
      }
    });
  }

  ngOnInit(): void {
    // Session is restored automatically by AuthService
  }

  toggleNotifications() {
    this.showNotifications = !this.showNotifications;
    if (this.showNotifications) {
      this.notificationService.clearUnread();
    }
  }

  logout() {
    this.authService.logout();
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      const file = input.files[0];
      this.authService.uploadProfileImage(file).subscribe({
        next: () => {
          console.log('Profile image uploaded successfully');
        },
        error: (err) => {
          console.error('Failed to upload profile image', err);
        }
      });
    }
  }
}

import { Injectable, signal, inject } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private authService = inject(AuthService);
  private hubConnection?: signalR.HubConnection;
  private connectionState: 'disconnected' | 'connecting' | 'connected' = 'disconnected';

  // Reactive state for notifications
  notifications = signal<string[]>([]);
  unreadCount = signal<number>(0);

  constructor() {}

  async startConnection() {
    if (this.connectionState !== 'disconnected') return;
    
    const token = this.authService.getToken();
    if (!token) return;

    this.connectionState = 'connecting';

    // If there's an old connection, stop it first
    if (this.hubConnection) {
      await this.hubConnection.stop();
    }

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.serverUrl}/hubs/notifications`, {
        accessTokenFactory: () => token,
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on('ReceiveNotification', (message: string) => {
      console.log('🔔 User Notification Received:', message);
      this.addNotification(message);
    });

    this.hubConnection.on('ReceiveAdminNotification', (message: string) => {
      console.log('🛡️ Admin Notification Received:', message);
      this.addNotification(`[ADMIN] ${message}`);
    });

    try {
      await this.hubConnection.start();
      this.connectionState = 'connected';
      console.log('SignalR Connected');
    } catch (err) {
      this.connectionState = 'disconnected';
      console.error('SignalR Error: ', err);
    }
  }

  async stopConnection() {
    if (this.hubConnection) {
      await this.hubConnection.stop();
      this.connectionState = 'disconnected';
      console.log('SignalR Disconnected');
    }
  }

  private addNotification(message: string) {
    this.notifications.update(prev => [message, ...prev.slice(0, 9)]); // Keep last 10
    this.unreadCount.update(count => count + 1);
    
    // show browser notification or toast here if needed
    if ('Notification' in window && (Notification as any).permission === 'granted') {
      new Notification('Fracto Alert', { body: message });
    }
  }

  clearUnread() {
    this.unreadCount.set(0);
  }

  removeNotification(index: number) {
    this.notifications.update(prev => prev.filter((_, i) => i !== index));
  }
}

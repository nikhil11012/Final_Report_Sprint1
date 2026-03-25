import { Injectable, signal } from '@angular/core';
import { HttpClient, HttpBackend } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Observable, tap } from 'rxjs';
import { Router } from '@angular/router';

export interface AuthResponse {
  token: string;
  expiresAtUtc: string;
  userId: number;
  username: string;
  email: string;
  role: number; // 1 = User, 2 = Admin
  profileImagePath?: string | null;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'fracto_token';
  private readonly ROLE_KEY = 'fracto_role';

  // Signals for reactive state
  currentUser = signal<{ username: string; role: number; profileImagePath?: string | null } | null>(null);

  private httpBackendClient: HttpClient;

  constructor(private http: HttpClient, private router: Router, private handler: HttpBackend) {
    this.httpBackendClient = new HttpClient(this.handler); // Bypass interceptors
    this.restoreSession();
  }

  login(credentials: { identifier: string; password: string }): Observable<AuthResponse> {
    const url = `${environment.apiUrl}/auth/login`;
    console.log('🚀 Login Attempt:', { url, identifier: credentials.identifier });
    return this.http.post<AuthResponse>(url, credentials).pipe(
      tap({
        next: (res) => console.log('✅ Login Success:', res.username),
        error: (err) => console.error('❌ Login Error:', err)
      }),
      tap(res => this.setSession(res))
    );
  }

  register(data: any): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/register`, data).pipe(
      tap(res => this.setSession(res))
    );
  }

  uploadProfileImage(file: File): Observable<{ profileImagePath: string }> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<{ profileImagePath: string }>(`${environment.apiUrl}/users/profile-image`, formData)
      .pipe(
        tap(res => {
          const user = this.currentUser();
          if (user) {
            this.currentUser.set({ ...user, profileImagePath: res.profileImagePath });
          }
        })
      );
  }

  logout() {
    sessionStorage.removeItem(this.TOKEN_KEY);
    sessionStorage.removeItem(this.ROLE_KEY);
    this.currentUser.set(null);
    this.router.navigate(['/login']);
  }

  getToken(): string | null {
    return sessionStorage.getItem(this.TOKEN_KEY);
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  isAdmin(): boolean {
    const role = sessionStorage.getItem(this.ROLE_KEY);
    return role === '2'; // Admin role is 2
  }

  private setSession(authResult: AuthResponse) {
    sessionStorage.setItem(this.TOKEN_KEY, authResult.token);
    sessionStorage.setItem(this.ROLE_KEY, authResult.role.toString());
    this.currentUser.set({ 
      username: authResult.username, 
      role: authResult.role,
      profileImagePath: authResult.profileImagePath
    });
  }

  private restoreSession() {
    const token = this.getToken();
    const role = sessionStorage.getItem(this.ROLE_KEY);
    
    if (token && role) {
      // Temporarily set basic info so the UI doesn't flash logged out
      this.currentUser.set({ username: 'Loading...', role: parseInt(role, 10) });

      // Fetch fresh profile from the backend using the bypass client
      // We must manually attach the Authorization header because the interceptor is bypassed
      this.httpBackendClient.get<any>(`${environment.apiUrl}/Auth/me`, {
        headers: { Authorization: `Bearer ${token}` }
      }).subscribe({
        next: (user) => {
          this.currentUser.set({ 
            username: user.username, 
            role: user.role,
            profileImagePath: user.profileImagePath
          });
        },
        error: () => {
          this.logout(); // If the token is invalid or expired, log them out
        }
      });
    }
  }
}

import { Routes } from '@angular/router';
import { authGuard, adminGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', loadComponent: () => import('./features/home/home').then(m => m.Home) },
  { path: 'doctors/:id', loadComponent: () => import('./features/doctors/doctor-details/doctor-details').then(m => m.DoctorDetails) },
  { path: 'booking-confirmation/:id', loadComponent: () => import('./features/doctors/booking-confirmation/booking-confirmation').then(m => m.BookingConfirmation) },
  { path: 'my-appointments', loadComponent: () => import('./features/dashboard/my-appointments/my-appointments').then(m => m.MyAppointments), canActivate: [authGuard] },
  { path: 'admin', loadComponent: () => import('./features/admin/admin-dashboard/admin-dashboard').then(m => m.AdminDashboard), canActivate: [adminGuard] },
  { path: 'login', loadComponent: () => import('./features/auth/login/login').then(m => m.Login) },
  { path: 'register', loadComponent: () => import('./features/auth/register/register').then(m => m.Register) },
];

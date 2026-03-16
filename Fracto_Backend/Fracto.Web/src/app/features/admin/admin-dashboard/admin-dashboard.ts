import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AdminService, AdminStats, AdminUser, AdminAppointment } from '../../../core/services/admin.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css'
})
export class AdminDashboard implements OnInit {
  private adminService = inject(AdminService);

  activeTab: 'stats' | 'users' | 'doctors' | 'appointments' | 'specializations' = 'stats';
  
  // Data
  stats?: AdminStats;
  users: AdminUser[] = [];
  doctors: any[] = [];
  appointments: AdminAppointment[] = [];
  specializations: any[] = [];
  
  isLoading = true;
  error = '';

  // User Edit Form
  showUserModal = false;
  userForm: any = { id: 0, fullName: '', email: '', role: 1 };

  // Doctor Form
  showDoctorModal = false;
  isEditingDoctor = false;
  isUploadingDoctorImage = false;
  doctorForm: any = { doctorId: 0, fullName: '', city: '', specializationId: 0, isActive: true, profileImagePath: '' };

  // Specialization Form
  showSpecModal = false;
  isEditingSpec = false;
  specForm: any = { specializationId: 0, name: '', description: '' };

  ngOnInit(): void {
    console.log('AdminDashboard initialized');
    this.loadStats();
    this.loadSpecializations();
  }

  // --- Image Upload ---
  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.isUploadingDoctorImage = true;
      this.adminService.uploadDoctorImage(file).subscribe({
        next: (res) => {
          this.doctorForm.profileImagePath = res.imagePath;
          this.isUploadingDoctorImage = false;
        },
        error: () => {
          alert('Failed to upload image.');
          this.isUploadingDoctorImage = false;
        }
      });
    }
  }

  setTab(tab: 'stats' | 'users' | 'doctors' | 'appointments' | 'specializations') {
    this.activeTab = tab;
    this.error = '';
    
    if (tab === 'stats') this.loadStats();
    if (tab === 'users') this.loadUsers();
    if (tab === 'doctors') this.loadDoctors();
    if (tab === 'appointments') this.loadAppointments();
    if (tab === 'specializations') this.loadSpecializations();
  }

  loadSpecializations() {
    this.adminService.getSpecializations().subscribe({
      next: (res) => this.specializations = res,
      error: () => console.error('Failed to load specializations')
    });
  }

  // --- Specializations ---
  openAddSpec() {
    this.isEditingSpec = false;
    this.specForm = { specializationId: 0, name: '', description: '' };
    this.showSpecModal = true;
  }

  openEditSpec(spec: any) {
    this.isEditingSpec = true;
    this.specForm = { ...spec };
    this.showSpecModal = true;
  }

  saveSpec() {
    const data = {
      name: this.specForm.name,
      description: this.specForm.description
    };

    if (this.isEditingSpec) {
      this.adminService.updateSpecialization(this.specForm.specializationId, data).subscribe({
        next: () => {
          alert('Specialization updated successfully.');
          this.showSpecModal = false;
          this.loadSpecializations();
        },
        error: () => alert('Failed to update specialization.')
      });
    } else {
      this.adminService.createSpecialization(data).subscribe({
        next: () => {
          alert('Specialization added successfully.');
          this.showSpecModal = false;
          this.loadSpecializations();
        },
        error: () => alert('Failed to add specialization.')
      });
    }
  }

  deleteSpec(id: number) {
    if (confirm('Are you sure you want to delete this specialization?')) {
      this.adminService.deleteSpecialization(id).subscribe({
        next: () => {
          this.specializations = this.specializations.filter(s => s.specializationId !== id);
        },
        error: (err: any) => alert(err.error?.message || 'Failed to delete specialization.')
      });
    }
  }

  // --- Stats ---
  loadStats() {
    this.isLoading = true;
    this.adminService.getStats().subscribe({
      next: (res) => {
        this.stats = res;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load statistics.';
        this.isLoading = false;
      }
    });
  }

  // --- Users ---
  loadUsers() {
    this.isLoading = true;
    this.adminService.getUsers().subscribe({
      next: (res) => {
        this.users = res;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load users.';
        this.isLoading = false;
      }
    });
  }

  openEditUser(user: AdminUser) {
    this.userForm = { ...user };
    this.showUserModal = true;
  }

  saveUser() {
    this.adminService.updateUser(this.userForm.id, {
      fullName: this.userForm.fullName,
      email: this.userForm.email,
      role: Number(this.userForm.role)
    }).subscribe({
      next: () => {
        alert('User updated successfully.');
        this.showUserModal = false;
        this.loadUsers();
      },
      error: () => alert('Failed to update user.')
    });
  }

  deleteUser(id: number) {
    if (confirm('Are you sure you want to delete this user? This action cannot be undone.')) {
      this.adminService.deleteUser(id).subscribe({
        next: () => {
          this.users = this.users.filter(u => u.id !== id);
        },
        error: () => alert('Failed to delete user.')
      });
    }
  }

  // --- Doctors ---
  loadDoctors() {
    this.isLoading = true;
    this.adminService.getDoctors().subscribe({
      next: (res) => {
        this.doctors = res;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load doctors.';
        this.isLoading = false;
      }
    });
  }

  openAddDoctor() {
    this.isEditingDoctor = false;
    this.doctorForm = { doctorId: 0, fullName: '', city: '', specializationId: this.specializations[0]?.specializationId || 0, isActive: true, profileImagePath: '' };
    this.showDoctorModal = true;
  }

  openEditDoctor(doctor: any) {
    this.isEditingDoctor = true;
    const spec = this.specializations.find(s => s.name === doctor.specializationName);
    this.doctorForm = { 
      ...doctor, 
      specializationId: spec?.specializationId || 0 
    };
    this.showDoctorModal = true;
  }

  saveDoctor() {
    const data = {
      fullName: this.doctorForm.fullName,
      city: this.doctorForm.city,
      specializationId: Number(this.doctorForm.specializationId),
      isActive: this.doctorForm.isActive,
      profileImagePath: this.doctorForm.profileImagePath
    };

    if (this.isEditingDoctor) {
      this.adminService.updateDoctor(this.doctorForm.doctorId, data).subscribe({
        next: () => {
          alert('Doctor updated successfully.');
          this.showDoctorModal = false;
          this.loadDoctors();
        },
        error: () => alert('Failed to update doctor.')
      });
    } else {
      this.adminService.createDoctor(data).subscribe({
        next: () => {
          alert('Doctor added successfully.');
          this.showDoctorModal = false;
          this.loadDoctors();
        },
        error: () => alert('Failed to add doctor.')
      });
    }
  }

  toggleDoctor(id: number) {
    this.adminService.toggleDoctorActive(id).subscribe({
      next: () => {
        const doc = this.doctors.find(d => d.doctorId === id);
        if (doc) doc.isActive = !doc.isActive;
      },
      error: () => alert('Failed to toggle status.')
    });
  }

  deleteDoctor(id: number) {
    if (confirm('Are you sure you want to delete this doctor? This action cannot be undone.')) {
      this.adminService.deleteDoctor(id).subscribe({
        next: () => {
          this.doctors = this.doctors.filter(d => d.doctorId !== id);
        },
        error: (err: any) => alert(err.error?.message || 'Failed to delete doctor.')
      });
    }
  }

  // --- Appointments ---
  loadAppointments() {
    this.isLoading = true;
    this.adminService.getAppointments().subscribe({
      next: (res) => {
        this.appointments = res;
        this.isLoading = false;
      },
      error: () => {
        this.error = 'Failed to load appointments.';
        this.isLoading = false;
      }
    });
  }

  approveApt(id: number) {
    this.adminService.approveAppointment(id).subscribe({
      next: () => {
        const apt = this.appointments.find(a => a.appointmentId === id);
        if (apt) apt.status = 2; // Approved
      },
      error: () => alert('Failed to approve.')
    });
  }

  completeApt(id: number) {
    this.adminService.completeAppointment(id).subscribe({
      next: () => {
        const apt = this.appointments.find(a => a.appointmentId === id);
        if (apt) apt.status = 3; // Completed
      },
      error: () => alert('Failed to complete.')
    });
  }

  cancelApt(id: number) {
    if (confirm('Are you sure you want to cancel this appointment?')) {
      this.adminService.cancelAppointment(id).subscribe({
        next: () => {
          const apt = this.appointments.find(a => a.appointmentId === id);
          if (apt) apt.status = 4; // Cancelled
        },
        error: () => alert('Failed to cancel.')
      });
    }
  }

  getStatusLabel(status: number): string {
    switch (status) {
      case 1: return 'Pending';
      case 2: return 'Approved';
      case 3: return 'Completed';
      case 4: return 'Cancelled';
      default: return 'Unknown';
    }
  }

  getStatusClass(status: number): string {
    switch (status) {
      case 1: return 'bg-yellow-100 text-yellow-800';
      case 2: return 'bg-green-100 text-green-800';
      case 3: return 'bg-blue-100 text-blue-800';
      case 4: return 'bg-red-100 text-red-800';
      default: return 'bg-gray-100 text-gray-800';
    }
  }
}

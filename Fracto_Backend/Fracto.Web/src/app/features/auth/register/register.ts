import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './register.html',
})
export class Register {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  registerForm = this.fb.group({
    fullName: ['', Validators.required],
    username: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  errorMessage = '';
  isLoading = false;

  onSubmit() {
    if (this.registerForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';
      
      const userData = {
        fullName: this.registerForm.value.fullName!,
        username: this.registerForm.value.username!,
        email: this.registerForm.value.email!,
        password: this.registerForm.value.password!,
        role: 1 // 1 for User, 2 for Admin (hardcoded to User for open registration)
      };

      this.authService.register(userData).subscribe({
        next: () => {
          this.router.navigate(['/']); // Redirect to home on successful register
        },
        error: (err) => {
          this.isLoading = false;
          this.errorMessage = err.error?.message || 'Registration failed. Please try again.';
        }
      });
    } else {
      this.registerForm.markAllAsTouched();
    }
  }
}

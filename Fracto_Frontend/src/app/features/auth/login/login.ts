import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './login.html',
})
export class Login {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  loginForm = this.fb.group({
    identifier: ['', Validators.required],
    password: ['', Validators.required],
    remember: [false]
  });

  errorMessage = '';
  isLoading = false;

  onSubmit() {
    if (this.loginForm.valid) {
      this.isLoading = true;
      this.errorMessage = '';
      
      const credentials = {
        identifier: this.loginForm.value.identifier!,
        password: this.loginForm.value.password!
      };

      this.authService.login(credentials).subscribe({
        next: () => {
          this.router.navigate(['/']);
        },
        error: (err) => {
          this.isLoading = false;
          this.errorMessage = err.error?.message || 'Login failed. Please verify your credentials.';
        }
      });
    } else {
      this.loginForm.markAllAsTouched();
    }
  }
}

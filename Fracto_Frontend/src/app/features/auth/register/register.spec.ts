import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { Router, provideRouter } from '@angular/router';
import { of } from 'rxjs';
import { Register } from './register';
import { AuthService } from '../../../core/services/auth.service';

describe('Register Component', () => {
  let component: Register;
  let fixture: ComponentFixture<Register>;
  let mockAuthService: any;

  beforeEach(async () => {
    mockAuthService = {
      register: (userData: any) => of({})
    };

    await TestBed.configureTestingModule({
      imports: [Register, ReactiveFormsModule],
      providers: [
        { provide: AuthService, useValue: mockAuthService },
        provideRouter([])
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Register);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should invalidate empty form', () => {
    component.registerForm.controls.fullName.setValue('');
    component.registerForm.controls.email.setValue('');
    expect(component.registerForm.valid).toBe(false);
  });

  it('should validate complete form', () => {
    component.registerForm.controls.fullName.setValue('Test User');
    component.registerForm.controls.username.setValue('testuser');
    component.registerForm.controls.email.setValue('test@test.com');
    component.registerForm.controls.password.setValue('password123');
    expect(component.registerForm.valid).toBe(true);
  });
});

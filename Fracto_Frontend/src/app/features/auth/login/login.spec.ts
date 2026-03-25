import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { Router, provideRouter } from '@angular/router';
import { of, throwError } from 'rxjs';
import { Login } from './login';
import { AuthService } from '../../../core/services/auth.service';

describe('Login Component', () => {
  let component: Login;
  let fixture: ComponentFixture<Login>;
  let mockAuthService: any;

  beforeEach(async () => {
    mockAuthService = {
      login: (credentials: any) => of({})
    };

    await TestBed.configureTestingModule({
      imports: [Login, ReactiveFormsModule],
      providers: [
        { provide: AuthService, useValue: mockAuthService },
        provideRouter([])
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Login);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should invalidate empty form', () => {
    component.loginForm.controls.identifier.setValue('');
    component.loginForm.controls.password.setValue('');
    expect(component.loginForm.valid).toBe(false);
  });

  it('should validate complete form', () => {
    component.loginForm.controls.identifier.setValue('testuser');
    component.loginForm.controls.password.setValue('password123');
    expect(component.loginForm.valid).toBe(true);
  });

  it('should call authService internally when form is valid on submit', () => {
    component.loginForm.controls.identifier.setValue('testuser');
    component.loginForm.controls.password.setValue('password123');
    let called = false;
    mockAuthService.login = () => {
      called = true;
      return of({});
    };
    component.onSubmit();
    expect(called).toBe(true);
    expect(component.isLoading).toBe(true);
  });
});

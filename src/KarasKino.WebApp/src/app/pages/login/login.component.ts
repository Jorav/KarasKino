import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';

type LoginStep = 'email' | 'password' | 'register' | 'google-only';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  step: LoginStep = 'email';
  lockedEmail = '';
  isLoading = false;
  error: string | null = null;

  emailForm: FormGroup;
  passwordForm: FormGroup;
  registerForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
  ) {
    this.emailForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });

    this.passwordForm = this.fb.group({
      password: ['', Validators.required]
    });

    this.registerForm = this.fb.group({
      password: ['', Validators.required],
      confirmPassword: ['', Validators.required]
    }, { validators: this.passwordsMatch });
  }

  passwordsMatch(group: FormGroup): { [key: string]: boolean } | null {
    const password = group.get('password')?.value;
    const confirm = group.get('confirmPassword')?.value;
    return password === confirm ? null : { mismatch: true };
  }

  continueWithEmail(): void {
    if (this.emailForm.invalid || this.isLoading) return;
    this.isLoading = true;
    this.error = null;

    const email = this.emailForm.get('email')?.value;

    this.auth.checkEmail(email).subscribe({
      next: (result) => {
        this.lockedEmail = email;
        this.isLoading = false;

        if (!result.exists) {
          this.step = 'register';
        } else if (result.hasPassword) {
          this.step = 'password';
        } else {
          this.step = 'google-only';
        }
      },
      error: () => {
        this.isLoading = false;
        this.error = 'Something went wrong. Please try again.';
      }
    });
  }

  signIn(): void {
    if (this.passwordForm.invalid || this.isLoading) return;
    this.isLoading = true;
    this.error = null;

    this.auth.login(this.lockedEmail, this.passwordForm.get('password')?.value).subscribe({
      next: () => this.router.navigate(['/movies']),
      error: (err) => {
        this.isLoading = false;
        if (err.status === 401) this.error = 'Incorrect password.';
        else this.error = 'Something went wrong. Please try again.';
      }
    });
  }

  register(): void {
    if (this.registerForm.invalid || this.isLoading) return;
    this.isLoading = true;
    this.error = null;

    this.auth.register(this.lockedEmail, this.registerForm.get('password')?.value).subscribe({
      next: () => {
        this.auth.login(this.lockedEmail, this.registerForm.get('password')?.value).subscribe({
          next: () => this.router.navigate(['/movies']),
          error: () => {
            this.isLoading = false;
            this.error = 'Registered but could not log in. Please try signing in.';
          }
        });
      },
      error: (err) => {
        this.isLoading = false;
        if (err.status === 409) this.error = 'An account with this email already exists.';
        else this.error = 'Something went wrong. Please try again.';
      }
    });
  }

  back(): void {
    this.step = 'email';
    this.error = null;
    this.passwordForm.reset();
    this.registerForm.reset();
  }

  loginWithGoogle(): void {
    this.auth.loginWithGoogle().subscribe({
      next: () => this.router.navigate(['/movies'])
    });
  }
}

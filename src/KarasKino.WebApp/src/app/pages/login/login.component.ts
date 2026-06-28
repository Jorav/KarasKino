import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';


@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  form: FormGroup;
  isLoading = false;
  error: string | null = null;
  isRegister = false;

  constructor(
    private fb: FormBuilder,
    private auth: AuthService,
    private router: Router
  ) {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  toggleMode(): void {
    this.isRegister = !this.isRegister;
    this.error = null;
  }

  submit(): void {
    if (this.form.invalid || this.isLoading) return;
    this.isLoading = true;
    this.error = null;

    const { email, password } = this.form.value;
    const action$ = this.isRegister
      ? this.auth.register(email, password)
      : this.auth.login(email, password);

    action$.subscribe({
      next: () => {
        if (this.isRegister) {
          this.error = 'Registered successfully. You can now log in.';
          this.isRegister = false;
        } else {
          this.router.navigate(['/movies']);
        }
        this.isLoading = false;
      },
      error: (err) => {
        this.isLoading = false;
        if (err.status === 401) this.error = 'Invalid email or password.';
        else if (err.status === 409) this.error = 'Email already registered.';
        else this.error = 'Something went wrong. Please try again.';
      }
    });
  }

  loginWithGoogle(): void {
    this.auth.loginWithGoogle();
  }
}

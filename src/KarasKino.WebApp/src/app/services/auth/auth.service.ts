import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';

export type UserRole = 'Viewer' | 'Editor' | 'Admin';

export interface AuthUser {
  email: string;
  role: UserRole;
}

export interface EmailCheckResult {
  exists: boolean;
  hasPassword: boolean;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly apiUrl = environment.apiBaseUrl;

  currentUser = signal<AuthUser | null>(null);

  constructor(private http: HttpClient) {
    this.fetchCurrentUser().subscribe({ error: () => { } });
  }

  login(email: string, password: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/authentication/login`, { email, password }, { withCredentials: true })
      .pipe(tap(() => this.fetchCurrentUser().subscribe({ error: () => { } })));
  }

  register(email: string, password: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/authentication/register`, { email, password }, { withCredentials: true });
  }

  logout(): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/authentication/logout`, {}, { withCredentials: true })
      .pipe(tap(() => this.currentUser.set(null)));
  }

  loginWithGoogle(): void {
    window.location.href = `${this.apiUrl}/authentication/google`;
  }

  fetchCurrentUser(): Observable<AuthUser> {
    return this.http.get<AuthUser>(`${this.apiUrl}/authentication/me`, { withCredentials: true })
      .pipe(tap(user => this.currentUser.set(user)));
  }

  isAuthenticated(): boolean {
    return this.currentUser() !== null;
  }

  canEdit(): boolean {
    const role = this.currentUser()?.role;
    return role === 'Editor' || role === 'Admin';
  }

  checkEmail(email: string): Observable<EmailCheckResult> {
    return this.http.post<EmailCheckResult>(
      `${this.apiUrl}/authentication/check-email`,
      { email },
      { withCredentials: true }
    );
  }
}

import { Component, inject, HostListener } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth/auth.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.scss'
})
export class HeaderComponent {
  auth = inject(AuthService);
  private router = inject(Router);
  dropdownOpen = false;

  toggleDropdown(event: Event): void {
    event.stopPropagation();
    this.dropdownOpen = !this.dropdownOpen;
  }

  @HostListener('document:click')
  closeDropdown(): void {
    this.dropdownOpen = false;
  }

  logout(): void {
    this.dropdownOpen = false;
    this.auth.logout().subscribe(() => this.router.navigate(['/login']));
  }

  get avatarLetter(): string {
    return this.auth.currentUser()?.email?.[0]?.toUpperCase() ?? '?';
  }
}
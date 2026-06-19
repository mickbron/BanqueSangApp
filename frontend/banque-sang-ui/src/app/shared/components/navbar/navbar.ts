import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-navbar',
  imports: [],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css',
})
export class Navbar {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  role = this.authService.getRole();

  /**
   * Déconnecte l'utilisateur connecté et le redirige vers la page de connexion.
   */
  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}

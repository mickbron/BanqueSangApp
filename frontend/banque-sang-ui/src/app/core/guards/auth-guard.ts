import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

/**
 * Guard qui protège les pages privées.
 * Il vérifie que l'utilisateur est connecté avant d'autoriser l'accès à une route.
 */
export const authGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if (authService.isLoggedIn()) {
    return true;
  }

  authService.logout();
  router.navigate(['/login']);
  return false;
};

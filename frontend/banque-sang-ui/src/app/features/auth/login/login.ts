import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-login',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  isLoading = false;
  errorMessage = '';

  loginForm = this.fb.group({
    login: ['', [Validators.required]],
    password: ['', [Validators.required]]
  });

  /**
   * Soumet le formulaire de connexion.
   * Si le formulaire est valide, appelle l'API d'authentification.
   */
  onSubmit(): void {
    this.errorMessage = '';

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    const request = {
      login: this.loginForm.value.login ?? '',
      password: this.loginForm.value.password ?? ''
    };

    this.isLoading = true;

    this.authService.login(request).subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigate(['/dashboard']);
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = error.error?.message ?? 'Erreur de connexion. Veuillez réessayer.';
      }
    });
  }

  /**
   * Indique si le champ login est invalide après interaction utilisateur.
   */
  get loginInvalid(): boolean {
    const control = this.loginForm.get('login');
    return !!control && control.invalid && control.touched;
  }

  /**
   * Indique si le champ mot de passe est invalide après interaction utilisateur.
   */
  get passwordInvalid(): boolean {
    const control = this.loginForm.get('password');
    return !!control && control.invalid && control.touched;
  }
}

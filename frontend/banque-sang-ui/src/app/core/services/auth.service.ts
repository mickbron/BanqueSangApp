import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { LoginRequest, LoginResponse } from '../../shared/models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly tokenKey = 'banque_sang_token';
  private readonly roleKey = 'banque_sang_role';
  private readonly expirationKey = 'banque_sang_expiration';

  constructor(private readonly http: HttpClient) {}

  /**
   * Envoie le login et le mot de passe vers l'API.
   * Si la connexion réussit, le token, le rôle et l'expiration sont sauvegardés en localStorage.
   */
  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${environment.apiUrl}/auth/login`, request).pipe(
      tap((response) => {
        this.saveSession(response);
      })
    );
  }

  /**
   * Sauvegarde les informations de session dans le navigateur.
   */
  private saveSession(response: LoginResponse): void {
    localStorage.setItem(this.tokenKey, response.token);
    localStorage.setItem(this.roleKey, response.role);
    localStorage.setItem(this.expirationKey, response.expiration);
  }

  /**
   * Retourne le token JWT actuellement stocké.
   */
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  /**
   * Retourne le rôle de l'utilisateur connecté.
   */
  getRole(): string | null {
    return localStorage.getItem(this.roleKey);
  }

  /**
   * Vérifie si l'utilisateur possède un token.
   */
  isLoggedIn(): boolean {
    return this.getToken() !== null;
  }

  /**
   * Déconnecte l'utilisateur en supprimant les informations de session.
   */
  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.roleKey);
    localStorage.removeItem(this.expirationKey);
  }
}

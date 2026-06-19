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
   * Envoie les identifiants à l'API.
   * Si l'authentification réussit, la session est sauvegardée dans le localStorage.
   */
  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${environment.apiUrl}/auth/login`, request).pipe(
      tap((response) => {
        this.saveSession(response);
      })
    );
  }

  /**
   * Sauvegarde le token, le rôle et la date d'expiration dans le localStorage.
   */
  private saveSession(response: LoginResponse): void {
    localStorage.setItem(this.tokenKey, response.token);
    localStorage.setItem(this.roleKey, response.role);
    localStorage.setItem(this.expirationKey, response.expiration);
  }

  /**
   * Retourne le token JWT stocké dans le navigateur.
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
   * Vérifie si l'utilisateur est connecté.
   * Un utilisateur est connecté uniquement s'il possède un token non expiré.
   */
  isLoggedIn(): boolean {
    const token = this.getToken();
    const expiration = localStorage.getItem(this.expirationKey);

    if (!token || !expiration) {
      return false;
    }

    const expirationDate = new Date(expiration);
    const now = new Date();

    return expirationDate > now;
  }

  /**
   * Vérifie si l'utilisateur possède l'un des rôles autorisés.
   */
  hasRole(allowedRoles: string[]): boolean {
    const role = this.getRole();

    if (!role) {
      return false;
    }

    return allowedRoles.includes(role);
  }

  /**
   * Supprime les informations de session.
   */
  logout(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.roleKey);
    localStorage.removeItem(this.expirationKey);
  }
}

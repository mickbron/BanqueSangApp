import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  CreateDonneurRequest,
  Donneur,
  EligibiliteResult
} from '../../shared/models/donneur.model';

@Injectable({
  providedIn: 'root'
})
export class DonneurService {
  private readonly apiUrl = `${environment.apiUrl}/donneurs`;

  constructor(private readonly http: HttpClient) {}

  /**
   * Récupère la liste complète des donneurs depuis l'API.
   */
  getAll(): Observable<Donneur[]> {
    return this.http.get<Donneur[]>(this.apiUrl);
  }

  /**
   * Récupère un donneur à partir de son identifiant.
   */
  getById(id: number): Observable<Donneur> {
    return this.http.get<Donneur>(`${this.apiUrl}/${id}`);
  }

  /**
   * Crée un nouveau donneur via l'API.
   */
  create(request: CreateDonneurRequest): Observable<{ idDonneur: number; message: string }> {
    return this.http.post<{ idDonneur: number; message: string }>(this.apiUrl, request);
  }

  /**
   * Demande au backend de vérifier l'éligibilité d'un donneur.
   */
  verifierEligibilite(id: number): Observable<EligibiliteResult> {
    return this.http.get<EligibiliteResult>(`${this.apiUrl}/${id}/eligibilite`);
  }
}

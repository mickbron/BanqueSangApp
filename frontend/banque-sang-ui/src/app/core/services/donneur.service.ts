import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Donneur, EligibiliteResult } from '../../shared/models/donneur.model';

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
   * Récupère un donneur par son identifiant.
   */
  getById(id: number): Observable<Donneur> {
    return this.http.get<Donneur>(`${this.apiUrl}/${id}`);
  }

  /**
   * Demande au backend de vérifier l'éligibilité d'un donneur.
   */
  verifierEligibilite(id: number): Observable<EligibiliteResult> {
    return this.http.get<EligibiliteResult>(`${this.apiUrl}/${id}/eligibilite`);
  }
}

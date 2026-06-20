import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  CreateDemandeSangRequest,
  DemandeSang,
  UpdateDemandeSangStatutRequest
} from '../../shared/models/demande-sang.model';

@Injectable({
  providedIn: 'root'
})
export class DemandeSangService {
  private readonly apiUrl = `${environment.apiUrl}/demandes-sang`;

  constructor(private readonly http: HttpClient) {}

  /**
   * Récupère toutes les demandes de sang.
   */
  getAll(): Observable<DemandeSang[]> {
    return this.http.get<DemandeSang[]>(this.apiUrl);
  }

  /**
   * Récupère une demande de sang par son identifiant.
   */
  getById(id: number): Observable<DemandeSang> {
    return this.http.get<DemandeSang>(`${this.apiUrl}/${id}`);
  }

  /**
   * Crée une nouvelle demande de sang.
   */
  create(request: CreateDemandeSangRequest): Observable<{ idDemande: number; message: string }> {
    return this.http.post<{ idDemande: number; message: string }>(this.apiUrl, request);
  }

  /**
   * Modifie uniquement le statut d'une demande de sang.
   */
  updateStatut(id: number, request: UpdateDemandeSangStatutRequest): Observable<{ message: string }> {
    return this.http.put<{ message: string }>(`${this.apiUrl}/${id}/statut`, request);
  }
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CreateDonRequest, CreateDonResponse, Don } from '../../shared/models/don.model';

@Injectable({
  providedIn: 'root'
})
export class DonService {
  private readonly apiUrl = `${environment.apiUrl}/dons`;

  constructor(private readonly http: HttpClient) {}

  /**
   * Récupère la liste des dons enregistrés.
   */
  getAll(): Observable<Don[]> {
    return this.http.get<Don[]>(this.apiUrl);
  }

  /**
   * Enregistre un nouveau don.
   * Le backend crée automatiquement une poche de sang associée.
   */
  create(request: CreateDonRequest): Observable<CreateDonResponse> {
    return this.http.post<CreateDonResponse>(this.apiUrl, request);
  }
}

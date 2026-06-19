import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  CreateResultatTestRequest,
  CreateResultatTestResponse,
  ResultatTest,
  TestBiologique,
  UpdateResultatTestRequest
} from '../../shared/models/test.model';

@Injectable({
  providedIn: 'root'
})
export class TestBiologiqueService {
  private readonly apiUrl = `${environment.apiUrl}/tests`;

  constructor(private readonly http: HttpClient) {}

  /**
   * Récupère la liste des types de tests biologiques.
   */
  getTests(): Observable<TestBiologique[]> {
    return this.http.get<TestBiologique[]>(this.apiUrl);
  }

  /**
   * Récupère tous les résultats de tests biologiques déjà soumis.
   */
  getResultats(): Observable<ResultatTest[]> {
    return this.http.get<ResultatTest[]>(`${this.apiUrl}/resultats`);
  }

  /**
   * Soumet un nouveau résultat de test biologique.
   */
  createResultat(request: CreateResultatTestRequest): Observable<CreateResultatTestResponse> {
    return this.http.post<CreateResultatTestResponse>(`${this.apiUrl}/resultats`, request);
  }

  /**
   * Modifie un résultat de test biologique existant.
   */
  updateResultat(id: number, request: UpdateResultatTestRequest): Observable<{ message: string }> {
    return this.http.put<{ message: string }>(`${this.apiUrl}/resultats/${id}`, request);
  }

}

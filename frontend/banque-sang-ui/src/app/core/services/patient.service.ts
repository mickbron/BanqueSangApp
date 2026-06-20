import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';
import {
  CreatePatientRequest,
  Patient,
  UpdatePatientRequest
} from '../../shared/models/patient.model';

@Injectable({
  providedIn: 'root'
})
export class PatientService {
  private readonly apiUrl = `${environment.apiUrl}/patients`;

  constructor(private readonly http: HttpClient) {}

  /**
   * Récupère tous les patients.
   */
  getAll(): Observable<Patient[]> {
    return this.http.get<Patient[]>(this.apiUrl);
  }

  /**
   * Récupère un patient par son identifiant.
   */
  getById(id: number): Observable<Patient> {
    return this.http.get<Patient>(`${this.apiUrl}/${id}`);
  }

  /**
   * Crée un nouveau patient.
   */
  create(request: CreatePatientRequest): Observable<{ idPatient: number; message: string }> {
    return this.http.post<{ idPatient: number; message: string }>(this.apiUrl, request);
  }

  /**
   * Modifie un patient existant.
   */
  update(id: number, request: UpdatePatientRequest): Observable<{ message: string }> {
    return this.http.put<{ message: string }>(`${this.apiUrl}/${id}`, request);
  }
}

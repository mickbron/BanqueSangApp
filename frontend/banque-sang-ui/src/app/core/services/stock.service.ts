import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Sang, StockGroupe } from '../../shared/models/stock.model';

@Injectable({
  providedIn: 'root'
})
export class StockService {
  private readonly apiUrl = `${environment.apiUrl}/stock`;

  constructor(private readonly http: HttpClient) {}

  /**
   * Récupère toutes les poches de sang.
   */
  getAll(): Observable<Sang[]> {
    return this.http.get<Sang[]>(this.apiUrl);
  }

  /**
   * Récupère les statistiques de stock groupées par groupe sanguin.
   */
  getStockParGroupe(): Observable<StockGroupe[]> {
    return this.http.get<StockGroupe[]>(`${this.apiUrl}/groupes`);
  }

  /**
   * Récupère les poches proches de la péremption ou déjà périmées.
   */
  getAlertes(): Observable<Sang[]> {
    return this.http.get<Sang[]>(`${this.apiUrl}/alertes`);
  }
}

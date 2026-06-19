import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StockService } from '../../../core/services/stock.service';
import { Sang, StockGroupe } from '../../../shared/models/stock.model';

@Component({
  selector: 'app-stock-dashboard',
  imports: [CommonModule],
  templateUrl: './stock-dashboard.html',
  styleUrl: './stock-dashboard.css',
})
export class StockDashboard implements OnInit {
  private readonly stockService = inject(StockService);

  poches: Sang[] = [];
  stockGroupes: StockGroupe[] = [];
  alertes: Sang[] = [];

  isLoading = false;
  errorMessage = '';

  /**
   * Charge automatiquement les données du stock à l'ouverture de la page.
   */
  ngOnInit(): void {
    this.loadStock();
  }

  /**
   * Charge les poches, les statistiques par groupe sanguin et les alertes.
   */
  loadStock(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.stockService.getAll().subscribe({
      next: (poches) => {
        this.poches = poches;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Impossible de charger les poches de sang.';
        this.isLoading = false;
      }
    });

    this.stockService.getStockParGroupe().subscribe({
      next: (stockGroupes) => {
        this.stockGroupes = stockGroupes;
      },
      error: () => {
        this.errorMessage = 'Impossible de charger le stock par groupe sanguin.';
      }
    });

    this.stockService.getAlertes().subscribe({
      next: (alertes) => {
        this.alertes = alertes;
      },
      error: () => {
        this.errorMessage = 'Impossible de charger les alertes de péremption.';
      }
    });
  }

  /**
   * Retourne le nombre total de poches.
   */
  get totalPoches(): number {
    return this.poches.length;
  }

  /**
   * Retourne le nombre de poches disponibles.
   */
  get totalDisponibles(): number {
    return this.poches.filter(poche => poche.sangDisponible).length;
  }

  /**
   * Retourne le nombre de poches indisponibles.
   */
  get totalIndisponibles(): number {
    return this.poches.filter(poche => !poche.sangDisponible).length;
  }

  /**
   * Retourne le nombre de poches périmées.
   */
  get totalPerimees(): number {
    const today = new Date();

    return this.poches.filter(poche => {
      const peremption = new Date(poche.sangDatePeremption);
      return peremption < today;
    }).length;
  }

  /**
   * Retourne une classe CSS selon la disponibilité de la poche.
   */
  getDisponibiliteClass(disponible: boolean): string {
    return disponible ? 'badge badge-success' : 'badge badge-warning';
  }

  /**
   * Retourne une classe CSS selon l'état de péremption.
   */
  getPeremptionClass(datePeremption: string): string {
    const today = new Date();
    const peremption = new Date(datePeremption);

    if (peremption < today) {
      return 'badge badge-danger';
    }

    const differenceMs = peremption.getTime() - today.getTime();
    const differenceJours = differenceMs / (1000 * 60 * 60 * 24);

    if (differenceJours <= 7) {
      return 'badge badge-warning';
    }

    return 'badge badge-success';
  }

  /**
   * Retourne le texte affiché pour la péremption.
   */
  getPeremptionLabel(datePeremption: string): string {
    const today = new Date();
    const peremption = new Date(datePeremption);

    if (peremption < today) {
      return 'Périmée';
    }

    const differenceMs = peremption.getTime() - today.getTime();
    const differenceJours = Math.ceil(differenceMs / (1000 * 60 * 60 * 24));

    if (differenceJours <= 7) {
      return `Expire dans ${differenceJours} j`;
    }

    return 'Valide';
  }
}

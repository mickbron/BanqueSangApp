import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DashboardService } from '../../../core/services/dashboard.service';
import { DashboardStats } from '../../../shared/models/dashboard.model';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  private readonly dashboardService = inject(DashboardService);

  stats: DashboardStats | null = null;

  isLoading = false;
  errorMessage = '';

  /**
   * Charge les statistiques à l'ouverture du tableau de bord.
   */
  ngOnInit(): void {
    this.loadStats();
  }

  /**
   * Récupère les statistiques depuis l'API.
   */
  loadStats(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.dashboardService.getStats().subscribe({
      next: (stats) => {
        this.stats = stats;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Impossible de charger les statistiques du tableau de bord.';
        this.isLoading = false;
      }
    });
  }
}

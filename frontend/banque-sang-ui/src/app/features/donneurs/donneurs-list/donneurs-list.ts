import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DonneurService } from '../../../core/services/donneur.service';
import { Donneur } from '../../../shared/models/donneur.model';

@Component({
  selector: 'app-donneurs-list',
  imports: [CommonModule],
  templateUrl: './donneurs-list.html',
  styleUrl: './donneurs-list.css',
})
export class DonneursList implements OnInit {
  private readonly donneurService = inject(DonneurService);

  donneurs: Donneur[] = [];
  isLoading = false;
  errorMessage = '';
  successMessage = '';

  /**
   * Charge automatiquement la liste des donneurs quand la page s'ouvre.
   */
  ngOnInit(): void {
    this.loadDonneurs();
  }

  /**
   * Récupère les donneurs depuis l'API.
   */
  loadDonneurs(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.donneurService.getAll().subscribe({
      next: (donneurs) => {
        this.donneurs = donneurs;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Impossible de charger la liste des donneurs.';
        this.isLoading = false;
      }
    });
  }

  /**
   * Vérifie l'éligibilité d'un donneur et affiche le résultat.
   */
  verifierEligibilite(donneur: Donneur): void {
    this.errorMessage = '';
    this.successMessage = '';

    this.donneurService.verifierEligibilite(donneur.idDonneur).subscribe({
      next: (result) => {
        if (result.eligible) {
          this.successMessage = `${donneur.donneurPrenom} ${donneur.donneurNom} est éligible au don.`;
        } else {
          this.errorMessage = `${donneur.donneurPrenom} ${donneur.donneurNom} n'est pas éligible : ${result.raisons.join(', ')}`;
        }

        this.loadDonneurs();
      },
      error: () => {
        this.errorMessage = 'Erreur lors de la vérification de l’éligibilité.';
      }
    });
  }

  /**
   * Retourne une classe CSS selon le statut d'éligibilité.
   */
  getEligibiliteClass(statut: string): string {
    return statut === 'ELIGIBLE' ? 'badge badge-success' : 'badge badge-danger';
  }
}

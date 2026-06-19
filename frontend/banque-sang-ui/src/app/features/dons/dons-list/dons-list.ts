import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DonService } from '../../../core/services/don.service';
import { Don } from '../../../shared/models/don.model';
import { DonForm } from '../don-form/don-form';

@Component({
  selector: 'app-dons-list',
  imports: [CommonModule, DonForm],
  templateUrl: './dons-list.html',
  styleUrl: './dons-list.css',
})
export class DonsList implements OnInit {
  private readonly donService = inject(DonService);

  dons: Don[] = [];
  isLoading = false;
  errorMessage = '';
  successMessage = '';
  showDonForm = false;

  /**
   * Charge les dons à l'ouverture de la page.
   */
  ngOnInit(): void {
    this.loadDons();
  }

  /**
   * Ouvre la modal d'enregistrement d'un don.
   */
  openDonForm(): void {
    this.errorMessage = '';
    this.successMessage = '';
    this.showDonForm = true;
  }

  /**
   * Ferme la modal d'enregistrement d'un don.
   */
  closeDonForm(): void {
    this.showDonForm = false;
  }

  /**
   * Méthode appelée après l'enregistrement réussi d'un don.
   */
  onDonCreated(): void {
    this.showDonForm = false;
    this.successMessage = 'Don enregistré avec succès. Une poche de sang a été créée.';
    this.loadDons();
  }

  /**
   * Récupère la liste des dons depuis l'API.
   */
  loadDons(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.donService.getAll().subscribe({
      next: (dons) => {
        this.dons = dons;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Impossible de charger la liste des dons.';
        this.isLoading = false;
      }
    });
  }

  /**
   * Retourne une classe CSS selon le statut du don.
   */
  getStatutClass(statut: string): string {
    if (statut === 'VALIDE') {
      return 'badge badge-success';
    }

    if (statut === 'REJETE') {
      return 'badge badge-danger';
    }

    return 'badge badge-warning';
  }
}

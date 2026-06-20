import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DemandeSangService } from '../../../core/services/demande-sang.service';
import { DemandeSang } from '../../../shared/models/demande-sang.model';
import { DemandeSangForm } from '../demande-sang-form/demande-sang-form';

@Component({
  selector: 'app-demandes-sang-list',
  imports: [CommonModule, DemandeSangForm],
  templateUrl: './demandes-sang-list.html',
  styleUrl: './demandes-sang-list.css',
})
export class DemandesSangList implements OnInit {
  private readonly demandeSangService = inject(DemandeSangService);

  demandes: DemandeSang[] = [];

  isLoading = false;
  errorMessage = '';
  successMessage = '';
  showDemandeForm = false;

  statuts = ['EN_ATTENTE', 'RESERVEE', 'TRAITEE', 'ANNULEE'];

  /**
   * Charge les demandes de sang à l'ouverture de la page.
   */
  ngOnInit(): void {
    this.loadDemandes();
  }

  /**
   * Récupère toutes les demandes depuis l'API.
   */
  loadDemandes(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.demandeSangService.getAll().subscribe({
      next: (demandes) => {
        this.demandes = demandes;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Impossible de charger les demandes de sang.';
        this.isLoading = false;
      }
    });
  }

  /**
   * Ouvre le formulaire de création.
   */
  openDemandeForm(): void {
    this.errorMessage = '';
    this.successMessage = '';
    this.showDemandeForm = true;
  }

  /**
   * Ferme le formulaire de création.
   */
  closeDemandeForm(): void {
    this.showDemandeForm = false;
  }

  /**
   * Recharge la liste après création.
   */
  onDemandeCreated(): void {
    this.showDemandeForm = false;
    this.successMessage = 'Demande de sang créée avec succès.';
    this.loadDemandes();
  }

  /**
   * Modifie le statut d'une demande.
   */
  updateStatut(demande: DemandeSang, statut: string): void {
    this.errorMessage = '';
    this.successMessage = '';

    this.demandeSangService.updateStatut(demande.idDemande, { statut }).subscribe({
      next: () => {
        this.successMessage = 'Statut modifié avec succès.';
        this.loadDemandes();
      },
      error: (error) => {
        this.errorMessage = error.error?.message ?? 'Erreur lors de la modification du statut.';
      }
    });
  }

  /**
   * Retourne la classe CSS selon le statut.
   */
  getStatutClass(statut: string): string {
    if (statut === 'TRAITEE') {
      return 'badge badge-success';
    }

    if (statut === 'ANNULEE') {
      return 'badge badge-danger';
    }

    if (statut === 'RESERVEE') {
      return 'badge badge-info';
    }

    return 'badge badge-warning';
  }

  /**
   * Retourne la classe CSS selon l'urgence.
   */
  getUrgenceClass(urgence: string): string {
    if (urgence === 'CRITIQUE') {
      return 'badge badge-danger';
    }

    if (urgence === 'URGENT') {
      return 'badge badge-warning';
    }

    return 'badge badge-info';
  }
}

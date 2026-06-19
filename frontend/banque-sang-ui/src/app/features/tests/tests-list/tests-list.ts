import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TestBiologiqueService } from '../../../core/services/test-biologique.service';
import { ResultatTest, TestBiologique } from '../../../shared/models/test.model';
import { TestForm } from '../test-form/test-form';

@Component({
  selector: 'app-tests-list',
  imports: [CommonModule, TestForm],
  templateUrl: './tests-list.html',
  styleUrl: './tests-list.css',
})
export class TestsList implements OnInit {
  private readonly testBiologiqueService = inject(TestBiologiqueService);

  tests: TestBiologique[] = [];
  resultats: ResultatTest[] = [];

  isLoading = false;
  errorMessage = '';
  successMessage = '';

  showTestForm = false;
  selectedResultat: ResultatTest | null = null;

  /**
   * Charge les tests et les résultats à l'ouverture de la page.
   */
  ngOnInit(): void {
    this.loadData();
  }

  /**
   * Charge la liste des types de tests biologiques
   * et la liste des résultats déjà soumis.
   */
  loadData(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.testBiologiqueService.getTests().subscribe({
      next: (tests) => {
        this.tests = tests;
      },
      error: () => {
        this.errorMessage = 'Impossible de charger les types de tests.';
      }
    });

    this.testBiologiqueService.getResultats().subscribe({
      next: (resultats) => {
        this.resultats = resultats;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Impossible de charger les résultats de tests.';
        this.isLoading = false;
      }
    });
  }

  /**
   * Ouvre la modal en mode création d'un nouveau résultat de test.
   */
  openTestForm(): void {
    this.errorMessage = '';
    this.successMessage = '';
    this.selectedResultat = null;
    this.showTestForm = true;
  }

  /**
   * Ouvre la modal en mode modification d'un résultat existant.
   */
  editResultat(resultat: ResultatTest): void {
    this.errorMessage = '';
    this.successMessage = '';
    this.selectedResultat = resultat;
    this.showTestForm = true;
  }

  /**
   * Ferme la modal de création ou de modification.
   */
  closeTestForm(): void {
    this.showTestForm = false;
    this.selectedResultat = null;
  }

  /**
   * Méthode appelée après l'enregistrement ou la modification réussie d'un résultat.
   * Elle ferme la modal puis recharge la liste.
   */
  onTestCreated(): void {
    this.showTestForm = false;
    this.selectedResultat = null;
    this.successMessage = 'Résultat de test enregistré avec succès.';
    this.loadData();
  }

  /**
   * Retourne le nombre total de résultats négatifs.
   */
  get totalNegatifs(): number {
    return this.resultats.filter(resultat => resultat.resultat === 'NEGATIF').length;
  }

  /**
   * Retourne le nombre total de résultats positifs.
   */
  get totalPositifs(): number {
    return this.resultats.filter(resultat => resultat.resultat === 'POSITIF').length;
  }

  /**
   * Retourne une classe CSS selon le résultat biologique.
   */
  getResultatClass(resultat: string): string {
    if (resultat === 'NEGATIF') {
      return 'badge badge-success';
    }

    if (resultat === 'POSITIF') {
      return 'badge badge-danger';
    }

    return 'badge badge-warning';
  }

  /**
   * Retourne une classe CSS selon le statut du test.
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

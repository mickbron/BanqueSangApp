import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { TestBiologiqueService } from '../../../core/services/test-biologique.service';
import { StockService } from '../../../core/services/stock.service';

import { Sang } from '../../../shared/models/stock.model';
import {
  CreateResultatTestRequest,
  ResultatTest,
  TestBiologique,
  UpdateResultatTestRequest
} from '../../../shared/models/test.model';

@Component({
  selector: 'app-test-form',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './test-form.html',
  styleUrl: './test-form.css',
})
export class TestForm implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly testBiologiqueService = inject(TestBiologiqueService);
  private readonly stockService = inject(StockService);

  @Input() resultatToEdit: ResultatTest | null = null;

  @Output() closed = new EventEmitter<void>();
  @Output() testCreated = new EventEmitter<void>();

  tests: TestBiologique[] = [];
  poches: Sang[] = [];

  isSubmitting = false;
  errorMessage = '';

  resultats = ['NEGATIF', 'POSITIF', 'EN_ATTENTE'];

  testForm = this.fb.group({
    idPersonnel: [1, [Validators.required, Validators.min(1)]],
    idSang: ['', [Validators.required]],
    idTest: ['', [Validators.required]],
    dateTest: [this.getCurrentDateTime(), [Validators.required]],
    resultat: ['NEGATIF', [Validators.required]],
    commentaire: ['']
  });

  /**
   * Charge les données nécessaires à l'ouverture de la modal.
   * En mode modification, le formulaire est prérempli avec le résultat sélectionné.
   */
  ngOnInit(): void {
    this.loadTests();
    this.loadPoches();
    this.fillFormForEdit();
  }

  /**
   * Récupère les types de tests biologiques disponibles depuis l'API.
   */
  loadTests(): void {
    this.testBiologiqueService.getTests().subscribe({
      next: (tests) => {
        this.tests = tests;
      },
      error: () => {
        this.errorMessage = 'Impossible de charger les types de tests.';
      }
    });
  }

  /**
   * Récupère les poches de sang depuis le stock.
   * Ces poches permettent de sélectionner l'échantillon à tester.
   */
  loadPoches(): void {
    this.stockService.getAll().subscribe({
      next: (poches) => {
        this.poches = poches;
      },
      error: () => {
        this.errorMessage = 'Impossible de charger les poches de sang.';
      }
    });
  }

  /**
   * Préremplit le formulaire lorsqu'un résultat existant est en cours de modification.
   * Les champs d'identification sont désactivés pour éviter de déplacer un résultat
   * vers une autre poche ou un autre type de test.
   */
  private fillFormForEdit(): void {
    if (!this.resultatToEdit) {
      return;
    }

    this.testForm.patchValue({
      idPersonnel: this.resultatToEdit.idPersonnel,
      idSang: String(this.resultatToEdit.idSang),
      idTest: String(this.resultatToEdit.idTest),
      dateTest: this.resultatToEdit.dateTest.slice(0, 16),
      resultat: this.resultatToEdit.resultat,
      commentaire: this.resultatToEdit.commentaire ?? ''
    });

    this.testForm.get('idPersonnel')?.disable();
    this.testForm.get('idSang')?.disable();
    this.testForm.get('idTest')?.disable();
    this.testForm.get('dateTest')?.disable();
  }

  /**
   * Ferme la modal sans enregistrer.
   */
  close(): void {
    this.closed.emit();
  }

  /**
   * Soumet le formulaire.
   * Si resultatToEdit est défini, le formulaire fonctionne en mode modification.
   * Sinon, il fonctionne en mode création.
   */
  onSubmit(): void {
    this.errorMessage = '';

    if (this.testForm.invalid) {
      this.testForm.markAllAsTouched();
      return;
    }

    if (this.resultatToEdit) {
      this.updateResultat();
      return;
    }

    this.createResultat();
  }

  /**
   * Crée un nouveau résultat de test biologique.
   * Les données sont envoyées au backend avec une requête POST.
   */
  private createResultat(): void {
    const formValue = this.testForm.getRawValue();

    const request: CreateResultatTestRequest = {
      idPersonnel: Number(formValue.idPersonnel),
      idSang: Number(formValue.idSang),
      idTest: Number(formValue.idTest),
      dateTest: formValue.dateTest ?? this.getCurrentDateTime(),
      resultat: formValue.resultat ?? 'NEGATIF',
      commentaire: formValue.commentaire || null
    };

    this.isSubmitting = true;

    this.testBiologiqueService.createResultat(request).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.testCreated.emit();
      },
      error: (error) => {
        this.isSubmitting = false;
        this.errorMessage = error.error?.message ?? 'Erreur lors de l’enregistrement du résultat.';
      }
    });
  }

  /**
   * Modifie un résultat de test biologique existant.
   * Les données sont envoyées au backend avec une requête PUT.
   */
  private updateResultat(): void {
    if (!this.resultatToEdit) {
      return;
    }

    const formValue = this.testForm.getRawValue();

    const request: UpdateResultatTestRequest = {
      resultat: formValue.resultat ?? 'NEGATIF',
      commentaire: formValue.commentaire || null,
      statutTest: formValue.resultat === 'EN_ATTENTE' ? 'EN_ATTENTE' : 'VALIDE'
    };

    this.isSubmitting = true;

    this.testBiologiqueService.updateResultat(this.resultatToEdit.idResultatTest, request).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.testCreated.emit();
      },
      error: (error) => {
        this.isSubmitting = false;
        this.errorMessage = error.error?.message ?? 'Erreur lors de la modification du résultat.';
      }
    });
  }

  /**
   * Vérifie si un champ est invalide après interaction utilisateur.
   */
  isInvalid(fieldName: string): boolean {
    const control = this.testForm.get(fieldName);
    return !!control && control.invalid && control.touched;
  }

  /**
   * Retourne la date et l'heure actuelles au format compatible avec input datetime-local.
   */
  private getCurrentDateTime(): string {
    const now = new Date();
    now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
    return now.toISOString().slice(0, 16);
  }
}

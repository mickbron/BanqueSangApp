import { Component, EventEmitter, OnInit, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { DemandeSangService } from '../../../core/services/demande-sang.service';
import { PatientService } from '../../../core/services/patient.service';
import { StockService } from '../../../core/services/stock.service';

import { CreateDemandeSangRequest } from '../../../shared/models/demande-sang.model';
import { Patient } from '../../../shared/models/patient.model';
import { Sang } from '../../../shared/models/stock.model';

@Component({
  selector: 'app-demande-sang-form',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './demande-sang-form.html',
  styleUrl: './demande-sang-form.css',
})
export class DemandeSangForm implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly demandeSangService = inject(DemandeSangService);
  private readonly patientService = inject(PatientService);
  private readonly stockService = inject(StockService);

  @Output() closed = new EventEmitter<void>();
  @Output() demandeCreated = new EventEmitter<void>();

  patients: Patient[] = [];
  poches: Sang[] = [];

  isSubmitting = false;
  errorMessage = '';

  typesComposants = ['SANG_TOTAL', 'GLOBULES_ROUGES', 'PLASMA', 'PLAQUETTES'];
  urgences = ['NORMAL', 'URGENT', 'CRITIQUE'];

  demandeForm = this.fb.group({
    idPatient: ['', [Validators.required]],
    idSang: [''],
    idPersonnel: [1, [Validators.required, Validators.min(1)]],
    typeComposant: ['SANG_TOTAL', [Validators.required]],
    quantitePoche: [1, [Validators.required, Validators.min(1)]],
    urgence: ['NORMAL', [Validators.required]],
    dateDemande: [this.getCurrentDateTime(), [Validators.required]],
    commentaire: ['']
  });

  /**
   * Charge les patients et les poches de sang à l'ouverture du formulaire.
   */
  ngOnInit(): void {
    this.loadPatients();
    this.loadPoches();
  }

  /**
   * Récupère les patients depuis l'API.
   */
  loadPatients(): void {
    this.patientService.getAll().subscribe({
      next: (patients) => {
        this.patients = patients;
      },
      error: () => {
        this.errorMessage = 'Impossible de charger les patients.';
      }
    });
  }

  /**
   * Récupère les poches de sang depuis le stock.
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
   * Ferme la modal sans enregistrer.
   */
  close(): void {
    this.closed.emit();
  }

  /**
   * Soumet la nouvelle demande de sang au backend.
   */
  onSubmit(): void {
    this.errorMessage = '';

    if (this.demandeForm.invalid) {
      this.demandeForm.markAllAsTouched();
      return;
    }

    const formValue = this.demandeForm.getRawValue();

    const request: CreateDemandeSangRequest = {
      idPatient: Number(formValue.idPatient),
      idSang: formValue.idSang ? Number(formValue.idSang) : null,
      idPersonnel: Number(formValue.idPersonnel),
      typeComposant: formValue.typeComposant ?? 'SANG_TOTAL',
      quantitePoche: Number(formValue.quantitePoche),
      urgence: formValue.urgence ?? 'NORMAL',
      dateDemande: formValue.dateDemande ?? this.getCurrentDateTime(),
      commentaire: formValue.commentaire || null
    };

    this.isSubmitting = true;

    this.demandeSangService.create(request).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.demandeCreated.emit();
      },
      error: (error) => {
        this.isSubmitting = false;
        this.errorMessage = error.error?.message ?? 'Erreur lors de la création de la demande.';
      }
    });
  }

  /**
   * Vérifie si un champ est invalide après interaction.
   */
  isInvalid(fieldName: string): boolean {
    const control = this.demandeForm.get(fieldName);
    return !!control && control.invalid && control.touched;
  }

  /**
   * Retourne la date et l'heure actuelles au format compatible avec datetime-local.
   */
  private getCurrentDateTime(): string {
    const now = new Date();
    now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
    return now.toISOString().slice(0, 16);
  }
}

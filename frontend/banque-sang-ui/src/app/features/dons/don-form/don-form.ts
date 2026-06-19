import { Component, EventEmitter, OnInit, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { DonService } from '../../../core/services/don.service';
import { DonneurService } from '../../../core/services/donneur.service';
import { Donneur } from '../../../shared/models/donneur.model';
import { CreateDonRequest } from '../../../shared/models/don.model';

@Component({
  selector: 'app-don-form',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './don-form.html',
  styleUrl: './don-form.css',
})
export class DonForm implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly donService = inject(DonService);
  private readonly donneurService = inject(DonneurService);

  @Output() closed = new EventEmitter<void>();
  @Output() donCreated = new EventEmitter<void>();

  donneurs: Donneur[] = [];
  isSubmitting = false;
  errorMessage = '';

  typesComposants = [
    { value: 'SANG_TOTAL', label: 'Sang total' },
    { value: 'GLOBULES_ROUGES', label: 'Globules rouges' },
    { value: 'PLASMA', label: 'Plasma' },
    { value: 'PLAQUETTES', label: 'Plaquettes' }
  ];

  donForm = this.fb.group({
    idDonneur: ['', [Validators.required]],
    idCentre: ['1', [Validators.required]],
    donDate: [this.getToday(), [Validators.required]],
    donVolumeMl: [450, [Validators.required, Validators.min(1)]],
    sangTypeComposant: ['SANG_TOTAL', [Validators.required]]
  });

  /**
   * Charge les donneurs dès l'ouverture de la modal.
   */
  ngOnInit(): void {
    this.loadDonneurs();
  }

  /**
   * Récupère les donneurs pour alimenter la liste déroulante.
   */
  loadDonneurs(): void {
    this.donneurService.getAll().subscribe({
      next: (donneurs) => {
        this.donneurs = donneurs;
      },
      error: () => {
        this.errorMessage = 'Impossible de charger les donneurs.';
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
   * Soumet le formulaire d'enregistrement d'un don.
   */
  onSubmit(): void {
    this.errorMessage = '';

    if (this.donForm.invalid) {
      this.donForm.markAllAsTouched();
      return;
    }

    const formValue = this.donForm.getRawValue();

    const request: CreateDonRequest = {
      idDonneur: Number(formValue.idDonneur),
      idCentre: Number(formValue.idCentre),
      donDate: formValue.donDate ?? this.getToday(),
      donVolumeMl: Number(formValue.donVolumeMl),
      sangTypeComposant: formValue.sangTypeComposant ?? 'SANG_TOTAL'
    };

    this.isSubmitting = true;

    this.donService.create(request).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.donCreated.emit();
      },
      error: (error) => {
        this.isSubmitting = false;
        this.errorMessage = error.error?.message ?? 'Erreur lors de l’enregistrement du don.';
      }
    });
  }

  /**
   * Vérifie si un champ est invalide après interaction utilisateur.
   */
  isInvalid(fieldName: string): boolean {
    const control = this.donForm.get(fieldName);
    return !!control && control.invalid && control.touched;
  }

  /**
   * Retourne la date du jour au format attendu par un input date.
   */
  private getToday(): string {
    return new Date().toISOString().slice(0, 10);
  }
}

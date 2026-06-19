import { Component, EventEmitter, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { DonneurService } from '../../../core/services/donneur.service';
import { CreateDonneurRequest } from '../../../shared/models/donneur.model';

@Component({
  selector: 'app-donneur-form',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './donneur-form.html',
  styleUrl: './donneur-form.css',
})
export class DonneurForm {
  private readonly fb = inject(FormBuilder);
  private readonly donneurService = inject(DonneurService);

  @Output() closed = new EventEmitter<void>();
  @Output() donneurCreated = new EventEmitter<void>();

  isSubmitting = false;
  errorMessage = '';

  groupesSanguins = ['A+', 'A-', 'B+', 'B-', 'O+', 'O-', 'AB+', 'AB-'];

  donneurForm = this.fb.group({
    donneurNom: ['', [Validators.required, Validators.maxLength(100)]],
    donneurPrenom: ['', [Validators.required, Validators.maxLength(100)]],
    donneurAdresse: ['', [Validators.required, Validators.maxLength(255)]],
    donneurTelephone: ['', [Validators.required, Validators.maxLength(20)]],
    donneurDateNaissance: ['', [Validators.required]],
    donneurGroupeSanguin: ['', [Validators.required]]
  });

  /**
   * Ferme la modal sans enregistrer.
   */
  close(): void {
    this.closed.emit();
  }

  /**
   * Soumet le formulaire de création d'un donneur.
   * Si le formulaire est valide, les données sont envoyées à l'API.
   */
  onSubmit(): void {
    this.errorMessage = '';

    if (this.donneurForm.invalid) {
      this.donneurForm.markAllAsTouched();
      return;
    }

    const formValue = this.donneurForm.getRawValue();

    const request: CreateDonneurRequest = {
      donneurNom: formValue.donneurNom ?? '',
      donneurPrenom: formValue.donneurPrenom ?? '',
      donneurAdresse: formValue.donneurAdresse ?? '',
      donneurTelephone: formValue.donneurTelephone ?? '',
      donneurDateNaissance: formValue.donneurDateNaissance ?? '',
      donneurGroupeSanguin: formValue.donneurGroupeSanguin ?? '',
      donneurStatutEligibilite: 'ELIGIBLE',
      donneurDernierDon: null
    };

    this.isSubmitting = true;

    this.donneurService.create(request).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.donneurCreated.emit();
      },
      error: (error) => {
        this.isSubmitting = false;
        this.errorMessage = error.error?.message ?? 'Erreur lors de la création du donneur.';
      }
    });
  }

  /**
   * Vérifie si un champ est invalide après interaction utilisateur.
   */
  isInvalid(fieldName: string): boolean {
    const control = this.donneurForm.get(fieldName);
    return !!control && control.invalid && control.touched;
  }
}

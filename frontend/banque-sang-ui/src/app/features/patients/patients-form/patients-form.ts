import { Component, EventEmitter, Input, OnInit, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';

import { PatientService } from '../../../core/services/patient.service';
import {
  CreatePatientRequest,
  Patient,
  UpdatePatientRequest
} from '../../../shared/models/patient.model';

@Component({
  selector: 'app-patient-form',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './patients-form.html',
  styleUrl: './patients-form.css',
})
export class PatientForm implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly patientService = inject(PatientService);

  @Input() patientToEdit: Patient | null = null;

  @Output() closed = new EventEmitter<void>();
  @Output() patientSaved = new EventEmitter<void>();

  isSubmitting = false;
  errorMessage = '';

  groupesSanguins = ['A+', 'A-', 'B+', 'B-', 'AB+', 'AB-', 'O+', 'O-'];

  patientForm = this.fb.group({
    idService: [''],
    patientNom: ['', [Validators.required]],
    patientPrenom: ['', [Validators.required]],
    patientAdresse: ['', [Validators.required]],
    patientTelephone: ['', [Validators.required]],
    patientDateNaissance: ['', [Validators.required]],
    patientGroupeSanguin: ['', [Validators.required]]
  });

  /**
   * Préremplit le formulaire si on modifie un patient existant.
   */
  ngOnInit(): void {
    this.fillFormForEdit();
  }

  /**
   * Remplit le formulaire avec les informations du patient sélectionné.
   */
  private fillFormForEdit(): void {
    if (!this.patientToEdit) {
      return;
    }

    this.patientForm.patchValue({
      idService: this.patientToEdit.idService ? String(this.patientToEdit.idService) : '',
      patientNom: this.patientToEdit.patientNom,
      patientPrenom: this.patientToEdit.patientPrenom,
      patientAdresse: this.patientToEdit.patientAdresse,
      patientTelephone: this.patientToEdit.patientTelephone,
      patientDateNaissance: this.patientToEdit.patientDateNaissance.slice(0, 10),
      patientGroupeSanguin: this.patientToEdit.patientGroupeSanguin
    });
  }

  /**
   * Ferme la fenêtre modale.
   */
  close(): void {
    this.closed.emit();
  }

  /**
   * Soumet le formulaire.
   * En mode création, appelle POST.
   * En mode modification, appelle PUT.
   */
  onSubmit(): void {
    this.errorMessage = '';

    if (this.patientForm.invalid) {
      this.patientForm.markAllAsTouched();
      return;
    }

    if (this.patientToEdit) {
      this.updatePatient();
      return;
    }

    this.createPatient();
  }

  /**
   * Crée un nouveau patient.
   */
  private createPatient(): void {
    const formValue = this.patientForm.getRawValue();

    const request: CreatePatientRequest = {
      idService: formValue.idService ? Number(formValue.idService) : null,
      patientNom: formValue.patientNom ?? '',
      patientPrenom: formValue.patientPrenom ?? '',
      patientAdresse: formValue.patientAdresse ?? '',
      patientTelephone: formValue.patientTelephone ?? '',
      patientDateNaissance: formValue.patientDateNaissance ?? '',
      patientGroupeSanguin: formValue.patientGroupeSanguin ?? ''
    };

    this.isSubmitting = true;

    this.patientService.create(request).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.patientSaved.emit();
      },
      error: (error) => {
        this.isSubmitting = false;
        this.errorMessage = error.error?.message ?? 'Erreur lors de la création du patient.';
      }
    });
  }

  /**
   * Modifie un patient existant.
   */
  private updatePatient(): void {
    if (!this.patientToEdit) {
      return;
    }

    const formValue = this.patientForm.getRawValue();

    const request: UpdatePatientRequest = {
      idService: formValue.idService ? Number(formValue.idService) : null,
      patientNom: formValue.patientNom ?? '',
      patientPrenom: formValue.patientPrenom ?? '',
      patientAdresse: formValue.patientAdresse ?? '',
      patientTelephone: formValue.patientTelephone ?? '',
      patientDateNaissance: formValue.patientDateNaissance ?? '',
      patientGroupeSanguin: formValue.patientGroupeSanguin ?? ''
    };

    this.isSubmitting = true;

    this.patientService.update(this.patientToEdit.idPatient, request).subscribe({
      next: () => {
        this.isSubmitting = false;
        this.patientSaved.emit();
      },
      error: (error) => {
        this.isSubmitting = false;
        this.errorMessage = error.error?.message ?? 'Erreur lors de la modification du patient.';
      }
    });
  }

  /**
   * Vérifie si un champ est invalide après interaction.
   */
  isInvalid(fieldName: string): boolean {
    const control = this.patientForm.get(fieldName);
    return !!control && control.invalid && control.touched;
  }
}

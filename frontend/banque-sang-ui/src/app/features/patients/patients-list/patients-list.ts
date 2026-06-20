import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PatientService } from '../../../core/services/patient.service';
import { Patient } from '../../../shared/models/patient.model';
import { PatientForm } from '../patients-form/patients-form';

@Component({
  selector: 'app-patients-list',
  imports: [CommonModule, PatientForm],
  templateUrl: './patients-list.html',
  styleUrl: './patients-list.css',
})
export class PatientsList implements OnInit {
  private readonly patientService = inject(PatientService);

  patients: Patient[] = [];

  isLoading = false;
  errorMessage = '';
  successMessage = '';

  showPatientForm = false;
  selectedPatient: Patient | null = null;

  /**
   * Charge les patients à l'ouverture de la page.
   */
  ngOnInit(): void {
    this.loadPatients();
  }

  /**
   * Charge la liste des patients depuis l'API.
   */
  loadPatients(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.patientService.getAll().subscribe({
      next: (patients) => {
        this.patients = patients;
        this.isLoading = false;
      },
      error: () => {
        this.errorMessage = 'Impossible de charger les patients.';
        this.isLoading = false;
      }
    });
  }

  /**
   * Ouvre le formulaire en mode création.
   */
  openPatientForm(): void {
    this.errorMessage = '';
    this.successMessage = '';
    this.selectedPatient = null;
    this.showPatientForm = true;
  }

  /**
   * Ouvre le formulaire en mode modification.
   */
  editPatient(patient: Patient): void {
    this.errorMessage = '';
    this.successMessage = '';
    this.selectedPatient = patient;
    this.showPatientForm = true;
  }

  /**
   * Ferme la modal.
   */
  closePatientForm(): void {
    this.showPatientForm = false;
    this.selectedPatient = null;
  }

  /**
   * Méthode appelée après création ou modification réussie.
   */
  onPatientSaved(): void {
    this.showPatientForm = false;
    this.selectedPatient = null;
    this.successMessage = 'Patient enregistré avec succès.';
    this.loadPatients();
  }
}

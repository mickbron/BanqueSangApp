export interface Patient {
  idPatient: number;
  idService?: number | null;
  patientNom: string;
  patientPrenom: string;
  patientAdresse: string;
  patientTelephone: string;
  patientDateNaissance: string;
  patientGroupeSanguin: string;
}

export interface CreatePatientRequest {
  idService?: number | null;
  patientNom: string;
  patientPrenom: string;
  patientAdresse: string;
  patientTelephone: string;
  patientDateNaissance: string;
  patientGroupeSanguin: string;
}

export interface UpdatePatientRequest {
  idService?: number | null;
  patientNom: string;
  patientPrenom: string;
  patientAdresse: string;
  patientTelephone: string;
  patientDateNaissance: string;
  patientGroupeSanguin: string;
}

export interface DemandeSang {
  idDemande: number;
  idPatient: number;
  patientNom: string;
  patientPrenom: string;
  patientGroupeSanguin: string;
  idSang?: number | null;
  idPersonnel: number;
  personnelNom: string;
  personnelPrenom: string;
  typeComposant: string;
  quantitePoche: number;
  urgence: string;
  dateDemande: string;
  commentaire?: string | null;
  statut: string;
}

export interface CreateDemandeSangRequest {
  idPatient: number;
  idSang?: number | null;
  idPersonnel: number;
  typeComposant: string;
  quantitePoche: number;
  urgence: string;
  dateDemande: string;
  commentaire?: string | null;
}

export interface UpdateDemandeSangStatutRequest {
  statut: string;
}

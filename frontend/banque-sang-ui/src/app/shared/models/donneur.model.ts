export interface Donneur {
  idDonneur: number;
  donneurNom: string;
  donneurPrenom: string;
  donneurAdresse: string;
  donneurTelephone: string;
  donneurDateNaissance: string;
  donneurGroupeSanguin: string;
  donneurStatutEligibilite: string;
  donneurDernierDon?: string | null;
}

export interface EligibiliteResult {
  donneurId: number;
  eligible: boolean;
  raisons: string[];
}

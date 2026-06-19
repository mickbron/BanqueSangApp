export interface TestBiologique {
  idTest: number;
  testNom: string;
}

export interface ResultatTest {
  idResultatTest: number;
  idPersonnel: number;
  personnelNom: string;
  personnelPrenom: string;
  idSang: number;
  idTest: number;
  testNom: string;
  dateTest: string;
  resultat: string;
  commentaire?: string | null;
  statutTest: string;
}

export interface CreateResultatTestRequest {
  idPersonnel: number;
  idSang: number;
  idTest: number;
  dateTest: string;
  resultat: string;
  commentaire?: string | null;
}

export interface CreateResultatTestResponse {
  idResultatTest: number;
  message: string;
}

export interface UpdateResultatTestRequest {
  resultat: string;
  commentaire?: string | null;
  statutTest: string;
}

export interface Sang {
  idSang: number;
  idDon: number;
  idCentre?: number | null;
  idHopital?: number | null;
  sangTypeComposant: string;
  sangDateCreation: string;
  sangDatePeremption: string;
  sangDisponible: boolean;
}

export interface StockGroupe {
  groupeSanguin: string;
  total: number;
  disponibles: number;
  indisponibles: number;
  perimees: number;
  prochesPeremption: number;
}

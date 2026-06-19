export interface Don {
  idDon: number;
  idDonneur: number;
  idCentre?: number | null;
  donDate: string;
  donStatut: string;
  donVolumeMl: number;
}

export interface CreateDonRequest {
  idDonneur: number;
  idCentre?: number | null;
  donDate: string;
  donVolumeMl: number;
  sangTypeComposant: string;
}

export interface CreateDonResponse {
  donId: number;
  sangId: number;
  statut: string;
  message: string;
}

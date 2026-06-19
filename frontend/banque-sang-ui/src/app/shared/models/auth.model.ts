//Ces interfaces représentent les données envoyées et reçues par l’API

export interface LoginRequest {
  login: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  role: string;
  expiration: string;
}

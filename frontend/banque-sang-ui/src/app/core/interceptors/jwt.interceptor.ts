import { HttpInterceptorFn } from '@angular/common/http';

/**
 * Ajoute automatiquement le token JWT dans les requêtes HTTP.
 */
export const jwtInterceptor: HttpInterceptorFn = (request, next) => {
  const token = localStorage.getItem('banque_sang_token');

  if (!token) {
    return next(request);
  }

  const requestWithToken = request.clone({
    setHeaders: {
      Authorization: `Bearer ${token}`
    }
  });

  return next(requestWithToken);
};

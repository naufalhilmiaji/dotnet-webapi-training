import { inject } from '@angular/core';
import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastService } from './toast.service';
import { AuthService } from './auth.service';
import { catchError, throwError } from 'rxjs';

export const httpErrorInterceptor: HttpInterceptorFn = (req, next) => {
  const router = inject(Router);
  const toast = inject(ToastService);
  const auth = inject(AuthService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      console.log('🛑 Http Error Interceptor:', error.status, req.url);

      if (error.status === 401) {
        if (req.url.includes('/auth/login')) {
          return throwError(() => error);
        }

        console.warn('🔒 Token expired / invalid');

        auth.logout();

        toast.error('Session expired. Please login again.');

        router.navigate(['/login'], {
          queryParams: { returnUrl: router.url },
        });
      }

      return throwError(() => error);
    }),
  );
};

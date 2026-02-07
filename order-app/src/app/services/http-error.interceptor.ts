import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';

export const httpErrorInterceptor: HttpInterceptorFn = (req, next) => {

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {

      if (err.status === 401) {
        console.group('🚨 401 UNAUTHORIZED');
        console.log('Request URL:', req.url);
        console.trace('Call stack');
        console.groupEnd();
      }

      return throwError(() => err);
    })
  );
};

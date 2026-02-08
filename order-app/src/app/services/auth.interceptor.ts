import { inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const platformId = inject(PLATFORM_ID);
  console.log('🛡️ Auth Interceptor: Intercepting', req.url);

  if (isPlatformBrowser(platformId)) {
    const token = localStorage.getItem('token');
    console.log('🛡️ Auth Interceptor: Token found?', !!token);

    if (token) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
      console.log('🛡️ Auth Interceptor: Token attached');
    }
  }

  return next(req);
};

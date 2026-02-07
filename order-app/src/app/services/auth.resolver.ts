import { ResolveFn } from '@angular/router';
import { inject } from '@angular/core';
import { AuthService } from './auth.service';

export const authResolver: ResolveFn<boolean> = () => {
  const auth = inject(AuthService);

  // paksa token dibaca lebih awal
  auth.getToken();

  return true;
};

import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { isPlatformBrowser } from '@angular/common';
import { Observable, tap } from 'rxjs';

interface LoginResponse {
  token: string;
  role: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private apiUrl = 'http://localhost:5197/api/auth';

  private token: string | null = null;
  private role: string | null = null;
  private isBrowser: boolean;

  constructor(
    private http: HttpClient,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);

    if (this.isBrowser) {
      this.token = localStorage.getItem('token');
      this.role = localStorage.getItem('role');
    }
  }

  init(): Promise<void> {
    return new Promise(resolve => {
      if (this.isBrowser) {
        this.token = localStorage.getItem('token');
        this.role = localStorage.getItem('role');
      }
      resolve();
    });
  }

  /**
   * Login user and persist auth state
   */
  login(username: string, password: string): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.apiUrl}/login`, { username, password })
      .pipe(
        tap(res => {
          this.token = res.token;
          this.role = res.role;

          if (this.isBrowser) {
            localStorage.setItem('token', res.token);
            localStorage.setItem('role', res.role);
          }
        })
      );
  }

  /**
   * Used by HTTP interceptor
   */
  getToken(): string | null {
    return this.token;
  }

  /**
   * Optional: role-based access
   */
  getRole(): string | null {
    return this.role;
  }

  /**
   * Used by auth guard
   * Must survive refresh and SSR
   */
  isLoggedIn(): boolean {
    const token = localStorage.getItem('token');
    return !!token;
  }

  /**
   * Clear auth state
   */
  logout(): void {
    this.token = null;
    this.role = null;

    if (this.isBrowser) {
      localStorage.clear();
    }
  }
}

import { Injectable, signal } from '@angular/core';

export interface Toast {
  title: string;
  message: string;
}

@Injectable({ providedIn: 'root' })
export class ToastService {

  toasts = signal<Toast[]>([]);

  success(message: string) {
    this.push('Success', message);
  }

  error(message: string) {
    this.push('Error', message);
  }

  info(message: string) {
    this.push('Info', message);
  }

  private push(title: string, message: string) {
    this.toasts.update(t => [...t, { title, message }]);

    setTimeout(() => {
      this.toasts.update(t => t.slice(1));
    }, 3000);
  }
}


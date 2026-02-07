import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="toast-container">
      <div class="toast" *ngFor="let t of toastService.toasts()">
        <strong>{{ t.title }}</strong>
        <p>{{ t.message }}</p>
      </div>
    </div>
  `,
  styleUrls: ['./toast.css']
})
export class ToastComponent {
  toastService = inject(ToastService);
}

import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { OrdersService } from '../../../services/orders.service';
import { ChangeDetectorRef } from '@angular/core';
import { SignalRService } from '../../../services/signalr.service';
import { ToastService } from '../../../services/toast.service';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './order-detail.html',
  styleUrl: './order-detail.css'
})
export class OrderDetailComponent implements OnInit {

  order: any | null = null;;
  loading = true;
  error = '';
  private isBrowser: boolean;

  constructor(
    private route: ActivatedRoute,
    private ordersService: OrdersService,
    private signalr: SignalRService,
    private cdr: ChangeDetectorRef,
    private toast: ToastService,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit() {
    if (!this.isBrowser) return;

    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;

    // 1️⃣ Load order dulu
    this.ordersService.getOrderById(id).subscribe({
      next: (res) => {
        this.order = res;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.error = 'Order not found';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });

    // 2️⃣ Connect SignalR SEKALI
    this.signalr.connect();

    this.signalr.onOrderStatusUpdated((orderId, status) => {
      if (!this.order || this.order.id !== orderId) return;

      this.order.status = status;

      // 🔔 Toast
      this.toast.success(`Order status updated to ${status}`);

      // 🔁 Paksa Angular refresh
      this.cdr.detectChanges();
    });
  }
}

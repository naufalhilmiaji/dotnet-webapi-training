import { Component, OnInit, Inject, PLATFORM_ID, ChangeDetectorRef } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { OrdersService } from '../../services/orders.service';
import { RouterModule } from '@angular/router';
import { SignalRService } from '../../services/signalr.service';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';
import { OrderItem } from '../../models/order-item.model';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './orders.html',
  styleUrl: './orders.css',
})
export class OrdersComponent implements OnInit {
  orders: any[] = [];
  products: Product[] = [];
  items: OrderItem[] = [];
  loading = true;
  error = '';
  private isBrowser: boolean;

  selectedProductId!: string;
  quantity = 1;

  totalAmount = 0;

  constructor(
    private ordersService: OrdersService,
    private productService: ProductService,
    private authService: AuthService,
    private cdr: ChangeDetectorRef,
    private signalR: SignalRService,
    @Inject(PLATFORM_ID) platformId: Object,
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit() {
    if (!this.isBrowser) return;

    if (!this.authService.isLoggedIn()) {
      console.warn('Auth not ready, skip loading orders/products');
      return;
    }

    this.signalR.connect();
    this.loadOrders();
    this.loadProducts();
  }

  loadOrders() {
    this.ordersService.getOrders().subscribe({
      next: (res) => {
        this.orders = res;
        this.loading = false;

        this.cdr.detectChanges();
      },
      error: (err) => {
        this.error = `Failed to load orders\n${err}`;
        this.loading = false;
        this.cdr.detectChanges();
      },
    });
  }

  loadProducts() {
    this.productService.getProducts().subscribe({
      next: (p) => (this.products = p),
      error: (err) => {
        if (err.status === 401) {
          console.warn('Products API unauthorized – token not ready yet');
          return;
        }

        this.error = 'Failed to load products';
      },
    });
  }

  calculateTotal() {
    this.totalAmount = this.items.reduce((sum, item) => sum + item.price * item.quantity, 0);
  }

  addItem() {
    const product = this.products.find((p) => p.id === this.selectedProductId);
    if (!product) return;

    const existing = this.items.find((i) => i.productId === product.id);

    if (existing) {
      existing.quantity += this.quantity;
    } else {
      this.items.push({
        productId: product.id,
        productName: product.name,
        price: product.price, // SNAPSHOT
        quantity: this.quantity,
      });
    }

    this.quantity = 1;
    this.calculateTotal();
  }

  removeItem(productId: string) {
    this.items = this.items.filter((i) => i.productId !== productId);
    this.calculateTotal();
  }
}

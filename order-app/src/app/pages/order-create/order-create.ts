import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';
import { Customer } from '../../models/customer.model';
import { CustomerService } from '../../services/customer.service';
import { OrderItem } from '../../models/order-item.model';
import { OrdersService } from '../../services/orders.service';
import { ToastService } from '../../services/toast.service';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './order-create.html',
  styles: [
    `
      .actions {
        display: flex;
        gap: 1rem;
        margin-bottom: 1rem;
      }
      .customer-select {
        margin-bottom: 2rem;
        padding: 1rem;
        border: 1px solid #ddd;
      }
    `,
  ],
})
export class OrderCreateComponent implements OnInit {
  products: Product[] = [];
  customers: Customer[] = [];
  items: OrderItem[] = [];

  selectedCustomerId: string | null = null;
  selectedProductId: string | null = null;
  quantity = 1;
  totalAmount = 0;

  loading = false;
  error = '';

  constructor(
    private productService: ProductService,
    private customerService: CustomerService,
    private ordersService: OrdersService,
    private toast: ToastService,
    @Inject(PLATFORM_ID) private platformId: Object,
  ) {}

  ngOnInit() {
    // ⛔ SSR → jangan call API protected
    if (!isPlatformBrowser(this.platformId)) {
      return;
    }

    this.loadProducts();
    this.loadCustomers();
  }

  loadCustomers() {
    this.customerService.getCustomers().subscribe({
      next: (res) => (this.customers = res),
      error: () => (this.error = 'Failed to load customers'),
    });
  }

  loadProducts() {
    this.productService.getProducts().subscribe({
      next: (p) => {
        this.products = p;
      },
      error: () => {
        this.error = 'Failed to load products';
      },
    });
  }

  addItem() {
    if (!this.selectedProductId) {
      this.error = 'Please select a product';
      return;
    }

    const product = this.products.find((p) => p.id === this.selectedProductId);
    if (!product) return;

    const existing = this.items.find((i) => i.productId === product.id);

    if (existing) {
      existing.quantity += this.quantity;
    } else {
      this.items.push({
        productId: product.id,
        productName: product.name,
        price: product.price,
        quantity: this.quantity,
      });
    }

    this.toast.success('Items have been added.');

    this.selectedProductId = null;
    this.quantity = 1;
    this.calculateTotal();
  }

  submitOrder() {
    if (!this.selectedCustomerId) {
      this.error = 'Please select a customer';
      return;
    }

    if (!this.items.length) {
      this.error = 'Order must contain at least one item';
      return;
    }

    const payload = {
      customerId: this.selectedCustomerId,
      items: this.items.map((i) => ({
        productId: i.productId,
        quantity: i.quantity,
      })),
    };

    this.loading = true;

    this.ordersService.createOrder(payload).subscribe({
      next: () => {
        this.toast.success('Order created successfully');
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to create order';
        this.loading = false;
      },
    });
  }

  removeItem(productId: string) {
    this.items = this.items.filter((i) => i.productId !== productId);
    this.toast.success('Items have been removed.');
    this.calculateTotal();
  }

  calculateTotal() {
    this.totalAmount = this.items.reduce((sum, i) => sum + i.price * i.quantity, 0);
  }
}

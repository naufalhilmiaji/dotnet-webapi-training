import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';
import { OrderItem } from '../../models/order-item.model';
import { OrdersService } from '../../services/orders.service';
import { ToastService } from '../../services/toast.service';


@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './order-create.html'
})
export class OrderCreateComponent implements OnInit {
  products: Product[] = [];
  items: OrderItem[] = [];

  selectedProductId: string | null = null;
  quantity = 1;
  totalAmount = 0;

  loading = false;
  error = '';

  constructor(
    private productService: ProductService,
    private ordersService: OrdersService,
    private toast: ToastService,
  ) { }

  ngOnInit() {
    this.loadProducts();
  }

  loadProducts() {
    this.productService.getProducts().subscribe({
      next: p => {
        this.products = p;
      }
    });

  }

  addItem() {
    if (!this.selectedProductId) {
      this.error = 'Please select a product';
      return;
    }

    const product = this.products.find(
      p => p.id === this.selectedProductId
    );
    if (!product) return;

    const existing = this.items.find(
      i => i.productId === product.id
    );

    if (existing) {
      existing.quantity += this.quantity;
    } else {
      this.items.push({
        productId: product.id,
        productName: product.name,
        price: product.price,
        quantity: this.quantity
      });
    }
    this.toast.success('Items have been added.');

    this.selectedProductId = null; // reset select (BENAR)
    this.quantity = 1;
    this.calculateTotal();
  }

  submitOrder() {
    if (!this.items.length) {
      this.error = 'Order must contain at least one item';
      return;
    }

    const payload = {
      items: this.items.map(i => ({
        productId: i.productId,   // 🔥 SOURCE OF TRUTH
        quantity: i.quantity
      }))
    };

    console.log('ORDER PAYLOAD', payload); // debug

    this.loading = true;

    this.ordersService.createOrder(payload).subscribe({
      next: () => {
        this.toast.success('Order created successfully');
      },
      error: () => {
        this.error = 'Failed to create order';
        this.loading = false;
      }
    });
  }

  removeItem(productId: string) {
    this.items = this.items.filter(i => i.productId !== productId);
    this.toast.success('Items have been removed.');
    this.calculateTotal();
  }

  calculateTotal() {
    this.totalAmount = this.items.reduce(
      (sum, i) => sum + i.price * i.quantity,
      0
    );
  }
}

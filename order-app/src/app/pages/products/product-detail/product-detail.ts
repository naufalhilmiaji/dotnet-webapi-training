import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../../services/product.service';
import { Product } from '../../../models/product.model';

@Component({
  standalone: true,
  imports: [CommonModule],
  template: `
    <h2>Product Detail</h2>

    <div *ngIf="loading">Loading...</div>

    <div *ngIf="!loading && product">
      <p><strong>Name:</strong> {{ product.name }}</p>
      <p><strong>Description:</strong> {{ product.description }}</p>
      <p><strong>Price:</strong> {{ product.price | number }}</p>
      <p><strong>Stock:</strong> {{ product.stock }}</p>

      <button (click)="edit()">Edit</button>
      <button (click)="back()">Back</button>
    </div>
  `
})
export class ProductDetailComponent implements OnInit {

  product: Product | null = null;
  loading = true;
  id: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: ProductService
  ) {}

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');

    if (!this.id) {
      this.loading = false;
      return;
    }

    this.service.getById(this.id).subscribe({
      next: (res) => {
        this.product = res;
        this.loading = false;
      },
      error: () => {
        // error sudah ditangani global toast
        this.loading = false;
      }
    });
  }

  edit() {
    if (this.id) {
      this.router.navigate(['/products/edit', this.id]);
    }
  }

  back() {
    this.router.navigate(['/products']);
  }
}

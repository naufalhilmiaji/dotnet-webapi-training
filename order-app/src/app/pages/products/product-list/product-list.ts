import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { ProductService } from '../../../services/product.service';
import { Product } from '../../../models/product.model';
import { Observable } from 'rxjs';
import { shareReplay } from 'rxjs/operators';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-list.html'
})
export class ProductListComponent {

  products$: Observable<Product[]>;

  constructor(
    private productService: ProductService,
    private router: Router
  ) {
    this.products$ = this.productService
      .getProducts()
      .pipe(shareReplay(1));
  }

  view(id: string) {
    this.router.navigate(['/products', id]);
  }

  edit(id: string) {
    this.router.navigate(['/products/edit', id]);
  }

  remove(id: string) {
    if (!confirm('Delete this product?')) return;

    this.productService.delete(id).subscribe({
      next: () => {
        // re-fetch after delete
        this.products$ = this.productService
          .getProducts()
          .pipe(shareReplay(1));
      }
    });
  }
}

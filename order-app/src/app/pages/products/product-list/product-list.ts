import { Component, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';
import { ProductService } from '../../../services/product.service';
import { Product } from '../../../models/product.model';
import { Observable, of } from 'rxjs';
import { shareReplay } from 'rxjs/operators';
import { AuthService } from '../../../services/auth.service';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-list.html'
})
export class ProductListComponent {

  products$: Observable<Product[]> = of([]);
  isAdmin = false;

  constructor(
    private productService: ProductService,
    private router: Router,
    private auth: AuthService,
    @Inject(PLATFORM_ID) platformId: Object
  ) {
    if (isPlatformBrowser(platformId)) {
      // ✅ browser only
      this.products$ = this.productService
        .getProducts()
        .pipe(shareReplay(1));
    }
    this.isAdmin = this.auth.getRole() === 'ADMIN';
  }

  view(id: string) {
    this.router.navigate(['/products', id]);
  }

  edit(id: string) {
    this.router.navigate(['/products/edit', id]);
  }

  remove(id: string) {
    if (!confirm('Delete this product?')) return;

    this.productService.delete(id).subscribe(() => {
      // refresh list (browser only)
      this.products$ = this.productService
        .getProducts()
        .pipe(shareReplay(1));
    });
  }

  create() {
    this.router.navigate(['/products/create']);
  }
}

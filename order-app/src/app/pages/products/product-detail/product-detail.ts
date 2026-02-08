import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../../services/product.service';
import { Product } from '../../../models/product.model';
import { ToastService } from '../../../services/toast.service';
import { AuthService } from '../../../services/auth.service';
import { finalize, timeout, catchError, TimeoutError, throwError } from 'rxjs';

@Component({
  standalone: true,
  imports: [CommonModule],
  templateUrl: './product-detail.html',
})
export class ProductDetailComponent implements OnInit {
  product: Product | null = null;
  loading = false;
  id: string | null = null;
  private isBrowser: boolean;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private service: ProductService,
    private toast: ToastService,
    private auth: AuthService,
    @Inject(PLATFORM_ID) platformId: Object,
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  ngOnInit() {
    if (!this.isBrowser) {
      return;
    }

    this.loading = true;

    this.id = this.route.snapshot.paramMap.get('id');
    if (!this.id) {
      this.loading = false;
      return;
    }

    console.log('🛒 ProductDetail: Fetching product', this.id);
    this.service
      .getById(this.id)
      .pipe(
        timeout(5000),
        catchError((err) => {
          if (err instanceof TimeoutError) {
            console.error('🛒 ProductDetail: Request timed out!');
            this.toast.error('Request timed out. Please try again.');
          }
          return throwError(() => err);
        }),
        finalize(() => {
          console.log('🛒 ProductDetail: Finalize - loading set to false');
          this.loading = false;
        }),
      )
      .subscribe({
        next: (res) => {
          console.log('🛒 ProductDetail: Success', res);
          this.product = res;
        },
        error: (err) => {
          console.error('🛒 ProductDetail: Error', err);
          if (err.status === 401) {
            this.toast.error('Unauthorized');
          } else {
            this.toast.error('Product not found');
          }

          this.router.navigate(['/products']);
        },
      });
  }

  edit() {
    if (this.id) {
      this.router.navigate(['/products/edit', this.id]);
    }
  }

  delete() {
    if (!this.product) return;
    if (!confirm('Delete this product?')) return;

    this.service.delete(this.product.id).subscribe({
      next: () => {
        this.toast.success('Product deleted');
        this.router.navigate(['/products']);
      },
      error: () => {
        this.toast.error('Failed to delete product');
      },
    });
  }

  back() {
    this.router.navigate(['/products']);
  }

  get isAdmin() {
    return this.auth.getRole() === 'ADMIN';
  }
}

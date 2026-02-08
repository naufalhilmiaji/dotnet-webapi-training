import { Component, OnInit, Inject, PLATFORM_ID } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../../services/product.service';
import { ToastService } from '../../../services/toast.service';

@Component({
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './product-form.html',
  styleUrls: ['./product-form.css'],
})
export class ProductFormComponent implements OnInit {
  form: FormGroup;
  isEdit = false;
  loading = false;
  id: string | null = null;
  private isBrowser: boolean;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private service: ProductService,
    private toast: ToastService,
    @Inject(PLATFORM_ID) platformId: Object,
  ) {
    this.isBrowser = isPlatformBrowser(platformId);

    this.form = this.fb.nonNullable.group({
      name: ['', Validators.required],
      description: [''],
      price: [0, [Validators.required, Validators.min(0)]],
      stock: [0, [Validators.required, Validators.min(0)]],
    });
  }

  ngOnInit() {
    if (!this.isBrowser) return;

    this.id = this.route.snapshot.paramMap.get('id');
    if (!this.id) return;

    this.isEdit = true;
    this.loading = true;

    this.service.getById(this.id).subscribe({
      next: (product) => {
        this.form.patchValue(product);
        this.loading = false;
      },
      error: () => {
        this.toast.error('Product not found');
        this.router.navigate(['/products']);
      },
    });
  }

  submit() {
    if (this.form.invalid || this.loading) return;

    this.loading = true;

    if (this.isEdit && this.id) {
      this.service.update(this.id, this.form.value).subscribe({
        next: () => {
          this.toast.success('Product updated');
          this.router.navigate(['/products']);
        },
        error: () => {
          this.toast.error('Failed to update product');
          this.loading = false;
        },
      });
    } else {
      this.service.create(this.form.value).subscribe({
        next: () => {
          this.toast.success('Product created');
          this.router.navigate(['/products']);
        },
        error: () => {
          this.toast.error('Failed to create product');
          this.loading = false;
        },
      });
    }
  }

  cancel() {
    this.router.navigate(['/products']);
  }
}

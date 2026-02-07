import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  Validators,
  FormGroup
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductService } from '../../../services/product.service';

@Component({
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <h2>{{ isEdit ? 'Edit' : 'Create' }} Product</h2>

    <form [formGroup]="form" (ngSubmit)="submit()">
      <input formControlName="name" placeholder="Name" />
      <input formControlName="description" placeholder="Description" />
      <input type="number" formControlName="price" placeholder="Price" />
      <input type="number" formControlName="stock" placeholder="Stock" />

      <button type="submit" [disabled]="form.invalid">
        Save
      </button>
      <button type="button" (click)="cancel()">Cancel</button>
    </form>
  `
})
export class ProductFormComponent implements OnInit {

  form: FormGroup;
  isEdit = false;
  id: string | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private service: ProductService
  ) {
    this.form = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      price: [0, [Validators.required, Validators.min(0)]],
      stock: [0, [Validators.required, Validators.min(0)]]
    });
  }

  ngOnInit() {
    this.id = this.route.snapshot.paramMap.get('id');

    if (this.id) {
      this.isEdit = true;

      this.service.getById(this.id).subscribe(product => {
        this.form.patchValue(product);
      });
    }
  }

  submit() {
    if (this.form.invalid) return;

    if (this.isEdit && this.id) {
      this.service
        .update(this.id, this.form.value)
        .subscribe(() => this.router.navigate(['/products']));
    } else {
      this.service
        .create(this.form.value)
        .subscribe(() => this.router.navigate(['/products']));
    }
  }

  cancel() {
    this.router.navigate(['/products']);
  }
}

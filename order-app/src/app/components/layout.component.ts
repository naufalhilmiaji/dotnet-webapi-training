import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <nav class="navbar">
      <div class="brand">OrderApp</div>

      <div class="nav-links">
        <a routerLink="/orders" routerLinkActive="active">Orders</a>
        <a routerLink="/orders/create" routerLinkActive="active">Create Order</a>
        <a routerLink="/products" routerLinkActive="active">Products</a>
      </div>
    </nav>

    <main class="container">
      <router-outlet />
    </main>
  `,
  styleUrls: ['./layout.css']
})
export class LayoutComponent {}

import { Routes } from '@angular/router';

import { LoginComponent } from './pages/login/login';
import { OrdersComponent } from './pages/orders/orders';
import { OrderDetailComponent } from './pages/orders/order-detail/order-detail';
import { OrderCreateComponent } from './pages/order-create/order-create';

import { ProductListComponent } from './pages/products/product-list/product-list';
import { ProductFormComponent } from './pages/products/product-form/product-form';
import { ProductDetailComponent } from './pages/products/product-detail/product-detail';

import { LayoutComponent } from './components/layout.component';
import { authGuard } from './services/auth.guard';

export const routes: Routes = [

  // 🔓 PUBLIC (NO NAVBAR)
  {
    path: 'login',
    component: LoginComponent
  },

  {
    path: '',
    component: LayoutComponent,
    canActivate: [authGuard],
    children: [

      // Orders
      { path: 'orders', component: OrdersComponent },
      { path: 'orders/create', component: OrderCreateComponent },
      { path: 'orders/:id', component: OrderDetailComponent },

      // Products
      { path: 'products', component: ProductListComponent },
      { path: 'products/create', component: ProductFormComponent },
      { path: 'products/edit/:id', component: ProductFormComponent },
      { path: 'products/:id', component: ProductDetailComponent },

      // default
      { path: '', redirectTo: 'orders', pathMatch: 'full' }
    ]
  },

  // fallback
  { path: '**', redirectTo: '' }
];

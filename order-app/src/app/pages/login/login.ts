import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
})
export class LoginComponent {
  username = '';
  password = '';
  error = '';
  loading = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute,
  ) {}

  /**
   * Handle login submit
   */
  login(): void {
    if (!this.username || !this.password) {
      this.error = 'Username and password are required';
      return;
    }

    this.error = '';
    this.loading = true;

    this.authService.login(this.username, this.password).subscribe({
      next: () => {
        const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') || '/orders';

        this.router.navigateByUrl(returnUrl);
      },
      error: (err) => {
        this.error = err.error?.message || err.error || 'Invalid username or password';
        this.loading = false;
      },
    });
  }
}

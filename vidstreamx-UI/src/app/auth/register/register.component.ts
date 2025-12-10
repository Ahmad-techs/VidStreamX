import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {

   username = '';
  email = '';
  password = '';

  constructor(private auth: AuthService, private router: Router, private snack: MatSnackBar) {}

submit() {
  this.auth.register({
    username: this.username,
    email: this.email,
    password: this.password
  }).subscribe({
    next: () => {
      this.snack.open('Registered. Please login.', 'Close', { duration: 2000 });
      this.router.navigate(['/login']);
    },
    error: err => this.snack.open(err?.error || 'Register failed', 'Close', { duration: 3000 })
  });
}
}

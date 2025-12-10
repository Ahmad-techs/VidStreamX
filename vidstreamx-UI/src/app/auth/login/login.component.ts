import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from 'src/app/core/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  email = '';
  password = '';

  constructor(
    private auth: AuthService,
    private router: Router,
    private snack: MatSnackBar
  ) {}

  submit() {
    this.auth.login({ email: this.email, password: this.password }).subscribe({
      next: (res: any) => {
        // Store session in AuthService if you have it
        this.auth.setSession(res.token, res.userId, res.username);
debugger
        // Store user in localStorage for Feed / Like functionality
        const user = {
          userId: res.userId,    // GUID from backend
          username: res.username,
          token: res.token       // JWT token
        };
        localStorage.setItem('vid_user', JSON.stringify(user));

        this.snack.open('Login successful', 'Close', { duration: 2000 });
        this.router.navigate(['/']); // redirect to feed or home
      },
      error: (err: { error: any; }) =>
        this.snack.open(err?.error || 'Login failed', 'Close', { duration: 3000 })
    });
  }
}

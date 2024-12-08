import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule,FormsModule,HttpClientModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  credentials = {
    email: '',
    password: ''
  };

  constructor(private authService: AuthService,private router:Router,private toastr:ToastrService) {}

  // login(): void {
  //   this.authService.login(this.credentials).subscribe({
  //     next: (response) => {
  //       console.log('Login successful:', response);
  //       alert('Login successful!');
  //       // Store the token in localStorage or sessionStorage
  //       localStorage.setItem('token', response.token);
  //     },
  //     error: (err) => {
  //       console.error('Error during login:', err);
  //       alert('Login failed. Please try again.');
  //     }
  //   });
  // }
  login(): void {
    if (!this.credentials.email || !this.credentials.password) {
      this.toastr.error('Please fill in both email and password.', 'Input Required');
      return;
    }
    else{

    this.authService.login(this.credentials).subscribe({
      next: (response) => {
        if (response.token) {
          console.log('Login successful, token received:', response.token);
          this.authService.setToken(response.token);  // Store token in localStorage
          this.redirectBasedOnRole();  // Redirect based on role
        }
      },
      error: (err) => {
        console.error('Error during login:', err);  // Log the error response
       this.toastr.error('Login failed. Please try again.','Retry',);
      }
    });
  }
  }
  
  
  
  redirectBasedOnRole() {
    const role = this.authService.getRole();
    if (role === 'Admin') {
      this.router.navigate(['/admin-dashboard']);
    } else if (role === 'Customer') {
      this.router.navigate(['/home']);
    } else if (role === 'RestaurantOwner') {
      this.router.navigate(['/dashboard']);
    } else if (role === 'DeliveryDriver') {
      this.router.navigate(['/delivery-driver']);
    }
  }
  
}

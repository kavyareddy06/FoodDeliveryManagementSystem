import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule,FormsModule,HttpClientModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  user = {
    username: '',
    email: '',
    phoneNumber: '',
    role: 'Customer', // Default role, but user can change it
    password: '',
  };

  constructor(private authService: AuthService, private router: Router,private toastr:ToastrService) {}

  register(): void {
    if (!this.user.username || !this.user.email || !this.user.phoneNumber || !this.user.role || !this.user.password) {
      this.toastr.error('Please fill all the required fields.', 'Input Required');
    }
    else{
    this.authService.register(this.user).subscribe({
      next: (response) => {
        console.log('Registration successful:', response);
       this.toastr.success('User registered successfully!','Registered');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        console.error('Error during registration:', err);
        this.toastr.error('Registration failed. Please try again.');
      }
    });
  }
  }
  
}

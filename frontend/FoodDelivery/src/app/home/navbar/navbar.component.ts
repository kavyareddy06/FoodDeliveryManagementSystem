import { Component, HostListener } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CartService } from '../../services/cart.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RestaurantListComponent } from '../../restaurant/restaurant-list/restaurant-list.component';
import { HomeComponent } from '../home.component';
import { SearchComponent } from '../../search/search.component';
import { OrderstatusComponent } from '../../restaurant/orderflow/orderstatus/orderstatus.component';
import { AboutComponent } from '../about/about.component';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, FormsModule,RestaurantListComponent,HomeComponent,SearchComponent,OrderstatusComponent,RouterLink,AboutComponent],
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  isLoggedIn: boolean = false;  // Flag to check if user is logged in
  // To hold the fetched username
    // To toggle mobile menu visibility
  cartItemCount: number = 0;  // To display the cart item count
  username='';
  isMobileMenuOpen = false;
  isDropdownOpen=false;
  isScrolled = false;
  constructor(
    private router: Router,
    private authservice: AuthService,
    private cartService: CartService
  ) {}

  toggleDropdown() {
    this.isDropdownOpen = !this.isDropdownOpen;
  }

  toggleMobileMenu() {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
  }
  goToCart(): void {
    this.router.navigate(['/cart']);  // Adjust the route if needed
  }

  profile() {
    console.log('Navigate to profile');
  }

  orders():void{
    this.router.navigate(['/orderstatus']);
  }

  @HostListener('window:scroll', [])
  onWindowScroll() {
    const offset = window.scrollY;
    this.isScrolled = offset > 50;
    const navbar = document.querySelector('.navbar');
    if (this.isScrolled) {
      navbar?.classList.add('scrolled');
    } else {
      navbar?.classList.remove('scrolled');
    }
  }

  ngOnInit(): void {
    // Fetch user details on component init
    this.authservice.getUserDetails().subscribe(
      (userDetails) => {
        console.log('User Details:', userDetails);  // Log to verify data
        this.username = userDetails.username; // Assign username here
        this.isLoggedIn = true;  // Set the logged-in flag
      },
      (error) => {
        console.error('Error fetching user details:', error);
      }
    );
     // Load cart items count
  }

  // Toggle mobile menu visibility
  // toggleMenu(): void {
  //   this.isMobileMenuOpen = !this.isMobileMenuOpen;
  // }

  // Navigate to cart page
  
  // Load the cart item count from the CartService
 

  // Log out the user
  logout(): void {
    localStorage.removeItem('restaurantId');
    localStorage.removeItem('role');
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }

}

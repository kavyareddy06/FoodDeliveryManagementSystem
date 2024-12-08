import { Component } from '@angular/core';
import { CartService } from '../../services/cart.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../home/navbar/navbar.component';
import { NavComponent } from '../../templates/nav/nav.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule,FormsModule,NavComponent],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent {
  cartItems: any[] = [];  // Holds the cart items
  totalPrice: number = 0;  // Holds the total price of items in the cart
  restaurantId: number|null=null ;
  userId: number  = 0;
  cartItemCount: number = 0;

  cuisineImageMap: { [key: string]: string } = {
    'coffee': 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQGkEHl3GW2nAW1J2EIijKtFeUvWeMl5KnK7g&s',
    'biryani': 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTC52b9Onr-ZXTzDWML7oLJjp1E_y5kuRPkfg&s',
    'burger': 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR7tWKyw6zBuQzMffAxLeNgZco249Qn1VkN9w&s',
    'taco': 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQJN038Yb-gHzcV11YvzEprUXLimqcNZk-3qg&s',
    'bowl': 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQrl3G1M0NI6M8ttEvusrN4OR_Kz7A26flxOA&s',
    'brownie': 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTaD300v0YUPzIiFu3yAPx6C9heqtcaINf8MA&s',
    'chinese': 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQ7w188O_VO1oNC3eKmmcppxEphlC0OpASCog&s',
    'chicken':'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSJb0kHteRlOTJWPjqO5YZ9TsUJXwbpnl-EIQ&s',
    'Starters':'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSi05CTNgqOfO8UWJlP2ibocAsgd425Nowpxw&s',
    'sandwich':'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR_JwVflJ0v6HgiM5I_vwef6GEkEA6bqcBXzw&s',
    'ice cream':'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR62U0V81BBQCnrRo6KX69Z7yWZfi_lOoKDpw&s',
    default: 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR6EuMRfHJAkjudmj5CCXUvB9Sy_QoGAgUPfw&s',
  };
  constructor(private cartService: CartService, private router: Router,private route:ActivatedRoute,private toastr:ToastrService) {}
 
  ngOnInit(): void {
   
    const storedUserId = localStorage.getItem('restaurantId'); // Fetch from localStorage

    if (storedUserId) {
      // Convert storedUserId to a number
      this.userId = +storedUserId; 
      this.loadCartItems();// Use the "+" operator to convert to a number
    } else {
      // Redirect to login if no userId is found in localStorage
      this.router.navigate(['/login']);
    }

    if (this.userId !== null) {
      // Fetch cart items only if userId is not null
      this.loadCartItems();
    } else {
      console.error('User ID not found in localStorage.');
    }
  }
  getCuisineImage(category: string): string {
    return this.cuisineImageMap[category] || this.cuisineImageMap['default'];
  }
  
  // Fetch cart items from the backend
  loadCartItems(): void {
    if (this.userId) {  // Ensure userId is valid
      this.cartService.getCartItems(this.userId).subscribe(
        (response: any) => {
          // Check if the response contains the `$values` property
          if (response && response.$values) {
            // Map through the $values array and extract necessary fields
            this.cartItems = response.$values.map((item: any) => ({
              cartItemId: item.cartItemId,
              quantity: item.quantity,
              price: item.price,
              menuItem: item.menuItem,
              imageUrl: this.getCuisineImage(item.menuItem.category),
            }));
            this.calculateTotalPrice();
            this.calculateCartItemCount();
          } else {
            console.error('Invalid response structure:', response);
          }
        },
        (error) => {
          console.error('Error loading cart items:', error);
        }
      );
    } else {
      console.error('Invalid userId:', this.userId);
    }
  }
  
  calculateCartItemCount(): void {
    this.cartItemCount = this.cartItems.reduce((count, cartItem) => {
      return count + cartItem.quantity; // Count all the items (sum of quantities)
    }, 0);
  }
  
  calculateTotalPrice(): void {
    this.totalPrice = this.cartItems.reduce((total, cartItem) => {
      return total + (cartItem.quantity * cartItem.price);
    }, 0);
  }
  

  increaseQuantity(item: any): void {
    item.quantity++;
    this.updateCartItem(item);
  }

  // Decrease the quantity of an item
  decreaseQuantity(item: any): void {
    if (item.quantity > 1) {
      item.quantity--;
      this.updateCartItem(item);
    }
  }
  updateCartItem(item: any): void {
    const userId = localStorage.getItem('restaurantId');
    const newQuantity = item.quantity;  // Get the updated quantity
    const cartItemId = item.cartItemId;  // Use cartItemId instead of itemId
  
    if (userId && cartItemId) {
      this.cartService.updateCartItem(userId, cartItemId, newQuantity).subscribe(() => {
        this.calculateTotalPrice(); // Recalculate total price after update
      });
    }
  }
  

  removeFromCart(item: any): void {
    const userId = localStorage.getItem('restaurantId');
    const cartItemId = item.cartItemId;  // Use cartItemId instead of itemId
  
    if (userId && cartItemId) {
      this.cartService.removeFromCart(userId, cartItemId).subscribe(() => {
        this.toastr.success('Item removed');
        this.loadCartItems();  // Reload the cart after removal
      });
    }
  }

  // calculateTotalPrice(): void {
  //   // Ensure cartItems is an array and handle empty array
  //   if (Array.isArray(this.cartItems)) {
  //     this.totalPrice = this.cartItems.reduce((total, item) => total + (item.price * item.quantity), 0);
  //   } else {
  //     this.totalPrice = 0;  // Default to 0 if cartItems is not an array
  //   }
  // }
  

  // Proceed to checkout
  checkout(): void {
    this.router.navigate(['/order']);
  }
  browseMenu():void{
    this.router.navigate(['/app-restaurant-list'])
  }
}

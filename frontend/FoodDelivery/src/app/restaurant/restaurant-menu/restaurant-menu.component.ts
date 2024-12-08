import { Component } from '@angular/core';
import { ActivatedRoute, Router, RouterOutlet } from '@angular/router';
import { RestaurantService } from '../../services/restaurant.service';
import { CommonModule } from '@angular/common';
import { HttpHeaders } from '@angular/common/http';
import { CartService } from '../../services/cart.service';
import { AuthService } from '../../services/auth.service';
import { ToastrModule, ToastrService } from 'ngx-toastr';
import { firstValueFrom } from 'rxjs';
import { PromotionService } from '../../services/promotion.service';
import { NavbarComponent } from '../../home/navbar/navbar.component';
import { NavComponent } from '../../templates/nav/nav.component';
@Component({
  selector: 'app-restaurant-menu',
  standalone: true,
  imports: [CommonModule,NavbarComponent,RouterOutlet,NavComponent],
  templateUrl: './restaurant-menu.component.html',
  styleUrl: './restaurant-menu.component.css'
})
export class RestaurantMenuComponent {
  restaurantId: number=0 ;
  restaurants: any[] = [];
  menuItems: any[] = [];
  promotions: any[] = [];
  currentIndex: number = 0;
  currentPage: number = 1;
  itemsPerPage: number = 6; // Number of restaurants per page
  totalItems: number = 0;
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
  
  constructor(
    private route: ActivatedRoute,
    private restaurantService: RestaurantService,
    private cartService:CartService,
    private authService:AuthService,
    private router:Router,
    private toastr:ToastrService,
    private promotionService: PromotionService
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.restaurantId = +params['restaurantId'];  
     // localStorage.setItem('resId',this.restaurantId);
      this.getMenuItems(this.restaurantId);  
      this.getPromotions(this.restaurantId);
     
    });
  }
  moveLeft(): void {
    this.currentIndex = this.currentIndex === 0 ? this.promotions.length - 1 : this.currentIndex - 1;
  }

  // Move to the right (next promotion)
  moveRight(): void {
    this.currentIndex = this.currentIndex === this.promotions.length - 1 ? 0 : this.currentIndex + 1;
  }
  getPromotions(restaurantId: number): void {
    this.promotionService.getPromotionsByres(restaurantId).subscribe(
      (promotions: any) => {
        this.promotions = promotions && promotions.$values ? promotions.$values : [];
        console.log('Promotions:', this.promotions);
      },
      (error) => {
        console.error('Error fetching promotions:', error);
      }
    );
  }
 
  getMenuItems(restaurantId: number): void {
    this.restaurantService.getRestaurantMenu(restaurantId).subscribe(
      (menuItems:any) => {
        console.log('Fetched menu items:', menuItems);  
       
        this.menuItems = menuItems && menuItems.$values ? menuItems.$values : [];  
       
      },
      (error) => {
        console.error('Error fetching menu items:', error);  
      }
    );
  }
  getCuisineImage(name: string): string {
    const normalizedName = name.trim().toLowerCase(); 
   // console.log(`Normalized Name: ${normalizedName}`); // Log normalized name
    return this.cuisineImageMap[normalizedName] || this.cuisineImageMap['default'];
  }
  addToCart(item: any): void {
    const userId = localStorage.getItem('restaurantId');
    const token = localStorage.getItem('token');
  
    if (!item.itemId ) {
      console.error('Item is missing itemId or quantity:', item);
      return;
    }
    if (!userId || !this.restaurantId || !token) {
      console.error('Missing userId, restaurantId, or token');
      return;
    }
  
    const storedRestaurantId = parseInt(localStorage.getItem('cartRestaurantId') || '', 10);
  
    if (storedRestaurantId && +storedRestaurantId !== this.restaurantId) {
      const confirmReplace = confirm(
        'Your cart contains items from a different restaurant. Do you want to replace them?'
      );
  
      if (!confirmReplace) {
        return;
      }
  
      // Clear the cart and then add the new item
      const apiUrl = `https://localhost:7120/api/Cart/clear?userId=${userId}`;
      this.cartService.clearCart(apiUrl).subscribe(
        () => {
          localStorage.setItem('cartRestaurantId', this.restaurantId.toString());
          this.addItemToCart(item, userId, token);
        },
        (error) => {
          console.error('Error clearing cart:', error);
        }
      );
    } else {
      if (!storedRestaurantId) {
        localStorage.setItem('cartRestaurantId', this.restaurantId.toString());
      }
      this.toastr.success('Item added to cart successfully');
      this.addItemToCart(item, userId, token);
     
    }
  }
  
  
  private addItemToCart(item: any, userId: string, token: string): void {
    item.quantity = (item.quantity || 0) + 1;
  
    const apiUrl = `https://localhost:7120/api/Cart/add?userId=${userId}&restaurantId=${this.restaurantId}&itemId=${item.itemId}&quantity=${item.quantity}`;
    
    this.cartService.addToCart(apiUrl, token).subscribe(
      (response) => {
        console.log("Item added successfully. Showing toastr message.");
        this.toastr.success('Item added to cart successfully');
      },
      (error) => {
        console.error('Error adding item to cart:', error);
      }
    );
  }
  clearCart(userId: number): void {
    const apiUrl = `https://localhost:7120/api/Cart/clear?userId=${userId}`;
    const token = localStorage.getItem('token') || '';
  
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
  
    this.cartService.clearCart(apiUrl).subscribe(
      () => {
        this.toastr.success('Cart cleared successfully');
        localStorage.removeItem('cartRestaurantId'); // Remove the stored restaurant ID
      },
      (error) => {
        this.toastr.error('Failed to clear the cart');
        console.error('Error clearing cart:', error);
      }
    );
  }
  
  goToCart(): void {
    // Navigate to the cart with restaurantId (make sure restaurantId is set)
    this.router.navigate([`/cart`]);
  }
  
}
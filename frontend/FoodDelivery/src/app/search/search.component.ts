import { Component } from '@angular/core';
import { RestaurantService } from '../services/restaurant.service';
import { MenuService } from '../services/menu.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { NavComponent } from '../templates/nav/nav.component';
import { NavbarComponent } from '../home/navbar/navbar.component';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [CommonModule,FormsModule,NavbarComponent,RouterModule],
  templateUrl: './search.component.html',
  styleUrl: './search.component.css'
})
export class SearchComponent {
  searchQuery: string = '';
  searchResults: any[] = [];
  isRestaurantSearch: boolean = true;
  isSearching: boolean = false;  // New flag to track whether we are searching

  constructor(
    private restaurantService: RestaurantService,
    private menuItemService: MenuService,
    private router:Router
  ) {}

  ngOnInit(): void {
    // Initialize any default values if necessary
  }

  onSearch(): void {
    if (this.searchQuery.trim()) {
      this.restaurantService.searchRestaurants(this.searchQuery)
        .subscribe({
          next: (data) => {
            this.searchResults = data;  // Assign transformed data to results
            console.log('Search Results:', this.searchResults); // Log the results for debugging
          },
          error: (err) => {
            console.error('Search error:', err);  // Handle errors
          }
        });
    } else {
      this.searchResults = [];  // Clear results if search query is empty
    }
  }
  // Search for Menu Items
  searchForMenuItems() {
    this.menuItemService.searchMenuItems(this.searchQuery).subscribe(
      (menuItems) => {
        this.searchResults = menuItems;
        this.isRestaurantSearch = false; // Found menu items
        this.isSearching = false; // Stop searching after results
      },
      (error) => {
        console.error('Error fetching menu items', error);
        this.isSearching = false;
      }
    );
  }
  goToRestaurantDetail(restaurantId: number): void {
    this.router.navigate([`/restaurant-menu/${restaurantId}`]); // Navigate to restaurant detail page
  }
}

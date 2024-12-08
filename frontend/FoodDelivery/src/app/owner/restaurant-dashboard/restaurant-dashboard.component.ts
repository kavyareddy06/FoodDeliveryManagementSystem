import { Component } from '@angular/core';
import { RestaurantService } from '../../services/restaurant.service';
import { Router, RouterLink } from '@angular/router';
import { MenuService } from '../../services/menu.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AddMenuItemComponent } from '../../menu-item/add-menu-item/add-menu-item.component';
import { ToastrService } from 'ngx-toastr';
import { PromotionComponent } from '../promotion/promotion.component';

@Component({
  selector: 'app-restaurant-dashboard',
  standalone: true,
  imports: [CommonModule,FormsModule,AddMenuItemComponent,RouterLink,PromotionComponent],
  templateUrl: './restaurant-dashboard.component.html',
  styleUrl: './restaurant-dashboard.component.css'
})
export class RestaurantDashboardComponent {
  restaurant: any = {};
  menuItems: any[] = [];
 // To store menu items
  newMenuItem: string = '';
  searchQuery: string = '';
  isEditingRestaurant: boolean = false;
  restaurantForm: any = {
    name: '',
    address: '',
    cuisineType: '',
    phone: '',
    hoursOfOperation: '',
    status: ''
  };
  isAddingRestaurant: boolean = false;
  //isEditingRestaurant: boolean = false; 
  constructor(
    private restaurantService: RestaurantService,
    private menuService: MenuService,
    private router: Router,
    private toastr:ToastrService
  ) {}

  ngOnInit(): void {
    this.getRestaurantDetails();
   
   // this.getMenuItems();
  }
  showAddRestaurantForm(): void {
    this.isAddingRestaurant = true;
  }
  
  getRestaurantDetails(): void {
    this.restaurantService.getMyRestaurant().subscribe(
      (data) => {
        this.restaurant = data;
        console.log(this.restaurant);
        this.populateInputFields();
        this.getMenuItems();
      },
      (error) => {
        console.error(error);
      }
    );
  }
  populateInputFields(): void {
    this.restaurantForm = { ...this.restaurant }; // Copy restaurant data into the form fields
  }
  getMenuItems(): void {
    this.menuService.getMenuItems(this.restaurant.restaurantId).subscribe(
      data => {
        console.log(this.restaurant.restaurantId);
        console.log('Menu items:', data);
        this.menuItems = data;  // Store the fetched menu items
      },
      error => {
        console.error('Error fetching menu items:', error);
      }
    );
  }
  addMenuItem(): void {
    const newItem = { name: this.newMenuItem };
    const restaurantId = this.restaurant.restaurantId;
    this.menuService.addMenuItem(newItem).subscribe(
      data => {
        this.menuItems.push(data); // Add the new item to the list
        this.newMenuItem = ''; // Clear the input field
      },
      error => {
        console.error('Error adding menu item:', error);
      }
    );
    this.router.navigate(['/add-menu-item', restaurantId]);
  }

  editMenuItem(item: any): void {
    this.router.navigate(['/menu-item/edit', item.id]);
  }

  deleteMenuItem(id: number): void {
    this.menuService.deleteMenuItem(id).subscribe(
      () => {
        this.menuItems = this.menuItems.filter(item => item.id !== id); // Remove the item from the list
      },
      error => {
        console.error('Error deleting menu item:', error);
      }
    );
  }
  addRestaurant(): void {
    // Retrieve ownerId (restaurant ID) from local storage
    const ownerId = localStorage.getItem('restaurantId');
    
    if (!ownerId) {
      console.error('Owner ID (restaurant ID) not found in local storage');
      return;
    }

    // Add the ownerId to the restaurant form data
    const restaurantData = {
      ...this.restaurantForm,
      ownerId: ownerId
    };

    this.restaurantService.addRestaurant(restaurantData).subscribe(
      (data) => {
        this.restaurant = data; // Update the restaurant details
        this.isAddingRestaurant = false; // Hide the form after adding
        this.toastr.success('Menu items added','Successfully Added')
      },
      (error) => {
        console.log(restaurantData);
        console.error('Error adding restaurant:', error);
      }
    );
  }
  editRestaurant(): void {
    this.isEditingRestaurant = true;
  }

  // Hide the edit form and go back to the dashboard
  cancelEdit(): void {
    this.isEditingRestaurant = false;
    this.getRestaurantDetails(); // Reset to the original restaurant details
  }

  // Update the restaurant with the new data
  updateRestaurant(): void {
    const ownerId = localStorage.getItem('restaurantId');
    
    if (!ownerId) {
      console.error('Owner ID not found in local storage');
      return;
    }

    const updatedRestaurantData = {
      ...this.restaurantForm,
      ownerId: ownerId // Ensure that the ownerId is included
    };

    this.restaurantService.updateRestaurant(this.restaurant.restaurantId, updatedRestaurantData).subscribe(
      (data) => {
        console.log(data);
      //  this.restaurant = data; // Update the restaurant details
        this.isEditingRestaurant = false; // Hide the edit form
        this.toastr.success('Restaurant updated successfully');
      },
      (error) => {
        console.error('Error updating restaurant:', error);
      }
    );
  }

 

  deleteRestaurant(): void {
    if (confirm('Are you sure you want to delete this restaurant?')) {
      this.restaurantService.deleteRestaurant(this.restaurant.restaurantId).subscribe(
        () => {
          this.router.navigate(['/']);  // Navigate to home after deletion
        },
        (error) => {
          console.error(error);
        }
      );
    }
  }
  promotionPage(){
    this.router.navigate(['/promotion']);
  }
}

import { Component } from '@angular/core';
import { InventoryService } from '../../services/inventory.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-admin-inventory',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-inventory.component.html',
  styleUrls: ['./admin-inventory.component.css']
})
export class AdminInventoryComponent {
  inventoryList: any[] = [];
  restaurantId: number = 1; // Example: Replace with dynamic restaurant ID
  isViewAll: boolean = true; // To toggle between all inventories and by restaurant ID
  newInventory: any = {
    restaurantId: this.restaurantId,
    itemId: 0, // Placeholder, will be set by user
    quantity: 0,
    reorderLevel: 20, // Default value for reorder level
    status: 'In Stock' // Default status
  };
  updateInventoryData: any = {
    restaurantId: this.restaurantId,
    itemId: 0, // Placeholder, will be set by user
    quantity: 0
  };

  // Pagination variables
  currentPage: number = 1; // Start with the first page
  pageSize: number = 5; // Show 5 items per page
  totalItems: number = 0; // Total number of items in the inventory list

  constructor(private inventoryService: InventoryService,private toastr:ToastrService) {}

  ngOnInit(): void {
    if (this.isViewAll) {
      this.loadAllInventories();
    } else {
      this.getInventoryByRestaurantId();
    }
  }

  // Switch to view all inventories
  viewAllInventories(): void {
    this.isViewAll = true;
    this.loadAllInventories();
  }

  // Switch to view inventories by restaurantId
  viewByRestaurantId(): void {
    this.isViewAll = false;
    this.getInventoryByRestaurantId();
  }

  // Get all inventory items for a restaurant
  loadAllInventories(): void {
    this.inventoryService.getAll().subscribe(
      (response) => {
        this.totalItems = response.$values.length;
        this.inventoryList = this.paginate(response.$values);  // Apply pagination
      },
      (error) => {
        console.error('Error fetching all inventories', error);
      }
    );
  }

  // Get inventory items for a specific restaurant ID
  getInventoryByRestaurantId(): void {
    this.inventoryService.getAllInventory(this.restaurantId).subscribe(
      (data) => {
        this.totalItems = data.$values.length;
        this.inventoryList = this.paginate(data.$values);  // Apply pagination
      },
      (error) => {
        console.error('Error fetching inventory by restaurant ID', error);
      }
    );
  }

  // Function to handle pagination logic
  paginate(data: any[]): any[] {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    return data.slice(startIndex, endIndex);
  }

  // Next page logic
  nextPage(): void {
    if (this.currentPage * this.pageSize < this.totalItems) {
      this.currentPage++;
      this.loadPageData();
    }
  }

  // Previous page logic
  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadPageData();
    }
  }

  // Load the data for the current page
  loadPageData(): void {
    if (this.isViewAll) {
      this.loadAllInventories();
    } else {
      this.getInventoryByRestaurantId();
    }
  }

  // Getter to calculate total pages
  get totalPages(): number {
    return Math.ceil(this.totalItems / this.pageSize);
  }

  // Add a new inventory item
  addInventory(): void {
    if (!this.newInventory.itemId || !this.newInventory.quantity) {
      console.error('Please provide all required fields');
      return;
    }
    this.inventoryService.addInventory(this.newInventory).subscribe(
      (response) => {
        this.isViewAll ? this.loadAllInventories() : this.getInventoryByRestaurantId(); // Refresh inventory list
       this.toastr.success('Inventory added successfully', response);
      },
      (error) => {
        console.error('Error adding inventory', error);
      }
    );
  }

  // Update an inventory item
  updateInventory(): void {
    if (!this.updateInventoryData.itemId || !this.updateInventoryData.quantity) {
      console.error('Please provide all required fields');
      return;
    }
    this.inventoryService.updateInventory(
      this.updateInventoryData.restaurantId,
      this.updateInventoryData.itemId,
      this.updateInventoryData.quantity
    ).subscribe(
      (response) => {
        this.isViewAll ? this.loadAllInventories() : this.getInventoryByRestaurantId(); // Refresh inventory list
       this.toastr.success('Inventory updated successfully', response);
      },
      (error) => {
        console.error('Error updating inventory', error);
      }
    );
  }
}

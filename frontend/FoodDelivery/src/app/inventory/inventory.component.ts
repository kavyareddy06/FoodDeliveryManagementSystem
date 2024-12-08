import { Component } from '@angular/core';
import { InventoryService } from '../services/inventory.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-inventory',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css'],
})
export class InventoryComponent {
  inventoryList: any[] = [];
  selectedRestaurantId: number = 0; // Default value for restaurant ID
  newInventory: any = {
    restaurantId: this.selectedRestaurantId,
    itemId: 0,
    quantity: 0,
    reorderLevel: 20,
    status: 'In Stock',
  };
  updateInventoryData: any = {
    restaurantId: this.selectedRestaurantId,
    itemId: 0,
    quantity: 0,
  };

  // Pagination variables
  currentPage: number = 1;
  itemsPerPage: number = 5; // Number of items per page
  totalPages: number = 1; // Total pages, will be calculated based on inventory length
  paginatedInventoryList: any[] = []; // List of items to display for the current page

  constructor(private inventoryService: InventoryService) {}

  ngOnInit(): void {
    this.getAllInventory();
  }

  getAllInventory(): void {
    if (!this.selectedRestaurantId) return;
    this.inventoryService.getAllInventory(this.selectedRestaurantId).subscribe(
      (response: any) => {
        // Extract the array from the response
        if (response?.$values) {
          this.inventoryList = response.$values;
          this.totalPages = Math.ceil(this.inventoryList.length / this.itemsPerPage); // Calculate total pages
          this.updatePaginatedInventoryList(); // Update the paginated list
        } else {
          console.error('Unexpected response format', response);
          this.inventoryList = [];
        }
      },
      (error) => {
        console.error('Error fetching inventory', error);
        this.inventoryList = []; // Reset the list on error
      }
    );
  }

  // Update the inventory list to show only the items for the current page
  updatePaginatedInventoryList(): void {
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    const endIndex = startIndex + this.itemsPerPage;
    this.paginatedInventoryList = this.inventoryList.slice(startIndex, endIndex);
  }

  // Handle pagination (Previous Page)
  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.updatePaginatedInventoryList();
    }
  }

  // Handle pagination (Next Page)
  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.updatePaginatedInventoryList();
    }
  }

  addInventory(): void {
    if (!this.newInventory.itemId || !this.newInventory.quantity) {
      alert('Please provide valid inputs for all fields.');
      return;
    }
    this.newInventory.restaurantId = this.selectedRestaurantId;
    this.inventoryService.addInventory(this.newInventory).subscribe(
      (response) => {
        alert('Inventory added successfully.');
        this.getAllInventory();
      },
      (error) => {
        console.error('Error adding inventory', error);
      }
    );
  }

  updateInventory(): void {
    if (!this.updateInventoryData.itemId || !this.updateInventoryData.quantity) {
      alert('Please provide valid inputs for all fields.');
      return;
    }
    this.updateInventoryData.restaurantId = this.selectedRestaurantId;
    this.inventoryService
      .updateInventory(
        this.updateInventoryData.restaurantId,
        this.updateInventoryData.itemId,
        this.updateInventoryData.quantity
      )
      .subscribe(
        (response) => {
          alert('Inventory updated successfully.');
          this.getAllInventory();
        },
        (error) => {
          console.error('Error updating inventory', error);
        }
      );
  }
}

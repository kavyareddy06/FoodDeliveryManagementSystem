import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  private apiBaseUrl = 'https://localhost:7120/api/Inventory'; // Replace with your API URL

  constructor(private http: HttpClient) {}

  getAllInventory(restaurantId: number): Observable<any> {
    return this.http.get<any>(`${this.apiBaseUrl}/all/${restaurantId}`)  // Extract the $values array
    ;
  }
  getAll(): Observable<any> {
    return this.http.get(`${this.apiBaseUrl}/all`);
  }

  // Add a new inventory item
  addInventory(inventory: any): Observable<any> {
    return this.http.post<any>(`${this.apiBaseUrl}/add`, inventory);
  }

  // Update an existing inventory item
  updateInventory(restaurantId: number, itemId: number,quantity:number): Observable<any> {
    return this.http.put<any>(`${this.apiBaseUrl}/update/${restaurantId}/${itemId}`,quantity);
  }

  // Get low stock items for a restaurant
  getLowStockItems(restaurantId: number): Observable<any> {
    return this.http.get<any>(`${this.apiBaseUrl}/low-stock/${restaurantId}`);
  }
}

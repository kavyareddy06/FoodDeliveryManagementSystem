import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

import { map, Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class DeliveryService {
  private apiUrl = 'https://localhost:7120/api/Delivery'; // Change this to your API base URL

  constructor(private http: HttpClient,private authService:AuthService) {}

  // Get all deliveries
  // delivery.service.ts
  getAllDeliveries(): Observable<any[]> {
    const headers = this.getAuthHeaders();
    return this.http.get<any>(`${this.apiUrl}`).pipe(
      map(response => response.$values)  // Extract the array from the response
    );
  }

  // Add a new delivery
  addDelivery(delivery: any): Observable<any> {
    const headers = this.getAuthHeaders();
    console.log(delivery);
    return this.http.post<any>(`${this.apiUrl}`, delivery, { headers });
  }

  // Delete a delivery
  deleteDelivery(deliveryId: number): Observable<any> {
    const headers = this.getAuthHeaders();
    return this.http.delete<any>(`${this.apiUrl}/${deliveryId}`, { headers });
  }
  getDeliveryDrivers(): Observable<any[]> {
    const token = localStorage.getItem('token'); // Get the token from localStorage
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`); // Add the token to headers

    return this.http.get<any>(`${this.apiUrl}/delivery-drivers`, { headers }).pipe(
      map(response => response['$values'] || []) // Extract the $values array or return an empty array
    );
  }
  // Update delivery status
  updateDeliveryStatus(deliveryId: number, status: string, location: string): Observable<any> {
    const headers = this.getAuthHeaders();
    console.log(status);
    console.log(location);
    return this.http.put<any>(`${this.apiUrl}/${deliveryId}/status`, { Status: status, Location: location }, { headers });
  }

  // Helper function to get Authorization headers with JWT token
  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken(); // Assuming getToken() returns the JWT token
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
  }
}

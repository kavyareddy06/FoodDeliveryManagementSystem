import { HttpClient, HttpEvent, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { Restaurant } from '../models/restaurant'; // Adjust the path as needed
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RestaurantService {
  private baseUrl = 'https://localhost:7120/api/Restaurants';

  constructor(private http: HttpClient) {}
  getRestaurants(options?: any): Observable<any[]> {
    return this.http.get<any[]>(this.baseUrl, {
      headers: options?.headers,
      observe: 'body' // Only return the body part of the response
    });
  }
  getRestaurantMenu(restaurantId: number): Observable<any> {
    // Retrieve the token from localStorage
    const apiUrl=`https://localhost:7120/api/MenuItems/restaurant`
    const token = localStorage.getItem('token'); // Ensure the token is stored with the key 'token'

    if (!token) {
      throw new Error('Authorization token is missing');
    }

    // Create HTTP headers with Authorization token
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    // Make the GET request with the headers and return the observable
    return this.http.get(`${apiUrl}/${restaurantId}`, { headers });
  }
  
  
  getMyRestaurant(): Observable<any> {
    // Get the token from local storage
    const token = localStorage.getItem('token');

    if (!token) {
      throw new Error('No token found in local storage');
    }

    // Set the Authorization header with the token
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    // Send the GET request with the token in headers
    return this.http.get(`${this.baseUrl}/my-restaurant`, { headers });
  }
  addRestaurant(newRestaurant: any): Observable<any> {
    const token = localStorage.getItem('token');
    if (!token) {
      throw new Error('No token found in local storage');
    }
  
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  
    return this.http.post(`${this.baseUrl}`, newRestaurant, { headers });
  }
  
  deleteRestaurant(restaurantId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${restaurantId}`);
  }

  updateRestaurant(restaurantId: number, updatedRestaurant: any): Observable<any> {
    const token = localStorage.getItem('token');
    if (!token) {
      throw new Error('No token found in local storage');
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.put(`${this.baseUrl}/${restaurantId}`, updatedRestaurant, { headers });
  }
  searchRestaurants(query: string): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
    });

    return this.http.get<any>(`${this.baseUrl}/search?restaurant=${query}`, { headers })
      .pipe(
        map(response => {
          console.log('Response:', response); // Log the response to inspect it

          // Transform or flatten the response if necessary
          const restaurants = response.$values.map((restaurant: any) => ({
            restaurantId: restaurant.restaurantId,
            name: restaurant.name,
            address: restaurant.address,
            cuisineType: restaurant.cuisineType,
            phone: restaurant.phone,
            hoursOfOperation: restaurant.hoursOfOperation,
            status: restaurant.status,
            menuItems: restaurant.menuItems.$values.map((item: any) => ({
              itemId: item.itemId,
              name: item.name,
              category: item.category,
              price: item.price,
              description: item.description,
              availability: item.availability
            }))
          }));

          return restaurants; // Return the transformed data
        })
      );
  }
}

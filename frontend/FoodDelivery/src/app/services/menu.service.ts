import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class MenuService {
  private baseUrl = 'https://localhost:7120/api/MenuItems';

  constructor(private http: HttpClient) {}

  getMenuItems(restaurantId:number): Observable<any> {
   // const restaurantId = localStorage.getItem('restaurantId');
    const token = localStorage.getItem('token');

    if (!restaurantId || !token) {
      throw new Error('Missing restaurantId or token');
    }

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    return this.http.get(`${this.baseUrl}/restaurant/${restaurantId}`, { headers });
  }

  addMenuItem(menuItem: any): Observable<any> {
    const token = localStorage.getItem('token');
    
    if (!token) {
      throw new Error('No token found');
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    });
 console.log(menuItem);
    return this.http.post<any>(this.baseUrl, menuItem, { headers });
  }

  deleteMenuItem(itemId: number): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
  
    return this.http.delete(`${this.baseUrl}/${itemId}`, { headers });
  }
  
  updateMenuItem(itemId: number, menuItem: any): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    
    const updatedItem = { ...menuItem, id: itemId };

    return this.http.put(`${this.baseUrl}/${itemId}`, updatedItem, {
      headers: headers,
      responseType: 'json'
    });
    
  }
  searchMenuItems(query: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}?search=${query}`);
  }
}

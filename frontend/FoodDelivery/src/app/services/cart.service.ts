import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { firstValueFrom, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private  apiUrl=`https://localhost:7120/api/Cart`
  constructor(private http: HttpClient) { }
  private getUserDetails() {
    const userId = localStorage.getItem('restaurantId');
    const token = localStorage.getItem('token');
    return { userId, token };
  }
  getCartId(userId: string): Observable<string> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    const url = `${this.apiUrl}/getCartId?userId=${userId}`;
    return this.http.get<string>(url, { headers });
  }
  
  // Method to add item to the cart
  addToCart(apiUrl: string, token: string): Observable<any> {
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.http.post(apiUrl, {}, { headers });
  }
  getCartItems(userId: number): Observable<any[]> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    const url = `${this.apiUrl}/${userId}`;
    return this.http.get<any[]>(url, { headers });
  }
  updateCartItem(userId: string, cartItemId: number, newQuantity: number): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    const url = `${this.apiUrl}/update/${cartItemId}?newQuantity=${newQuantity}`;
    return this.http.put(url, null, { headers });
  }
  // Remove item from cart
  removeFromCart(userId: string, itemId: number): Observable<any> {
    const { token } = this.getUserDetails();
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    const url = `${this.apiUrl}/remove/${itemId}`;
    return this.http.delete(url, { headers });
  }
  clearCart(apiUrl: string): Observable<void> {
    const token = localStorage.getItem('token') || '';
  
    // Ensure headers include the Authorization token
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
  
    // Return the HTTP DELETE request as an observable
    return this.http.delete<void>(apiUrl, { headers });
  }
  
  
  getCartItemCount(): Observable<number> {
    const userId = localStorage.getItem('restaurantId'); // Assuming you have userId stored
    const token = localStorage.getItem('token'); // Assuming you have token for authorization

    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    // Make an API call to get the cart items count
    return this.http.get<number>(`${this.apiUrl}/getItemCount?userId=${userId}`, { headers });
  }
}

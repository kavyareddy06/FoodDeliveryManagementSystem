import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private apiUrl = 'https://localhost:7120/api/Order'; 

  constructor(private http: HttpClient) {}
  private getAuthHeaders() {
    const token = localStorage.getItem('token');
    return {
      Authorization: `Bearer ${token}`,
    };
  }
  
  private getUserDetails() {
    const userId = localStorage.getItem('restaurantId');
    const token = localStorage.getItem('token');
    return { userId, token };
  }

  placeOrder(restauarantId:number,orderRequest: any): Observable<any> {
    console.log(orderRequest);
    const { token } = this.getUserDetails();
    const userId=localStorage.getItem('restaurantId');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    const baseUrl=`${this.apiUrl}/${userId}/${restauarantId}`
    return this.http.post<any>(baseUrl, orderRequest, { headers });
  }
  getUserOrders(userId: number) {
    return this.http.get(`${this.apiUrl}/user-orders/${userId}`, {
      headers: this.getAuthHeaders(),
    });
  }

  trackOrder(orderId: number) {
    return this.http.get(`${this.apiUrl}/track-order/${orderId}`, {
      headers: this.getAuthHeaders(),
    });
  }

  cancelOrder(orderId: number) {
    return this.http.put(`${this.apiUrl}/cancel-order/${orderId}`, null, {
      headers: this.getAuthHeaders(),
    });
  }
  updateOrderStatus(orderId: number, status: string) {
    return this.http.put(`${this.apiUrl}/update-status/${orderId}`, status, {
      headers: this.getAuthHeaders(),
    });
  }
  submitFeedback(feedback: any) {
    return this.http.post(`https://localhost:7120/api/Feedback`, feedback, {
      headers: this.getAuthHeaders(),
    }); 
  }
  getDriverDetails(url: string) {
    return this.http.get(url); 
  }
  
}

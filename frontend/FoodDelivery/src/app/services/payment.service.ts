import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private apiUrl = 'https://localhost:7120/api/Payment/process-payment';  // Your backend URL

  constructor(private http: HttpClient) {}

  processPayment(paymentDetails: any): Observable<any> {
    const token = localStorage.getItem('token');  // Retrieve token from localStorage
    let headers = new HttpHeaders();

    if (token) {
      headers = headers.set('Authorization', `Bearer ${token}`);  // Add token to header if available
    }

    return this.http.post<any>(this.apiUrl, paymentDetails, { headers });
  }
}

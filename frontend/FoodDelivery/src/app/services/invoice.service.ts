import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class InvoiceService {
  constructor(private http: HttpClient) {}

  // sendInvoice(orderId: number): Observable<any> {
  //    const apiUrl=`https://localhost:7120/api/invoice`
  //   return this.http.post(`${apiUrl}/send-invoice/${orderId}`, null);
  // }
}

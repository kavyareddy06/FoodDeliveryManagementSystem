import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderReportService {
  private apiUrl = 'https://localhost:7120/api';  // Backend API URL

  constructor(private http: HttpClient) {}

  // Fetch orders grouped by date
  getOrdersGroupedByDate(
    restaurantId: number,
    startDate: string,
    endDate: string,
    frequency: string
  ): Observable<any> {
    const url = `${this.apiUrl}/OrderReport/grouped-by-date?restaurantId=${restaurantId}&frequency=${frequency}&startDate=${startDate}&endDate=${endDate}`;
    return this.http.get<any[]>(url);
  }

  // Fetch orders grouped by category
  getOrdersGroupedByCategory(
    restaurantId: number,
    startDate: string,
    endDate: string
  ): Observable<any> {
    const url = `${this.apiUrl}/OrderReport/grouped-by-category?restaurantId=${restaurantId}&startDate=${startDate}&endDate=${endDate}`;
    return this.http.get<any[]>(url);
  }
}
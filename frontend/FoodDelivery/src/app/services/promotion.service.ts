import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PromotionService {
  private apiUrl='https://localhost:7120/api/Promotion';
  constructor(private http: HttpClient) {}
  getPromotionsByres(restaurantId?: number): Observable<any> {
    const token = localStorage.getItem('token');
  
    if (!token) {
      console.error('No token found in local storage.');
      throw new Error('Unauthorized: Token is missing.');
    }
  
    const headers = new HttpHeaders({ 'Authorization': `Bearer ${token}` });
  
    let url = this.apiUrl;
    if (restaurantId) {
      url = `${this.apiUrl}/restaurant/${restaurantId}`;
    }
  
    return this.http.get(url, { headers });
  }
  

  // Create a new promotion
  createPromotion(promotion: any): Observable<any> {
    const token = localStorage.getItem('token');  // Get the token from local storage
    if (!token) {
      throw new Error('No token found in local storage');
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,  // Add token to Authorization header
    });

    return this.http.post(this.apiUrl, promotion, { headers });
  }

  // Update an existing promotion
  updatePromotion(promotion: any): Observable<any> {
    const token = localStorage.getItem('token');  // Get the token from local storage
    if (!token) {
      throw new Error('No token found in local storage');
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,  // Add token to Authorization header
    });

    return this.http.put(`${this.apiUrl}/${promotion.promotionId}`, promotion, { headers });
  }

  // Method to delete a promotion
  deletePromotion(promotionId: number): Observable<any> {
    const token = localStorage.getItem('token');  // Get the token from local storage
    if (!token) {
      throw new Error('No token found in local storage');
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,  // Add token to Authorization header
    });

    return this.http.delete(`${this.apiUrl}/${promotionId}`, { headers });
  }

  // Method to get promotions (if needed)
  getPromotions(): Observable<any> {
    const token = localStorage.getItem('token');  // Get the token from local storage
    if (!token) {
      throw new Error('No token found in local storage');
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,  // Add token to Authorization header
    });

    return this.http.get(this.apiUrl, { headers });
  }
  applyDiscount(code: string, orderTotal: number): Observable<number> {
    const token = localStorage.getItem('token');
    if (!token) {
      throw new Error('User is not authenticated');
    }
  
    const discountRequest = { code, orderTotal };
  
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
  
    return this.http.post<{ discountedTotal: number }>(`${this.apiUrl}/apply-discount`, discountRequest, { headers })
      .pipe(
        map(response => response.discountedTotal),  // Extracting the discounted total from the response
        catchError(error => {
          console.error('Error applying discount:', error);
          throw error;
        })
      );
  }
  
  
}

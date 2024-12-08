import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Route, Router } from '@angular/router';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7120/api/Auth'; // ASP.NET Core API base URL

  constructor(private http: HttpClient,private router:Router) {}

  // Register API call
  register(user: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, user);
  }

  // Login API call
  login(credentials: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, credentials);
  }
  getUserDetails(): Observable<any> {
    const token = localStorage.getItem('token');
    const userId=localStorage.getItem('restaurantId');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    // Call API to get user details based on the token
    return this.http.get<any>(`${this.apiUrl}/${userId}`, { headers });
  }
  getUserDetailsById(userId: string): Observable<any> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

    // Fetch user details based on the userId
    return this.http.get<any>(`${this.apiUrl}/${userId}`, { headers });
  }
  // setToken(token: string): void {
  //   localStorage.setItem('token', token);
  //   // Decode JWT and save the role
  //   const decodedToken = JSON.parse(atob(token.split('.')[1]));
  //   localStorage.setItem('role', decodedToken.role);
  // }
  setToken(token: string): void {
    // Save the token in localStorage
    localStorage.setItem('token', token);
  
    // Decode the JWT
    const decodedToken = JSON.parse(atob(token.split('.')[1]));
  
    // Save the role in localStorage
    localStorage.setItem('role', decodedToken.role);
  
    // Extract restaurantId from the token's claims and save it in localStorage
    const restaurantId = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
    
    if (restaurantId) {
      localStorage.setItem('restaurantId', restaurantId);  // Save the restaurantId
    } else {
      console.error('Restaurant ID is missing in the token');
    }
  }
  
  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getRole(): string | null {
    return localStorage.getItem('role');
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('cartRestaurantId')
    localStorage.removeItem('role');
    this.router.navigate(['/login']);
  }
}

import { Component } from '@angular/core';
import { RestaurantService } from '../../services/restaurant.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { CommonModule } from '@angular/common';
import { HttpHeaders } from '@angular/common/http';
import { OneComponent } from '../../templates/one/one.component';

@Component({
  selector: 'app-restaurant-details',
  standalone: true,
  imports: [CommonModule,OneComponent],
  templateUrl: './restaurant-details.component.html',
  styleUrl: './restaurant-details.component.css'
})
export class RestaurantDetailsComponent {
  restaurants: any[] = [];
  cuisineImageMap: { [key: string]: string } = {
    'Mehfil': 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSS1GBuPiaWdSGWx9ULelofYRsXp6KEdy9o_w&s',
    'McDonald':'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTZ03ev2zfnbhNj0TlTD4uBp2H7C0opTs0Rig&s',
    'Starbucks Coffee':'data:image/jpeg;base64,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',
    'California Burrito':'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSir_uVXcPdxTYQ3l1jHwhNBpcx_CuAIAm9EQ&s',
    'Harley Fine Baking':'data:image/jpeg;base64,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',
    'Paradise': 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSm-geaNuBjUpcqJ5CDWkinM_SDfwILSgaNCQ&s',
    'BurgerKing': 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcR1OEvXMwFQMyyUUhxwFPkJFPHQwqy5I1HMdw&s',
    'KFC': 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSiRZCKcPZWMU6qYwx0iuuMOQmUYPJzXrgyQg&s',
    'Cream Stone':'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSmgFIAzXg6Ces2XBuO-emL1ow47P0Q297WrQ&s',
    'SwiggyRestaurant':'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRWzilWv8JjT_xeJiBIWvxlxE_MvJZUN5uPUw&s',
    'Pista House': 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQRrxuBlaH_JbRLrEMylteig0o6SfrdJYKmtQ&s',
    'default': 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQdbr1YkpECGY7WnAIbIiqM6bnjWHLB6PKLeQ&s'
  };
  currentPage: number = 1;
  itemsPerPage: number = 6; // Number of restaurants per page
  totalItems: number = 0;
  constructor(
    private restaurantService: RestaurantService,
    private router: Router,
    private toastr:ToastrService
  ) {}

  ngOnInit(): void {
      const token = localStorage.getItem('token');
      console.log('Token from localStorage:', token); // Check the token in the console
      this.getRestaurants(); // Call your method after logging the token
  }
  getRestaurants(): void {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    this.restaurantService.getRestaurants({ headers }).subscribe(
      (response: any) => {
        const restaurantsData = response?.$values || []; // Access the $values array
        this.totalItems = restaurantsData.length; // Calculate total items
        const startIndex = (this.currentPage - 1) * this.itemsPerPage;
        const endIndex = startIndex + this.itemsPerPage;
        this.restaurants = restaurantsData.slice(startIndex, endIndex).map((restaurant: any) => ({
          ...restaurant,
          imageUrl: this.getCuisineImage(restaurant.name)
        }));
      },
      error => {
        console.error('Error fetching restaurants:', error);
      }
    );
  }

  
  getCuisineImage(cuisine: string): string {
    return this.cuisineImageMap[cuisine] || this.cuisineImageMap['default'];
  }
  viewRestaurantDetails(restaurantId: number): void {
    console.log(restaurantId);
   // this.toastr.success(`Viewing Menu Items `);
    this.router.navigate(['/restaurant-menu', restaurantId]);
    console.log("routing");
  }
  changePage(page: number): void {
    this.currentPage = page;
    this.getRestaurants();
  }
}
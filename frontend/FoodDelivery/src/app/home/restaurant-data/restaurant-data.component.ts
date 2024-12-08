import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-restaurant-data',
  standalone: true,
  imports: [CommonModule,FormsModule,RouterLink],
  templateUrl: './restaurant-data.component.html',
  styleUrl: './restaurant-data.component.css'
})
export class RestaurantDataComponent {
  menuCategories = [
    { name: 'Lunch', image: 'assets/pictures/our-menu-01.jpg' },
    { name: 'Dinner', image: 'assets/pictures/our-menu-05.jpg' },
    { name: 'Drink', image: 'assets/pictures/our-menu-07.jpg' },
    { name: 'Starters', image: 'assets/pictures/our-menu-12.jpg' },
    { name: 'Happy Hour', image: 'assets/pictures/our-menu-16.jpg' },
    { name: 'Dessert', image: 'assets/pictures/our-menu-18.jpg' },
  ];
}

import { Component, OnInit } from '@angular/core';
import { RestaurantMenuComponent } from '../../restaurant/restaurant-menu/restaurant-menu.component';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-hero',
  standalone: true,
  imports: [RestaurantMenuComponent,RouterLink],
  templateUrl: './hero.component.html',
  styleUrl: './hero.component.css'
})
export class HeroComponent implements OnInit {
  constructor() { }
  currentIndex: number = 0;
  totalSlides: number = 4;
  
  cards: any;

  ngOnInit(): void {
    this.cards = document.querySelectorAll('.card');
    this.autoScroll();
  }

  autoScroll(): void {
    setInterval(() => {
      this.nextCard();
    }, 3000); // Scroll every 3 seconds
  }

  nextCard(): void {
    this.currentIndex = (this.currentIndex + 1) % this.cards.length;
    const offset = this.currentIndex * -100;
    document.querySelector('.carousel')!.scrollTo({
      left: offset,
      behavior: 'smooth',
    });
  }
}

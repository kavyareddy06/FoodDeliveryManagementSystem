import { Component, HostListener } from '@angular/core';
import { NavbarComponent } from './navbar/navbar.component';
import { HeroComponent } from './hero/hero.component';
import { RestaurantListComponent } from '../restaurant/restaurant-list/restaurant-list.component';
import { FooterComponent } from './footer/footer.component';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { RestaurantDataComponent } from './restaurant-data/restaurant-data.component';
import { AboutusComponent } from '../templates/aboutus/aboutus.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [NavbarComponent,HeroComponent,RestaurantListComponent,FooterComponent,CommonModule,RouterLink,RestaurantDataComponent,AboutusComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  username='';
  isMobileMenuOpen = false;
  isScrolled = false;
  constructor(private router:Router){}

  toggleMobileMenu() {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
  }
  goToCart(): void {
    this.router.navigate(['/cart']);  // Adjust the route if needed
  }

  profile() {
    console.log('Navigate to profile');
  }

  orders():void{
    this.router.navigate(['/orderstatus']);
  }

  logout() {
    console.log('User logged out');
   // this.username = null;
  }
  @HostListener('window:scroll', [])
  onWindowScroll() {
    const offset = window.scrollY;
    this.isScrolled = offset > 50;
    const navbar = document.querySelector('.navbar');
    if (this.isScrolled) {
      navbar?.classList.add('scrolled');
    } else {
      navbar?.classList.remove('scrolled');
    }
  }
}
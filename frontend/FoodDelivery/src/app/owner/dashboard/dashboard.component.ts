import { Component } from '@angular/core';
import { AddMenuItemComponent } from '../../menu-item/add-menu-item/add-menu-item.component';
import { RestaurantDashboardComponent } from '../restaurant-dashboard/restaurant-dashboard.component';
import { PromotionComponent } from '../promotion/promotion.component';
import { RouterLink, RouterOutlet } from '@angular/router';
import { DeliveryComponent } from '../delivery/delivery.component';
import { OrderReportComponent } from '../order-report/order-report.component';
import { NavComponent } from '../../templates/nav/nav.component';
import { InventoryComponent } from '../../inventory/inventory.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [AddMenuItemComponent,RestaurantDashboardComponent,PromotionComponent,RouterOutlet,RouterLink,DeliveryComponent,OrderReportComponent,NavComponent,InventoryComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {

}

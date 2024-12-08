import { Component } from '@angular/core';
import { AddMenuItemComponent } from '../../menu-item/add-menu-item/add-menu-item.component';
import { RestaurantDashboardComponent } from '../../owner/restaurant-dashboard/restaurant-dashboard.component';
import { PromotionComponent } from '../../owner/promotion/promotion.component';
import { RouterLink, RouterOutlet } from '@angular/router';
import { DeliveryComponent } from '../../owner/delivery/delivery.component';
import { OrderReportComponent } from '../../owner/order-report/order-report.component';
import { DeilveryDriversComponent } from '../deilvery-drivers/deilvery-drivers.component';
import { RestaurantDetailsComponent } from '../restaurant-details/restaurant-details.component';
import { CouponsComponent } from '../coupons/coupons.component';
import { AdminReportComponent } from '../admin-report/admin-report.component';
import { AdminInventoryComponent } from '../admin-inventory/admin-inventory.component';
import { NavComponent } from '../../templates/nav/nav.component';
import { AdminFeedbackComponent } from '../admin-feedback/admin-feedback.component';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [AddMenuItemComponent,RestaurantDetailsComponent,CouponsComponent,RouterOutlet,RouterLink,DeilveryDriversComponent,AdminReportComponent,AdminInventoryComponent,NavComponent,AdminFeedbackComponent],
  templateUrl: './admin-dashboard.component.html',
  styleUrl: './admin-dashboard.component.css'
})
export class AdminDashboardComponent {

}

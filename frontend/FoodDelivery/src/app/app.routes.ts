import { Routes } from '@angular/router';
import { provideHttpClient, HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './main/login/login.component';
import { RegisterComponent } from './main/register/register.component';

import { AuthGuard } from './guards/auth.guard';
import { HomeComponent } from './home/home.component';
import { RestaurantDashboardComponent } from './owner/restaurant-dashboard/restaurant-dashboard.component';
import { AddMenuItemComponent } from './menu-item/add-menu-item/add-menu-item.component';
import { RestaurantListComponent } from './restaurant/restaurant-list/restaurant-list.component';
import { RestaurantMenuComponent } from './restaurant/restaurant-menu/restaurant-menu.component';
import { CartComponent } from './restaurant/cart/cart.component';
import { PaymentComponent } from './restaurant/payment/payment.component';
import { OrderComponent } from './restaurant/orderflow/order/order.component';
import { SearchComponent } from './search/search.component';
import { InvoiceComponent } from './restaurant/orderflow/invoice/invoice.component';
import { OrderstatusComponent } from './restaurant/orderflow/orderstatus/orderstatus.component';
import { MatIconModule } from '@angular/material/icon'; // Import MatIconModule
import { MatProgressBarModule } from '@angular/material/progress-bar'; // Import MatProgressBarModule
import { MatTooltipModule } from '@angular/material/tooltip';
import { PromotionComponent } from './owner/promotion/promotion.component';
import { DashboardComponent } from './owner/dashboard/dashboard.component';
import { FeedbackComponent } from './restaurant/orderflow/feedback/feedback.component';
import { DeliveryComponent } from './owner/delivery/delivery.component';
import { DeliveryDriverComponent } from './delivery-driver/delivery-driver.component';
import { OrderReportComponent } from './owner/order-report/order-report.component';
import { AdminDashboardComponent } from './admin/admin-dashboard/admin-dashboard.component';
import { RestaurantDetailsComponent } from './admin/restaurant-details/restaurant-details.component';
import { CouponsComponent } from './admin/coupons/coupons.component';
import { DeilveryDriversComponent } from './admin/deilvery-drivers/deilvery-drivers.component';
import { AdminReportComponent } from './admin/admin-report/admin-report.component';
import { InventoryComponent } from './inventory/inventory.component';
import { AdminInventoryComponent } from './admin/admin-inventory/admin-inventory.component';
import { RestaurantDataComponent } from './home/restaurant-data/restaurant-data.component';
import { OneComponent } from './templates/one/one.component';
import { NavbarComponent } from './home/navbar/navbar.component';
import { AboutusComponent } from './templates/aboutus/aboutus.component';
import { NavComponent } from './templates/nav/nav.component';
import { AboutComponent } from './home/about/about.component';
import { AdminFeedbackComponent } from './admin/admin-feedback/admin-feedback.component';
export const routes: Routes = [
    {path:'',component:HomeComponent},
    {path:'navbar',component:NavbarComponent},
    {path:'aboutus',component:AboutusComponent},
    {path:'about',component:AboutComponent},
    {path:'login',component:LoginComponent},
    {path:'register',component:RegisterComponent},
    //{ path: 'admin', component: AdminComponent, canActivate: [AuthGuard] },
    {path:'home',component:HomeComponent},
    {path:'resturant-dashboard',component:RestaurantDashboardComponent},
    {
         path: 'add-menu-item/:restaurantId', component: AddMenuItemComponent 
    },
    {path:'app-restaurant-list',component:RestaurantListComponent},
    { path: 'restaurant-menu/:restaurantId', component: RestaurantMenuComponent },
    { path: 'cart', component: CartComponent },
    { path: 'payment', component: PaymentComponent },
    {path:'order',component:OrderComponent},
    {path:'search',component:SearchComponent},
    {path:'invoice',component:InvoiceComponent},
    {path:'orderstatus',component:OrderstatusComponent},
    {path:'promotion',component:PromotionComponent},
    {path:'dashboard',component:DashboardComponent, children: [
        { path: 'restaurant-dashboard', component: RestaurantDashboardComponent },
        { path: 'promotion', component: PromotionComponent },
       {path:'inventory',component:InventoryComponent},
        {path:'deilvery-person',component:DeliveryComponent},
        {path:'order-report',component:OrderReportComponent}
      ],

    },
    {path:'app-feedback',component:FeedbackComponent},
    {path:'delivery-driver',component:DeliveryDriverComponent},
    {path:'admin-dashboard',component:AdminDashboardComponent, children: [
        { path: 'restaurant-details', component: RestaurantDetailsComponent },
        { path: 'coupons', component: CouponsComponent },
        { path: 'deilvery-drivers', component: DeilveryDriversComponent },
        {path:'deilvery-person',component:DeliveryComponent},
        {path:'admin-report',component:AdminReportComponent},
        {path:'admin-inventories',component:AdminInventoryComponent},{
          path:'admin-feedback',component:AdminFeedbackComponent
        }
      ],

    },
    {path:'inventory',component:InventoryComponent},
    {
      path:'restaurant-data',component:RestaurantDataComponent
    },{
      path:'one',component:OneComponent
    },
    {
      path:'nav',component:NavComponent
    }
   
    
];

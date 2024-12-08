import { Component } from '@angular/core';
import { PromotionService } from '../../services/promotion.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RestaurantService } from '../../services/restaurant.service';
import { ToastrService } from 'ngx-toastr';
interface Promotion {
 promotionId?: number;
  restaurantId: number;
  code: string;
  discountType: string;
  discountAmount: number;
  startDate: string;
  endDate: string;
  status: string;
}
@Component({
  selector: 'app-promotion',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './promotion.component.html',
  styleUrl: './promotion.component.css'
})
export class PromotionComponent {
  promotions: Promotion[] = [];
  promotion= {
 //  promotionId: 0,
    restaurantId: 0, // Will be populated dynamically
    code: '',
    discountType: 'percentage',
    discountAmount: 0,
    startDate: '',
    endDate: '',
    status: 'active'
  };
  isEditing = false;
  private restaurantId: number =0;

  constructor(private promotionService: PromotionService,private restaurantService: RestaurantService,private toastr:ToastrService) {}

  ngOnInit() {
 this.loadRestaurantDetails();
   // this.loadPromotions();
  }
  formatDate(date: string): string {
    const d = new Date(date);
    return d.toISOString().split('T')[0]; // Extracts only the date part (yyyy-MM-dd)
  }
  loadRestaurantDetails() {
    this.restaurantService.getMyRestaurant().subscribe(
      (restaurant) => {
        this.restaurantId = restaurant.restaurantId;
        this.promotion.restaurantId = this.restaurantId;
        console.log('Restaurant ID set:', this.restaurantId);

        // Now load promotions after restaurantId is set
        this.loadPromotions();
      },
      (error) => {
        console.error('Error loading restaurant details:', error);
      }
    );
  }
  loadPromotions() {
    if (this.restaurantId === 0) {
      console.error('Restaurant ID is not set. Cannot load promotions.');
      return;
    }
  
    this.promotionService.getPromotionsByres(this.restaurantId).subscribe(
      (response: any) => {
        this.promotions = response.$values || []; // Extract promotions array
        console.log('Promotions loaded:', this.promotions);
  
        // Check and update the status for expired promotions
        const currentDate = new Date();
        this.promotions.forEach((promo) => {
          const endDate = new Date(promo.endDate);
          if (endDate < currentDate && promo.status !== 'expired') {
            promo.status = 'expired';
            this.promotionService.updatePromotion(promo).subscribe(() => {
              console.log(`Promotion ${promo.promotionId} marked as expired.`);
            });
          }
        });
      },
      (error) => {
        console.error('Error loading promotions:', error);
      }
    );
  }
  
  savePromotion() {
    this.promotion.startDate = this.formatDate(this.promotion.startDate);
    this.promotion.endDate = this.formatDate(this.promotion.endDate);
    if (this.isEditing) {
      this.promotionService.updatePromotion(this.promotion).subscribe(() => {
        this.toastr.info('Promotion updated successfully!');
        this.loadPromotions();
      });
    } else {
      this.promotionService.createPromotion(this.promotion).subscribe(() => {
        this.toastr.success('Promotion created successfully!');
        this.loadPromotions();
      },
      (err)=>{
        console.log(this.promotion);
      }
    );
    }
  }

  editPromotion(promotion: Promotion) {
    this.promotion = { ...promotion , startDate: this.formatDate(promotion.startDate),
      endDate: this.formatDate(promotion.endDate)};
    this.isEditing = true;
  }

  // Explicitly specify the type for the promotion parameter
  deletePromotion(promotion: Promotion) {
    this.promotionService.deletePromotion(promotion.promotionId!).subscribe(() => {
      alert('Promotion deleted!');
      this.loadPromotions();
    });
  }

  // applyDiscount(code: string, orderTotal: number) {
  //   this.promotionService.applyDiscount(code, orderTotal).subscribe(discountedTotal => {
  //     alert(`Discount applied! New total: ${discountedTotal}`);
  //   });
  // }
}

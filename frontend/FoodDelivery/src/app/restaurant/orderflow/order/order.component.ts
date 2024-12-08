import { Component } from '@angular/core';
import { CartService } from '../../../services/cart.service';
import { OrderService } from '../../../services/order.service';
import { Router } from '@angular/router';
import { PaymentComponent } from '../../payment/payment.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { InvoiceService } from '../../../services/invoice.service';
import { PromotionService } from '../../../services/promotion.service';
import { NavComponent } from '../../../templates/nav/nav.component';
import { HttpClient } from '@angular/common/http';
import { OrderstatusComponent } from '../orderstatus/orderstatus.component';

@Component({
  selector: 'app-order',
  standalone: true,
  imports: [PaymentComponent,CommonModule,FormsModule,NavComponent,OrderstatusComponent],
  templateUrl: './order.component.html',
  styleUrl: './order.component.css'
})
export class OrderComponent {
  currentStep = 1; // 1 for delivery address, 2 for payment
  cartItems: any[] = [];
  totalPrice: number = 0;
  cartItemCount: number = 0;
  userId: number | null = 0;
  deliveryFee = 74; // Example value
convenienceFee = 7; // Example value
gstCharges = 26.16; // Example value
deliveryDistance = 13.2;
discountCode: string = '';
  discountAmount: number = 0;
  discountApplied: boolean = false;
  discountError: string = '';
  totalAmount: number = 0;
 // totalAmount = 0;
  deliveryAddress = { flatNo: '', building: '', address: '', pincode: '' };
  cuisineImageMap: { [key: string]: string } = {
    'Main Course': '',
    'Drinks': 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRiapFuvE-Jj60bC8M_ZDzOQu6aT_kiLYzjVQ&s',
    'Snacks': 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTSIFl225qetjy1ZoKAEPJqDKNOCp-6I6eJAg&s',
    default: 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRBi95NVBBbDXJF7UFO1b3CTHnT7hlPgLMgdw&s',
  };
  loggedInUser: any = {
    username: '',
    phoneNumber: ''
  };
  discountedTotal: number=0;


  
  constructor(
    private cartService: CartService,
    private orderService: OrderService,
    private router: Router,
    private authService: AuthService,
    private toastr:ToastrService,
    private invoiceservice:InvoiceService,
    private promotionService:PromotionService,
    private http:HttpClient
  ) {}

  ngOnInit(): void {
    const storedId=localStorage.getItem('restaurantId');
    this.userId = Number(storedId) ;// Get userId from auth service
    if (this.userId) {
      this.loadCartItems(); // Load cart items when userId is available
    } else {
      console.error('User not authenticated');
    }
    this.loadUserDetails();
  }
  loadUserDetails(): void {
    this.authService.getUserDetails().subscribe(
      (userData) => {
        // Assuming the API returns the user's name and phone number
        this.loggedInUser = {
          username: userData.username,
          phoneNumber: userData.phoneNumber
        };
      },
      (error) => {
        console.error('Error loading user details:', error);
      }
    );
  }
  loadCartItems(): void {
    if (this.userId) {  // Ensure userId is valid
      this.cartService.getCartItems(this.userId).subscribe(
        (response: any) => {
          // Check if the response contains the `$values` property
          if (response && response.$values) {
            // Map through the $values array and extract necessary fields
            this.cartItems = response.$values.map((item: any) => ({
              cartItemId: item.cartItemId,
              quantity: item.quantity,
              price: item.price,
              menuItem: item.menuItem,
              imageUrl: this.getCuisineImage(item.menuItem.category),
            }));
            this.calculateTotalPrice();
            this.calculateCartItemCount();
          } else {
            console.error('Invalid response structure:', response);
          }
        },
        (error) => {
          console.error('Error loading cart items:', error);
        }
      );
    } else {
      console.error('Invalid userId:', this.userId);
    }
  }

  calculateCartItemCount(): void {
    this.cartItemCount = this.cartItems.reduce((count, cartItem) => {
      return count + cartItem.quantity; // Count all the items (sum of quantities)
    }, 0);
  }

  calculateTotalPrice(): void {
    this.totalPrice = this.cartItems.reduce((total, cartItem) => {
      return total + (cartItem.quantity * cartItem.price);
    }, 0);
   // this.totalPrice+= this.deliveryFee + this.convenienceFee + this.gstCharges;
  }
  increaseQuantity(item: any): void {
    item.quantity++;
    this.updateCartItem(item);
  }

  // Decrease the quantity of an item
  decreaseQuantity(item: any): void {
    if (item.quantity > 1) {
      item.quantity--;
      this.updateCartItem(item);
    }
  }
  updateCartItem(item: any): void {
    const userId = localStorage.getItem('restaurantId');
    const newQuantity = item.quantity;  // Get the updated quantity
    const cartItemId = item.cartItemId;  // Use cartItemId instead of itemId
  
    if (userId && cartItemId) {
      this.cartService.updateCartItem(userId, cartItemId, newQuantity).subscribe(() => {
        this.calculateTotalPrice(); // Recalculate total price after update
      });
    }
  }
  

  removeFromCart(item: any): void {
    const userId = localStorage.getItem('restaurantId');
    const cartItemId = item.cartItemId;  // Use cartItemId instead of itemId
  
    if (userId && cartItemId) {
      this.cartService.removeFromCart(userId, cartItemId).subscribe(() => {
        this.loadCartItems();  // Reload the cart after removal
      });
    }
  }
  goToPayment() {
    if (this.isDeliveryAddressValid()) {
      this.currentStep = 2; // Go to payment step
    } else {
      this.toastr.info('Please enter a valid delivery address!','Check details');
    }
  }

  isDeliveryAddressValid() {
    return (
      this.deliveryAddress.flatNo &&
      this.deliveryAddress.building &&
      this.deliveryAddress.address &&
      this.deliveryAddress.pincode
    );
  }

  onPaymentSuccess(paymentData: any) {
    const restaurantId = this.cartItems.length > 0 ? this.cartItems[0].menuItem.restaurantId : null;
  
    if (!restaurantId) {
      this.toastr.warning('Error', 'Restaurant ID is missing. Cannot place the order.');
      return;
    }
  
    const deliveryAddressString = JSON.stringify({
      flatNo: this.deliveryAddress.flatNo,
      building: this.deliveryAddress.building,
      address: this.deliveryAddress.address,
      pincode: this.deliveryAddress.pincode,
    });
  
    const orderRequest = {
      userId: this.userId,
      deliveryAddress: deliveryAddressString,
      paymentMethod: paymentData.paymentDetails.paymentMethod,
      cartItems: this.cartItems,
      totalPrice: this.totalPrice
    };
  
    this.orderService.placeOrder(restaurantId, orderRequest).subscribe(
      (response) => {
        this.toastr.success('Order placed successfully!');
       
        localStorage.removeItem('cartRestaurantId');
  
        // Trigger the invoice after successful order placement
        if (response.orderId) {
          this.sendInvoice(response.orderId);
        } else {
          this.toastr.warning('Order placed, but invoice cannot be sent as order ID is missing.');
        }
  
        this.router.navigate(['/orderstatus']);  // Redirect to confirmation page
      },
      (error) => {
        this.toastr.warning('Sorry low stock','Error placing the order');
      }
    );
  }
  sendInvoice(orderId: number) {
    const apiUrl = `https://localhost:7120/api/Invoice`;
  
    this.http.get(`${apiUrl}/send-invoice/${orderId}`, {}).subscribe({
      next: () => {
        this.toastr.success('Invoice sent successfully!');
      },
      error: (error: { error: { message: any; }; }) => {
        this.toastr.error(
          error.error?.message || 'Failed to send the invoice.'
        );
      }
    });
  }
    
  applyDiscount(): void {
    if (this.discountApplied) {
      this.toastr.error( 'Discount has already been applied.');
      return;
    }

    if (!this.discountCode) {
      this.discountError = 'Please enter a discount code.';
      return;
    }

    this.promotionService.applyDiscount(this.discountCode, this.totalPrice).subscribe(
      (discountedTotal) => {
        if (discountedTotal > 0) {
          this.discountedTotal = discountedTotal;
          //console.log(this.discountedTotal);
        //  console.log(this.totalPrice);
          this.discountApplied = true;
          this.discountError = '';
          this.toastr.success('Discount applied successfully');
          this.calculateTotalAmount();  // Update the total amount after discount
        } else {
          this.discountApplied = false;
          this.toastr.warning('Invalid or expired discount code','TimedOut Coupon');
        }
      },
      (error) => {
        //this.discountError = 'Error applying discount: ' + error.message;
        this.toastr.error('Invalid discount Coupon',this.discountError);
      }
    );
  }
  
  calculateTotalAmount(): void {
    // Use the discounted total as the final price
    this.totalPrice = this.discountedTotal; //+ this.deliveryFee + this.convenienceFee + this.gstCharges;
  }
  
  
  // sendInvoice(orderId: number) {
  //   console.log('Sending invoice for orderId:', orderId); // Log the orderId
  //   this.invoiceservice.sendInvoice(orderId).subscribe(
  //     (response) => {
  //       this.toastr.success('Invoice sent successfully.');
  //     },
  //     (error) => {
  //       console.error('Error sending invoice:', error);
  //       this.toastr.error('Error sending the invoice');
  //     }
  //   );
  // }
  

  // Define the function to get cuisine image based on the category
  getCuisineImage(category: string): string {
    return this.cuisineImageMap[category] || this.cuisineImageMap['default'];
  }
  // applyDiscount(): void {
  //   if (!this.discountCode) {
  //     this.discountError = 'Please enter a discount code.';
  //     return;
  //   }

  //   this.promotionService.applyDiscount(this.discountCode, this.totalPrice).subscribe(
  //     (discount) => {
  //       if (discount > 0) {
  //         this.discountAmount = discount;
  //         this.discountApplied = true;
  //         this.discountError = '';
  //         this.toastr.success('Discount applied successfully');
  //         this.calculateTotalAmount();
  //       } else {
  //         this.discountApplied = false;
  //         this.discountError = 'Invalid or expired discount code';
  //       }
  //     },
  //     (error) => {
  //       this.discountError = 'Error applying discount: ' + error.message;
  //       this.toastr.error(this.discountError);
  //     }
  //   );
  // }
  // calculateTotalAmount(): void {
  //   this.totalAmount = this.totalPrice - this.discountAmount + this.deliveryFee + this.convenienceFee + this.gstCharges;
  // }
}

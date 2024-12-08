import { Component, EventEmitter, Input, Output } from '@angular/core';
import { PaymentService } from '../../services/payment.service';
import { CartService } from '../../services/cart.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css']
})
export class PaymentComponent {

  @Input() deliveryAddress: any;  // Input to receive delivery address from parent
  @Output() paymentSuccess: EventEmitter<any> = new EventEmitter();  // Output to notify parent on payment success

  paymentMethod: string = 'COD';  // Default to COD
  paymentDetails: any = {
    paymentMethod: "COD",
    paymentStatus: "Pending",
    cardNumber: "",
    expiryDate: "",
    cvv: ""
  };

  totalPrice: number = 0;

  constructor(
    private paymentService: PaymentService,
    private cartService: CartService,
    private toastr:ToastrService
  ) {}

  ngOnInit(): void {
    const userId = localStorage.getItem('restaurantId');
    if (userId) {
      this.loadCartTotalPrice(+userId);  // Pass userId to fetch total price
    } else {
      console.error('User ID is missing in localStorage');
    }
  }

  loadCartTotalPrice(userId: number): void {
    this.cartService.getCartItems(userId).subscribe(
      (response: any) => {
        const cartItems = response?.$values ?? [];
        if (Array.isArray(cartItems) && cartItems.length > 0) {
          this.totalPrice = cartItems.reduce((total, item) => total + (item.price * item.quantity), 0);
        } else {
          console.error('Cart items are empty or not in the expected format:', cartItems);
        }
      },
      error => {
        console.error('Error fetching cart items:', error);
      }
    );
  }

  onPaymentMethodChange(event: any): void {
    this.paymentMethod = event.target.value;
    this.paymentDetails.paymentMethod = this.paymentMethod;
  }

  confirmOrder(): void {
    // Logic for confirming the order, potentially saving to backend or updating order status
    alert("Order Confirmed");
  }

  onSubmit(): void {
    // Basic validation for card fields if CreditCard is selected
    if (this.paymentMethod === 'Credit Card' && !this.isCreditCardDataValid()) {
      this.toastr.info('Please fill in all Credit Card details correctly.');
      return;
    }

    if (this.paymentMethod === 'COD') {
      this.paymentSuccess.emit({
        paymentDetails: this.paymentDetails,
        deliveryAddress: this.deliveryAddress
      });
      this.toastr.info("Payment confirmed via Cash on Delivery","Wohhoo!!");
    } else {
      // Log payment data to verify its correctness
      console.log('Payment Data:', this.paymentDetails);

      this.paymentService.processPayment(this.paymentDetails).subscribe(
        (response) => {
          console.log('Payment successful:', response);
          // Emit payment success along with address data for order placement
          this.paymentSuccess.emit({
            paymentDetails: this.paymentDetails,
            deliveryAddress: this.deliveryAddress
          });
        },
        (error) => {
          console.error('Payment failed:', error);
          this.toastr.warning('Payment failed. Please try again.');
        }
      );
    }
  }

  isCreditCardDataValid(): boolean {
    const { cardNumber, expiryDate, cvv } = this.paymentDetails;
    return cardNumber && expiryDate && cvv && cardNumber.length === 12 && expiryDate.length === 5 && cvv.length === 3;
  }
}


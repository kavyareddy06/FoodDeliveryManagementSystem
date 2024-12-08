import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../../services/order.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { FeedbackComponent } from '../feedback/feedback.component';
import { ToastrComponentlessModule, ToastrService } from 'ngx-toastr';
import { HttpClient } from '@angular/common/http';
import { NavComponent } from '../../../templates/nav/nav.component';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from '../../../home/navbar/navbar.component';
@Component({
  selector: 'app-orderstatus',
  standalone: true,
  imports: [CommonModule,FormsModule, MatIconModule,MatTooltipModule,MatProgressBarModule,FeedbackComponent,NavComponent,RouterOutlet ],
  templateUrl: './orderstatus.component.html',
  styleUrl: './orderstatus.component.css'
})
export class OrderstatusComponent implements OnInit {
  orders: any[] = [];
  selectedOrderId: number | null = null; // Track selected order for feedback
  isFeedbackModalOpen: boolean = false; // Track feedback modal visibility
  isDriverDetailsModalOpen: boolean = false; // Track driver details modal visibility
  driverDetails: { [key: string]: any } = {}; // Change this line
 // Store driver details
  trackingDetails: any = null;
  imageMapping: { [key: string]: string } = {
    "Pizza": "assets/images/pizza.jpg",
    "Burger": "assets/images/burger.jpg",
    "Pasta": "assets/images/pasta.jpg",
    default:'',
    // Add more mappings as needed
  };
  constructor(private orderService: OrderService, private toastr: ToastrService,private http:HttpClient) {}

  ngOnInit() {
    const userId = parseInt(localStorage.getItem('restaurantId') || '0', 10);
    this.orderService.getUserOrders(userId).subscribe(
      (data: any) => {
        this.orders = data?.$values.map((order: { status: string }) => {
          order.status = order.status.toLowerCase(); // Normalize status to lowercase
          return order;
        }) || [];
      },
      (error) => {
        alert('Failed to fetch orders. Please try again later.');
      }
    );
  }
  checkDriverStatus(orderId: number) {
    const url = `https://localhost:7120/api/Delivery/order/${orderId}`;
    
    this.orderService.getDriverDetails(url).subscribe(
      (data: any) => {
        if (data?.message === "Delivery information not found for this order.") {
          this.toastr.warning('No delivery driver assigned for this order.');
          this.driverDetails = {}; // Reset driver details
          this.trackingDetails = null; // Reset tracking details
          this.isDriverDetailsModalOpen = false; // Close modal
        } else {
          const driverUserId = data?.userId;
          this.getUserDetails(driverUserId); // Fetch driver details using the userId
          this.isDriverDetailsModalOpen = true; // Open modal when driver details are available
        }
      },
      (error) => {
        this.toastr.warning('Failed to fetch delivery details.');
      }
    );
  }
  
  
  closeDriverDetailsModal() {
    this.isDriverDetailsModalOpen = false;
   this.driverDetails = {};
    this.trackingDetails = null;
  }

  // Get progress value based on the status of the order
  getProgressValue(status: string): number {
    switch (status.toLowerCase()) {
      case 'pending':
        return 20;  // 20% for pending
      case 'in preparation':
        return 50;  // 50% for in preparation
      case 'out for delivery':
        return 80;  // 80% for out for delivery
      case 'delivered':
        return 100;  // 100% for delivered
      default:
        return 0;  // Default to 0% if status is unknown
    }
  }

  // Track order status
  trackOrder(orderId: number) {
    this.orderService.trackOrder(orderId).subscribe(
      (data: any) => {
        this.toastr.info(`Order Status: ${data.status}`);
      },
      (error) => {
        this.toastr.error('Failed to track the order.');
      }
    );
  }

  // Cancel the order
  cancelOrder(orderId: number) {
    this.orderService.cancelOrder(orderId).subscribe(
      () => {
        alert('Order cancelled successfully');
        this.ngOnInit(); // Refresh the orders list
      },
      (error) => {
        alert('Failed to cancel the order.');
      }
    );
  }

  // Open the feedback modal for the selected order
  openFeedbackModal(orderId: number) {
    this.selectedOrderId = orderId; // Set the selected order ID
    this.isFeedbackModalOpen = true;
  }

  // Close the feedback modal
  closeFeedbackModal() {
    this.isFeedbackModalOpen = false;
    this.selectedOrderId = null; // Reset selected order ID
  }

  // Submit the feedback
  giveFeedback(feedbackData: { comments: string; rating: number }) {
    const feedback = {
      orderId: this.selectedOrderId, // Use the selected order ID
      userId: parseInt(localStorage.getItem('restaurantId') || '0', 10),
      rating: feedbackData.rating,
      comments: feedbackData.comments,
      feedbackDate: new Date(),
    };

    this.orderService.submitFeedback(feedback).subscribe(
      () => {
        this.toastr.success('Thank you for your feedback!');
        this.closeFeedbackModal(); // Close the modal after submitting
      },
      (error) => {
        this.toastr.warning('Failed to submit feedback. Please try again.');
      }
    );
  }
  getUserDetails(userId: string): void {
    const url = `https://localhost:7120/api/Auth/${userId}`;
    this.http.get(url).subscribe(
      (data) => {
        this.driverDetails = data; // Store driver details
      },
      (error) => {
        console.error('Failed to fetch user details', error);
      }
    );
  }
}
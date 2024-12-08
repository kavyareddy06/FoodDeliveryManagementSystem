import { Component, OnInit } from '@angular/core';
import { DeliveryService } from '../services/delivery.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../services/auth.service';
import { HttpClient } from '@angular/common/http';
import { NavComponent } from '../templates/nav/nav.component';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-delivery-driver',
  standalone: true,
  imports: [CommonModule, FormsModule,NavComponent],
  templateUrl: './delivery-driver.component.html',
  styleUrls: ['./delivery-driver.component.css']
})
export class DeliveryDriverComponent implements OnInit {
  deliveries: any[] = [];
  driver: any = {}; // Assuming driver data is fetched from the API
  selectedDeliveryId: number | null = null;
  statusUpdate: string = 'Picked Up';
  locationUpdate: string = '';
  customerDetails: { [userId: string]: any } = {};
  constructor(private deliveryService: DeliveryService,private authService:AuthService,private http:HttpClient,private toastr:ToastrService) {}

  ngOnInit(): void {
    this.getDriverDetails();
    this.getDeliveries();
  }

  // Get all deliveries
  getDeliveries(): void {
    this.deliveryService.getAllDeliveries().subscribe(
      (data) => {
        this.deliveries = data;
      },
      (error) => {
        console.error('Error fetching deliveries', error);
      }
    );
  }

  // Get driver details (mock data or API)
  getDriverDetails(): void {
    this.authService.getUserDetails().subscribe(
      (data) => {
        this.driver = data; // Populate the driver object with the fetched data
      },
      (error) => {
        console.error('Error fetching driver details', error);
      }
    );
  }

  // Update delivery status
  updateDeliveryStatus(): void {
    if (this.selectedDeliveryId != null) {
      this.deliveryService.updateDeliveryStatus(this.selectedDeliveryId, this.statusUpdate, this.locationUpdate).subscribe(
        () => {
          this.getDeliveries(); // Refresh the delivery list
          this.toastr.success('Updated successfully');
          this.statusUpdate = 'Picked Up';
          this.locationUpdate = '';
        },
        (error) => {
          console.error('Error updating delivery status', error);
        }
      );
    }
  }

  selectDelivery(delivery: any): void {
    this.selectedDeliveryId = delivery.deliveryId;
    this.statusUpdate = delivery.deliveryStatus;
    this.locationUpdate = delivery.deliveryAddress;
  }
  getUserDetails(userId: string): void {
    // Check if the user details are already cached
    if (!this.customerDetails[userId]) {
      const url = `https://localhost:7120/api/Auth/${userId}`;
      this.http.get(url).subscribe(
        (data) => {
          this.customerDetails[userId] = data; // Cache the response
        },
        (error) => {
          console.error('Failed to fetch user details', error);
        }
      );
    }
  }
}

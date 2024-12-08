import { Component, OnInit } from '@angular/core';
import { DeliveryService } from '../../services/delivery.service';
import { AuthService } from '../../services/auth.service';  // Import AuthService
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-delivery',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './delivery.component.html',
  styleUrls: ['./delivery.component.css']
})
export class DeliveryComponent implements OnInit {
  deliveries: any[] = [];
  deliveryDrivers: any[] = [];
  newDelivery: any = {
    OrderId: '',
    UserId: '',  // Driver ID
    PickupTime: '',
    DeliveryStatus: 'Picked Up',
    DeliveryAddress: ''
  };

  constructor(
    private deliveryService: DeliveryService, 
    private authService: AuthService,  // Inject AuthService
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getDeliveryDrivers();
    this.getDeliveries();
  }

  getDeliveries(): void {
    this.deliveryService.getAllDeliveries().subscribe(
      (data) => {
        this.deliveries = data;  // Now 'data' should be an array
        this.populateDriverDetails(); // Fetch driver details after deliveries are loaded
      },
      (error) => {
        console.error('Error fetching deliveries', error);
      }
    );
  }

  getDeliveryDrivers(): void {
    this.deliveryService.getDeliveryDrivers().subscribe(
      (data) => {
        this.deliveryDrivers = data;  // Populate dropdown list of drivers
      },
      (error) => {
        console.error('Error fetching delivery drivers', error);
      }
    );
  }

  // Fetch driver details for each delivery based on userId
  populateDriverDetails(): void {
    this.deliveries.forEach((delivery) => {
      if (delivery.userId) {
        this.authService.getUserDetailsById(delivery.userId).subscribe(
          (userDetails) => {
            // Assign the fetched driver details to the delivery object
            delivery.user = userDetails;
          },
          (error) => {
            console.error('Error fetching user details', error);
          }
        );
      }
    });
  }

  // Add new delivery
  addDelivery(): void {
    this.deliveryService.addDelivery(this.newDelivery).subscribe(
      () => {
        this.getDeliveries(); // Refresh the delivery list
        this.resetForm();
      },
      (error) => {
        console.error('Error adding delivery', error);
      }
    );
  }

  // Reset form after adding a delivery
  resetForm(): void {
    this.newDelivery = {
      OrderId: '',
      UserId: '',
      PickupTime: '',
      DeliveryStatus: 'Picked Up',
      DeliveryAddress: ''
    };
  }

  // Delete delivery
  deleteDelivery(deliveryId: number): void {
    this.deliveryService.deleteDelivery(deliveryId).subscribe(
      () => {
        this.getDeliveries(); // Refresh the delivery list
      },
      (error) => {
        console.error('Error deleting delivery', error);
      }
    );
  }
}

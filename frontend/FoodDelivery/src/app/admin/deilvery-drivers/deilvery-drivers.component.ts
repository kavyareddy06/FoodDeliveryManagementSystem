import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DeliveryService } from '../../services/delivery.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-deilvery-drivers',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './deilvery-drivers.component.html',
  styleUrl: './deilvery-drivers.component.css'
})
export class DeilveryDriversComponent {
  deliveries: any[] = [];
  deliveryDrivers: any[] = [];
  newDelivery: any = {
    OrderId: '',
    UserId: '', // Driver ID, could be dynamically selected
    PickupTime: '',
    DeliveryStatus: 'Picked Up',
    DeliveryAddress: ''
  };
  constructor(private deliveryService: DeliveryService, private router: Router) {}

  ngOnInit(): void {
    this.getDeliveryDrivers();
    this.getDeliveries();
  }
  // delivery.component.ts
getDeliveries(): void {
  this.deliveryService.getAllDeliveries().subscribe(
    (data) => {
      this.deliveries = data;
      console.log(this.deliveries);  // Now 'data' should be an array
    },
    (error) => {
      console.error('Error fetching deliveries', error);
    }
  );
}
getDeliveryDrivers(): void {
  this.deliveryService.getDeliveryDrivers().subscribe(
    (data) => {
      this.deliveryDrivers = data;
      console.log(this.deliveryDrivers); // Populate dropdown list
    },
    (error) => {
      console.error('Error fetching delivery drivers', error);
    }
  );
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
}

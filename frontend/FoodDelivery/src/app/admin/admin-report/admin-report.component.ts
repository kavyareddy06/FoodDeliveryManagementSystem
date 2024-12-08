import { Component } from '@angular/core';
import { OrderReportService } from '../../services/order-report.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-report',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-report.component.html',
  styleUrl: './admin-report.component.css'
})
export class AdminReportComponent {
  restaurantId: number = 1;
  startDate: string = '2024-12-01';
  endDate: string = '2024-12-05';
  frequency: string = 'daily';  // Default frequency
  totalAmount: number = 0;
  ordersByDate: any[] = [];
  ordersByCategory: any[] = [];
  showForm: boolean = false; // Controls visibility of the Fetch Data form
  showDateData: boolean = false; // Controls visibility for Orders by Date
  showCategoryData: boolean = false; // Controls visibility for Orders by Category

  constructor(private orderReportService: OrderReportService) {}

  ngOnInit(): void {
    this.restaurantId = 1;  // Assume the logged-in user's restaurant ID is 1
    this.fetchReports();
  }

  fetchReports(): void {
    console.log('Fetching reports for restaurantId:', this.restaurantId);
    console.log('Start Date:', this.startDate);
    console.log('End Date:', this.endDate);

    if (!this.restaurantId || !this.startDate || !this.endDate) {
      console.error('Invalid input values!');
      return;
    }

    // Fetch orders grouped by date
    this.orderReportService.getOrdersGroupedByDate(this.restaurantId, this.startDate, this.endDate, this.frequency).subscribe(
      (data) => {
        if (data && data.$values) {
          const groupedByDate = data.$values.reduce((acc: any[], group: any) => {
            const dateKey = new Date(group.$values[0].orderDate).toLocaleDateString();
            const totalAmount = group.$values.reduce((sum: number, order: any) => sum + order.totalAmount, 0);
            const totalOrders = group.$values.length;

            acc.push({
              date: dateKey,
              totalAmount: totalAmount,
              totalOrders: totalOrders,
              orders: group.$values
            });
            return acc;
          }, []);
          
          this.ordersByDate = groupedByDate;
          this.totalAmount = this.ordersByDate.reduce((sum, item) => sum + item.totalAmount, 0);
        } else {
          console.error('Invalid data structure:', data);
        }
      },
      (error) => {
        console.error('Error fetching orders by date:', error);
      }
    );

    // Fetch orders grouped by category
    this.orderReportService.getOrdersGroupedByCategory(this.restaurantId, this.startDate, this.endDate).subscribe(
      (data) => {
        if (data && data.$values) {
          const ordersByCategory = data.$values.map((categoryGroup: any) => {
            const totalAmount = categoryGroup.$values.reduce((sum: number, orderItem: any) => sum + orderItem.price * orderItem.quantity, 0);
            const totalOrders = categoryGroup.$values.length;

            return {
              category: categoryGroup.$values[0].menuItem ? categoryGroup.$values[0].menuItem.category : 'Unknown',
              totalAmount: totalAmount,
              totalOrders: totalOrders
            };
          });

          this.ordersByCategory = ordersByCategory;
        } else {
          console.error('Invalid data structure for categories:', data);
        }
      },
      (error) => {
        console.error('Error fetching orders by category:', error);
      }
    );
  }

  showGroupBy(type: string): void {
    if (type === 'date') {
      this.showDateData = true;
      this.showCategoryData = false;
    } else if (type === 'category') {
      this.showCategoryData = true;
      this.showDateData = false;
    }
  }
}

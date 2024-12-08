import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-invoice',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './invoice.component.html',
  styleUrl: './invoice.component.css',
})
export class InvoiceComponent {
  orderId: number | undefined; // Order ID to trigger the invoice email
  responseData: any | null = null; // To display the response data
  errorMessage: string = ''; // To display errors if any

  constructor(private http: HttpClient) {}

  sendInvoice() {
    if (!this.orderId) {
      this.errorMessage = 'Please provide a valid Order ID.';
      this.responseData = null;
      return;
    }

    const apiUrl = `https://localhost:7120/api/Invoice`;

    this.http.post(`${apiUrl}/send-invoice/${this.orderId}`, {}).subscribe({
      next: (response: any) => {
        // Store the response data
        this.responseData = response;
        this.errorMessage = ''; // Clear any previous error message
      },
      error: (error) => {
        // Handle errors gracefully
        this.errorMessage =
          error.error?.message || 'An error occurred while sending the invoice.';
        this.responseData = null; // Clear the response data
      },
    });
  }
}

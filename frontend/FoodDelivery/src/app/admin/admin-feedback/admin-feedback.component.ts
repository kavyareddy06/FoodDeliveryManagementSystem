import { CommonModule } from '@angular/common';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-feedback',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-feedback.component.html',
  styleUrls: ['./admin-feedback.component.css']
})
export class AdminFeedbackComponent {
  feedbackList: any[] = []; // Holds the list of feedback
  currentPage: number = 1;  // Track the current page
  pageSize: number = 5;  // Items per page
  totalPages: number = 1;  // Total pages, calculated later
  private apiUrl = 'https://localhost:7120/api/Feedback'; // Replace with your API endpoint
  private authToken = localStorage.getItem('token');
  
  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.getAllFeedback();
  }

  getAllFeedback() {
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.authToken}`,
    });

    this.http.get<any>(this.apiUrl, { headers }).subscribe(
      (response) => {
        this.feedbackList = this.processFeedbackResponse(response);
        this.totalPages = Math.ceil(this.feedbackList.length / this.pageSize); // Calculate total pages
      },
      (error) => {
        console.error('Error fetching feedback:', error);
      }
    );
  }

  // Process the response to handle nested objects and references
  private processFeedbackResponse(response: any): any[] {
    return response.$values.map((feedback: any) => ({
      userId: feedback.userId,
      orderId: feedback.orderId,
      rating: feedback.rating,
      comments: feedback.comments,
      feedbackDate: feedback.feedbackDate
    }));
  }

  // Get the current page's feedback
  getPagedFeedback() {
    const startIndex = (this.currentPage - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    return this.feedbackList.slice(startIndex, endIndex);
  }

  // Change the page
  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
    }
  }
}

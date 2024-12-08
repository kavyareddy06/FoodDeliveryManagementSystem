import { Component } from '@angular/core';
import {  EventEmitter, Input, Output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
//import { FeedbackComponent } from '../feedback/feedback.component';
@Component({
  selector: 'app-feedback',
  standalone: true,
  imports: [MatIconModule,MatTooltipModule,MatProgressBarModule,CommonModule,FormsModule],
  templateUrl: './feedback.component.html',
  styleUrl: './feedback.component.css'
})
export class FeedbackComponent {

    @Input() orderId: number | null = null;
    @Output() submitFeedback = new EventEmitter<{ comments: string, rating: number }>();
    @Output() close = new EventEmitter<void>();
  
    feedbackText: string = '';
    rating: number = 0;
  
    // Additional rating factors
    foodQualityRating: number = 0;
    deliveryTimeRating: number = 0;
    driverProfessionalismRating: number = 0;
  
    // Methods for individual ratings
    setFoodQualityRating(star: number) {
      this.foodQualityRating = star;
    }
  
    setDeliveryTimeRating(star: number) {
      this.deliveryTimeRating = star;
    }
  
    setDriverProfessionalismRating(star: number) {
      this.driverProfessionalismRating = star;
    }
  
    setRating(star: number) {
      this.rating = star;
    }
  
    submit() {
      if (this.rating > 0 && this.feedbackText.trim()) {
        this.submitFeedback.emit({
          comments: this.feedbackText,
          rating: this.rating
        });
      } else {
        alert("Please provide both a rating and feedback.");
      }
    }
  
    closeModal() {
      this.close.emit();
    }
  }
  
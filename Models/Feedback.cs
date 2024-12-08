namespace FoodDeliveryManagementSystem.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; } // Primary Key
        public int OrderId { get; set; }    // Foreign Key: Refers to the Order
        public int UserId { get; set; }     // Foreign Key: Refers to the Customer's UserId
        public int Rating { get; set; }     // Rating (e.g., 1-5 stars)
        public string Comments { get; set; } // Optional comments from the customer
        public DateTime FeedbackDate { get; set; } // Date when feedback was submitted

        // Navigation Properties
        public Order? Order { get; set; }    // Navigation to the associated Order
        public User? User { get; set; }      // Navigation to the User who gave feedback
    }

}

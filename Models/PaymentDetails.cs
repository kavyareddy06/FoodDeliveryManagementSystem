namespace FoodDeliveryManagementSystem.Models
{
    public class PaymentDetails
    {
        public string PaymentMethod { get; set; }  // COD or Credit Card
        public string PaymentStatus { get; set; }  // Pending, Completed, etc.
        public string CardNumber { get; set; }     // Only for Credit Card payments
        public string ExpiryDate { get; set; }     // Only for Credit Card payments
        public string CVV { get; set; }
    }
}

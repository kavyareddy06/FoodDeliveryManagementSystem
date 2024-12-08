namespace FoodDeliveryManagementSystem.Models
{
    public class Delivery
    {
        public int DeliveryId { get; set; }
        public int OrderId { get; set; } // Refers to the associated Order
        public int UserId { get; set; } // Refers to the Driver's UserId
        public DateTime PickupTime { get; set; }
        public DateTime? DeliveryTime { get; set; }
        public string DeliveryStatus { get; set; }
        public string DeliveryAddress { get; set; }

        // Navigation Properties
        public User? User { get; set; } // The driver handling the delivery
        public Order? Order { get; set; } // The related order
    }

}

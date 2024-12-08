namespace FoodDeliveryManagementSystem.Models
{



    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; } // Refers to the Customer's UserId
        public int RestaurantId { get; set; } // The restaurant fulfilling the order
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public string? DeliveryAddress { get; set; }

        // Navigation Properties
        public User? User { get; set; } // The customer who placed the order
        public Restaurant? Restaurant { get; set; } // The restaurant handling the order
        public ICollection<OrderItem>? OrderItems { get; set; } 
    }

}

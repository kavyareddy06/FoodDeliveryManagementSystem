namespace FoodDeliveryManagementSystem.Models
{
    public class OrderTracking
    {
        public int TrackingId { get; set; }
        public int OrderId { get; set; }
        public string Status { get; set; }
        public DateTime Timestamp { get; set; }
        public string Location { get; set; }

        public Order Order { get; set; }
    }

}

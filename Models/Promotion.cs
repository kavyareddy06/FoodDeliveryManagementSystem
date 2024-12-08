namespace FoodDeliveryManagementSystem.Models
{
    public class Promotion
    {
        public int PromotionId { get; set; }
        public int RestaurantId { get; set; }
        public string Code { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        public Restaurant? Restaurant { get; set; }
    }

}

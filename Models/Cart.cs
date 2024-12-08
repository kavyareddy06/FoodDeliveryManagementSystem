using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryManagementSystem.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int RestaurantId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; } // Nullable

        public User User { get; set; }
        public Restaurant Restaurant { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}

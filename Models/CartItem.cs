using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryManagementSystem.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        [Required]
        public int CartId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public Cart Cart { get; set; } // Navigation property

        public MenuItem MenuItem { get; set; } // N
    }
}

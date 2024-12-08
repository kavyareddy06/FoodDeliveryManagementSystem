using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FoodDeliveryManagementSystem.Models
{
    public class MenuItem
    {
        public int ItemId { get; set; }
        [Required]
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public bool Availability { get; set; }
       // [ForeignKey("RestaurantId")]
       public Restaurant? Restaurant { get; set; }
    }

}

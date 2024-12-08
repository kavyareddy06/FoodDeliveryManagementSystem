using System.Text.Json.Serialization;

namespace FoodDeliveryManagementSystem.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        [JsonIgnore]
        public ICollection<Restaurant> RestaurantsOwned { get; set; }
    }

}

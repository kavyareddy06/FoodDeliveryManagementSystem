namespace FoodDeliveryManagementSystem.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string CuisineType { get; set; }
        public string Phone { get; set; }
        public string HoursOfOperation { get; set; }
        public string Status { get; set; }
        public int OwnerId { get; set; }
        public User? Owner { get; set; }
        public ICollection<MenuItem>? MenuItems { get; set; }
    }

}

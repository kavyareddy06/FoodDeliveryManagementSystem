namespace FoodDeliveryManagementSystem.Models
{
    public class Inventory
    {
        public int InventoryId { get; set; }
        public int RestaurantId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int ReorderLevel { get; set; }
        public string Status { get; set; }

        public Restaurant? Restaurant { get; set; }
        public MenuItem? MenuItem { get; set; }
    }

}

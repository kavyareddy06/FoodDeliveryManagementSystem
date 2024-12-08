using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Services
{
    public interface IInventoryService
    {
     
        public Task TrackInventoryAsync();
       public List<Inventory> GetLowStockItemsBasedOnOrders(int restaurantId);
        public Task UpdateInventoryQuantityAsync(int restaurantId, int itemId, int newQuantity);
        public Task<Inventory> AddInventoryAsync(Inventory inventory);
        public Task<Inventory?> GetByIdAsync(int inventoryId);
        public Task<IEnumerable<Inventory>> GetInventoryByRestaurantIdAsync(int restaurantId);
        public Task<List<Inventory>> GetAllInventories();
        public Task DecreaseInventoryAsync(int itemId, int quantity);


    }
}

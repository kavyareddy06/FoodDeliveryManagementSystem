using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Repositories
{
    public interface IInventoryRepository
    {
      public  Task<IEnumerable<Inventory>> GetAllAsync();
        public  Task<IEnumerable<Inventory>> GetAll();
        public Task<Inventory?> GetByItemIdAndRestaurantIdAsync(int itemId, int restaurantId);
        public Task<IEnumerable<Inventory>> GetByRestaurantIdAsync(int restaurantId);
        public Task<Inventory?> GetByIdAsync(int inventoryId);
        public Task AddAsync(Inventory inventory);
        public Task UpdateAsync(Inventory inventory);
        public Task SaveChangesAsync();
        public List<Inventory> GetLowStockItemsBasedOnOrders(int restaurantId);
        public Task<Inventory> GetByItemId(int itemId);
    }
}

using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Repositories
{
    public interface IMenuItemRepository
    {
       public Task<IEnumerable<MenuItem>> GetMenuItemsByRestaurantAsync(int restaurantId);
        public Task<MenuItem> GetMenuItemAsync(int id);
        public Task AddMenuItemAsync(MenuItem menuItem);
        public Task UpdateMenuItemAsync(MenuItem menuItem);
        public Task DeleteMenuItemAsync(int id);
        public Task<MenuItem> GetByIdAsync(int itemId);
    }
}

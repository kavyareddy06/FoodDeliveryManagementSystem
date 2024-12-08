using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Services
{
    public interface IMenuItemService
    {
       public Task<IEnumerable<MenuItem>> GetMenuItemsByRestaurantAsync(int restaurantId);
        public Task CreateMenuItemAsync(MenuItem menuItem);
        public Task<bool> UpdateMenuItemAsync(MenuItem updatedMenuItem);
        public Task DeleteMenuItemAsync(int id);
    }
}

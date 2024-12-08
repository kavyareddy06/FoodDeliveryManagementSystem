using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Repositories;

namespace FoodDeliveryManagementSystem.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepo;

        public MenuItemService(IMenuItemRepository menuItemRepo)
        {
            _menuItemRepo = menuItemRepo;
        }

        public async Task<IEnumerable<MenuItem>> GetMenuItemsByRestaurantAsync(int restaurantId)
        {
            return await _menuItemRepo.GetMenuItemsByRestaurantAsync(restaurantId);
        }

        public async Task CreateMenuItemAsync(MenuItem menuItem)
        {
            await _menuItemRepo.AddMenuItemAsync(menuItem);
        }

        public async Task<bool> UpdateMenuItemAsync(MenuItem updatedMenuItem)
        {
            // Fetch the MenuItem from the database by its ID
            var menuItem = await _menuItemRepo.GetMenuItemAsync(updatedMenuItem.ItemId);

            // If the MenuItem doesn't exist, return false
            if (menuItem == null)
                return false;

            // Update the properties of the existing MenuItem with the new values
            menuItem.Name = updatedMenuItem.Name;
            menuItem.Category = updatedMenuItem.Category;
            menuItem.Price = updatedMenuItem.Price;
            menuItem.Description = updatedMenuItem.Description;
            menuItem.Availability = updatedMenuItem.Availability;

            // Save the updated MenuItem to the database
            await _menuItemRepo.UpdateMenuItemAsync(menuItem);
            return true;
        }


        public async Task DeleteMenuItemAsync(int id)
        {
            await _menuItemRepo.DeleteMenuItemAsync(id);
        }
    }
}

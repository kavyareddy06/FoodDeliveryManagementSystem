using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FoodDeliveryManagementSystem.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly FoodDeliveryContext _context;

        public MenuItemRepository(FoodDeliveryContext context)
        {
            _context = context;
        }
        public async Task<MenuItem> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Menu item ID must be greater than 0.");
            }

            var menuItem = await _context.MenuItems
                .FirstOrDefaultAsync(m => m.ItemId == id);

            if (menuItem == null)
            {
                throw new KeyNotFoundException($"Menu item with ID {id} not found.");
            }

            return menuItem;
        }
        public async Task<IEnumerable<MenuItem>> GetMenuItemsByRestaurantAsync(int restaurantId)
        {
            return await _context.MenuItems.Where(m => m.RestaurantId == restaurantId).ToListAsync();
        }

        public async Task<MenuItem> GetMenuItemAsync(int id)
        {
            return await _context.MenuItems.FindAsync(id);
        }

        public async Task AddMenuItemAsync(MenuItem menuItem)
        {
            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMenuItemAsync(MenuItem menuItem)
        {
            _context.MenuItems.Update(menuItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMenuItemAsync(int id)
        {
            var menuItem = await GetMenuItemAsync(id);
            if (menuItem != null)
            {
                _context.MenuItems.Remove(menuItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}

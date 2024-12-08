using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace FoodDeliveryManagementSystem.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly FoodDeliveryContext _context;

        public RestaurantRepository(FoodDeliveryContext context)
        {
            _context = context;
        }

        public async Task<Restaurant> GetRestaurantAsync(int id)
        {
            return await _context.Restaurants
                .Include(r => r.MenuItems)
                .FirstOrDefaultAsync(r => r.RestaurantId == id);
        }

        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            return await _context.Restaurants.Include(r => r.MenuItems).ToListAsync();
        }

        public async Task AddRestaurantAsync(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRestaurantAsync(Restaurant restaurant)
        {
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRestaurantAsync(int id)
        {
            var restaurant = await GetRestaurantAsync(id);
            if (restaurant != null)
            {
                _context.Restaurants.Remove(restaurant);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<IEnumerable<Restaurant>> SearchRestaurantsAsync(string query)
        {
            var restaurants = _context.Restaurants.AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                query = query.ToLower();

                // Search only by restaurant name
                restaurants = restaurants.Where(r => r.Name.ToLower().Contains(query));
            }

            return await restaurants.ToListAsync();
        }

        // Search restaurants by location (address)
        public async Task<IEnumerable<Restaurant>> SearchRestaurantsByLocationAsync(string location)
        {
            var restaurants = _context.Restaurants.AsQueryable();

            if (!string.IsNullOrEmpty(location))
            {
                location = location.ToLower();
                restaurants = restaurants.Where(r => r.Address.ToLower().Contains(location));
            }

            return await restaurants.ToListAsync();
        }
        public async Task<Restaurant> GetRestaurantByOwnerId(int ownerId)
        {
            return await _context.Restaurants
                .Include(r => r.MenuItems) // Include related menu items if needed
                .FirstOrDefaultAsync(r => r.OwnerId == ownerId);
        }
        public async Task<Restaurant> GetByIdAsync(int restaurantId)
        {
            return await _context.Restaurants.FindAsync(restaurantId); // Fetch the restaurant by ID
        }

    }
}

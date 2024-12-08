using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Repositories;

namespace FoodDeliveryManagementSystem.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepo;

        public RestaurantService(IRestaurantRepository restaurantRepo)
        {
            _restaurantRepo = restaurantRepo;
        }

        public async Task<Restaurant> GetRestaurantAsync(int id)
        {
            return await _restaurantRepo.GetRestaurantAsync(id);
        }

        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            return await _restaurantRepo.GetAllRestaurantsAsync();
        }

        public async Task CreateRestaurantAsync(Restaurant restaurant)
        {
            await _restaurantRepo.AddRestaurantAsync(restaurant);
        }

        public async Task<bool> UpdateRestaurant(int id, Restaurant updatedRestaurant)
        {
            // Fetch the restaurant from the database by its ID
            var restaurant = await _restaurantRepo.GetRestaurantAsync(id);

            // If the restaurant doesn't exist, return false
            if (restaurant == null)
                return false;

            // Update the restaurant properties with the new values
            restaurant.Name = updatedRestaurant.Name;
            restaurant.Address = updatedRestaurant.Address;
            restaurant.CuisineType = updatedRestaurant.CuisineType;
            restaurant.Phone = updatedRestaurant.Phone;
            restaurant.HoursOfOperation = updatedRestaurant.HoursOfOperation;
            restaurant.Status = updatedRestaurant.Status;

            // Save the updated restaurant to the database
            await _restaurantRepo.UpdateRestaurantAsync(restaurant);
            return true;
        }


        public async Task DeleteRestaurantAsync(int id)
        {
            await _restaurantRepo.DeleteRestaurantAsync(id);
        }
        public async Task<IEnumerable<Restaurant>> SearchRestaurantsAsync(string query)
        {
            var restaurants = await _restaurantRepo.GetAllRestaurantsAsync();

            if (string.IsNullOrEmpty(query))
                return restaurants;

            query = query.ToLower();

            return restaurants.Where(r =>
                r.Name.ToLower().Contains(query) ||
                r.CuisineType.ToLower().Contains(query) ||
                r.Address.ToLower().Contains(query));
        }

        // Method to search restaurants by location (address)
        public async Task<IEnumerable<Restaurant>> SearchRestaurantsByLocationAsync(string location)
        {
            var restaurants = await _restaurantRepo.GetAllRestaurantsAsync();

            if (string.IsNullOrEmpty(location))
                return restaurants;

            location = location.ToLower();

            return restaurants.Where(r => r.Address.ToLower().Contains(location));
        }
        public async Task<Restaurant> GetRestaurantByOwnerId(int ownerId)
        {
            return await _restaurantRepo.GetRestaurantByOwnerId(ownerId);
        }
    }
}

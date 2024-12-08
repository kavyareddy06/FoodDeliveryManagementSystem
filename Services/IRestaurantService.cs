using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Services
{
    public interface IRestaurantService
    {
         
           public Task<Restaurant> GetRestaurantAsync(int id);
        public Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
        public Task CreateRestaurantAsync(Restaurant restaurant);
        public Task<bool> UpdateRestaurant(int id, Restaurant updatedRestaurant);
        public Task DeleteRestaurantAsync(int id);
        public Task<IEnumerable<Restaurant>> SearchRestaurantsAsync(string query);
        public Task<IEnumerable<Restaurant>> SearchRestaurantsByLocationAsync(string location);
        public Task<Restaurant> GetRestaurantByOwnerId(int ownerId);


    }
}

using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Repositories
{
    public interface IRestaurantRepository
    {
        public Task<Restaurant> GetRestaurantAsync(int id);
        public Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
        public Task AddRestaurantAsync(Restaurant restaurant);
        public Task UpdateRestaurantAsync(Restaurant restaurant);
        public Task DeleteRestaurantAsync(int id);
        public Task<IEnumerable<Restaurant>> SearchRestaurantsAsync(string query);
        public Task<IEnumerable<Restaurant>> SearchRestaurantsByLocationAsync(string location);
        public Task<Restaurant> GetRestaurantByOwnerId(int ownerId);
        public Task<Restaurant> GetByIdAsync(int restaurantId);
    }
}

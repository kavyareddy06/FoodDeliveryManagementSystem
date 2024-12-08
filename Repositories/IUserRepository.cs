using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Repositories
{
    public interface IUserRepository
    {
        public Task<User> GetUserByEmail(string email);
        public Task AddUser(User user);
        public Task<User> GetByIdAsync(int userId);
    }
}

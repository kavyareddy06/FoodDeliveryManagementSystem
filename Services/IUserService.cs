using FoodDeliveryManagementSystem.Models;

namespace FoodDeliveryManagementSystem.Services
{
    public interface IUserService
    {

        public Task<string> AuthenticateUser(string email, string password);
        public Task Register(User user);
        public Task<User> GetUserByIdAsync(int userId);
        public Task<List<User>> GetUsersByRoleAsync(string role);

    }
}

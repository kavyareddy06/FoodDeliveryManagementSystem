using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagementSystem.Repositories
{
    public class UserRepository:IUserRepository
    {
        private readonly FoodDeliveryContext _context;

        public UserRepository(FoodDeliveryContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

       

        public async Task AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User> GetByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId); // This fetches the user by ID
        }
    }
}

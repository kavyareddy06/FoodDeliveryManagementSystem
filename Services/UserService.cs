using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
       private readonly JwtService _jwtService;
        private readonly FoodDeliveryContext _context;

        public UserService(IUserRepository userRepository,JwtService jwtService,FoodDeliveryContext context)
        {
            _userRepository = userRepository;
            _jwtService=jwtService;
            _context = context;
          
        }
        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.UserId == userId)
                .FirstOrDefaultAsync(); // Assuming 'Users' is the DbSet for user records
        }
        public async Task<string> AuthenticateUser(string email, string password)
        {
            // Retrieve the user by email
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                return null;  // Return null if the user does not exist
            }

            if (user.PasswordHash != password)
            {
                return null;  // Return null if the password doesn't match
            }

            
            var token = _jwtService.GenerateToken(user.Email, user.Role,user.UserId);
            return token;
        }
        public async Task<List<User>> GetUsersByRoleAsync(string role)
        {
            return await _context.Users
                .Where(u => u.Role == role)
                .ToListAsync();
        }



        public async Task Register(User user)
        {
           // user.PasswordHash = password; 
            await _userRepository.AddUser(user); 
        }

    }



}

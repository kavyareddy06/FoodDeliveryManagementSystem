using FoodDeliveryManagementSystem.Models;
using FoodDeliveryManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtService _jwtService;

        public AuthController(IUserService userService,JwtService jwtService)
        {
            _userService = userService;
            _jwtService=jwtService;
           
        }
        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUserById(int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user); // Return user details including the username
        }

        [HttpPost("login")]
        [AllowAnonymous] 
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            if (login == null)
                return BadRequest("Invalid client request");

            var token = await _userService.AuthenticateUser(login.Email, login.Password);

            if (token == null)
                return Unauthorized("Invalid credentials.");

            return Ok(new { Token = token });
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Role = model.Role,
                IsActive = true,
                PasswordHash = model.Password // Store the plain-text password (not hashed)
            };

            await _userService.Register(user);  // Save user without password hashing
            return Ok(new { message = "User registered successfully" });
        }

    }
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string Password { get; set; } 
    }

}

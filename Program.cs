using FoodDeliveryManagementSystem.Data;
using FoodDeliveryManagementSystem.Repositories;
using FoodDeliveryManagementSystem.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var jwtval = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtval["Key"]);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Use ReferenceHandler.Preserve to handle circular references
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
builder.Services.AddScoped<IMenuItemService, MenuItemService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderRepository,OrderRepository>();
builder.Services.AddScoped<IPaymentRepository,PaymentRepository>();
builder.Services.AddScoped<IOrderService,OrderService>();
builder.Services.AddScoped<IPaymentService,PaymentService>();
builder.Services.AddScoped<IPromotionRepository,PromotionRepository>();
builder.Services.AddScoped<IPromotionService,PromotionService>();
builder.Services.AddScoped<IFeedbackRepository, FeedbackRepository>();
builder.Services.AddScoped<IFeedbackService, FeedbackService>();
builder.Services.AddScoped<IOrderTrackingRepository, OrderTrackingRepository>();
builder.Services.AddScoped<IOrderTrackingService, OrderTrackingService>();
builder.Services.AddScoped<IDeliveryRepository,DeliveryRepository>();
builder.Services.AddScoped<IDeliveryService,DeliveryService>();
builder.Services.AddScoped<IOrderReportRepository,OrderReportRepository>();
builder.Services.AddScoped<IOrderReportService,OrderReportService>();
builder.Services.AddScoped<IInventoryRepository, InventoryRepository>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddSingleton<TwilioTestService>();
//builder.Services.AddSignalR();
//builder.Services.AddScoped<IOrderRepository,OrderRepository>();
//builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
////builder.Services.AddScoped<IOrderTrackingRepository, OrderTrackingRepository>();
//builder.Services.AddScoped<IOrderService, OrderService>();
//builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<InvoiceService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddSingleton<IHostedService, OrderStatusUpdateService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular development server origin
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddDbContext<FoodDeliveryContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DeafultConnection")));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtval["Issuer"],
        ValidAudience = jwtval["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)

    };
});
builder.Services.AddAuthorization(options =>
{
    // Admin-only policy
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

    // Restaurant Owner-only policy
    options.AddPolicy("RestaurantOwnerOnly", policy => policy.RequireRole("RestaurantOwner"));

    // Delivery Driver-only policy
    options.AddPolicy("DeliveryDriverOnly", policy => policy.RequireRole("DeliveryDriver"));

    // Customer-only policy
    options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));

    // Any role policy (Admin, Restaurant Owner, Delivery Driver, or Customer)
    options.AddPolicy("AllRoles", policy => policy.RequireRole("Admin", "RestaurantOwner", "DeliveryDriver", "Customer"));
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngularApp");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

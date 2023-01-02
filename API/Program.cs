using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineFoodDelivery.Data;
using OnlineFoodDelivery.Models;
using OnlineFoodDelivery.Policies;
using OnlineFoodDelivery.Policies.Handlers;
using OnlineFoodDelivery.Policies.Requirements;
using OnlineFoodDelivery.Repository;
using OnlineFoodDelivery.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();

// Configure swagger to let us enter a JWT token for authentication
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter a valid JWT token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDBContext>();

// Policy based authorization implementing resource based authorization
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("OwnsOrder", policy =>
        policy.Requirements.Add(new OwnsOrderRequirement()))
    .AddPolicy("IsDeliveryRecipient", policy =>
        policy.Requirements.Add(new IsDeliveryRecipientRequirement()))
    .AddPolicy("IsDeliveryDriver", policy =>
        policy.Requirements.Add(new IsDeliveryDriverRequirement()))
    .AddPolicy("OwnsRestaurant", policy =>
        policy.Requirements.Add(new OwnsRestaurantRequirement()));


builder.Services.AddSingleton<IAuthorizationHandler, OwnsOrderHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, IsDeliveryRecipientHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, IsDeliveryDriverHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, OwnsRestaurantHandler>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])
            )
        };
    });

// Services and Repositories
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Update database automatically
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<ApplicationDBContext>()
        .Database.Migrate();
}




app.Run();


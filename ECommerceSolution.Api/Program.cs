using ECommerceSolution.BLL.DTOs;
using ECommerceSolution.BLL.Interfaces;
using ECommerceSolution.BLL.Mapper;
using ECommerceSolution.BLL.Services;
using ECommerceSolution.BLL.Validators;
using ECommerceSolution.DAL.Interfaces;
using ECommerceSolution.DAL.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ECommerceSolution.BLL.PasswordHashers;
using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);

// Configure Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Configure Authorization
builder.Services.AddAuthorization();

// HttpContext Accessor
builder.Services.AddHttpContextAccessor();

// User Context Dependency
builder.Services.AddScoped<IUserContext, UserContextService>();

// Controllers with JSON Options
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// API Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cross-Origin Resource Sharing
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials();
    });
});

// Database Context Configuration
var connectionString = "mongodb://localhost:27017";
var databaseName = "test";
builder.Services.AddSingleton<IMongoDBContext>(_ => new MongoDBContext(connectionString, databaseName));

// Repository Configuration
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();

// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(Program).Assembly, typeof(MappingProfile).Assembly);

// Password Hasher
builder.Services.AddTransient<IPasswordHasher, BCryptPasswordHasher>();

// Token Service
builder.Services.AddTransient<ITokenService, TokenService>();

// Validators
builder.Services.AddTransient<IValidator<ProductDTO>, ProductValidator>();
builder.Services.AddTransient<IValidator<UserRegistrationDTO>, UserValidator>();

// Service Layer
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IUserService, UserService>();

// Rate Limiting Configuration
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

var app = builder.Build();

// Middleware Configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowLocalhost3000");
app.UseIpRateLimiting();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

using Microsoft.EntityFrameworkCore;
using BikeRental.Domain;
using BikeRental.Domain.Models;
using BikeRental.Application.Contracts.Bike;
using BikeRental.Application.Contracts.BikeModel;
using BikeRental.Application.Contracts.Renter;
using BikeRental.Application.Contracts.Rental;
using BikeRental.Application.Contracts;
using BikeRental.Application;
using BikeRental.Application.Services;
using BikeRental.Infrastructure.EfCore.Repository;
using BikeRental.Infrastructure.EfCore;
using BikeRental.Domain.DataSeeder;
using MongoDB.Driver;
using BikeRental.ServiceDefaults;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add default service configurations
builder.AddServiceDefaults();

// AutoMapper configuration for DTO mappings
builder.Services.AddAutoMapper(config => config.AddProfile<BikeRentalProfile>());

// Data seeder for initial test data
builder.Services.AddSingleton<DataSeeder>();

// Entity Framework Core repositories registration
builder.Services.AddScoped<IRepository<Bike, int>, BikeEfCoreRepository>();
builder.Services.AddScoped<IRepository<BikeModel, int>, BikeModelEfCoreRepository>();
builder.Services.AddScoped<IRepository<Renter, int>, RenterEfCoreRepository>();
builder.Services.AddScoped<IRepository<Rental, int>, RentalEfCoreRepository>();

// Application services registration
builder.Services.AddScoped<IBikeService, BikeService>();
builder.Services.AddScoped<IBikeModelService, BikeModelService>();
builder.Services.AddScoped<IRenterService, RenterService>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

// Controller configuration with JSON serialization options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Swagger/OpenAPI configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MongoDB client configuration
builder.Services.AddSingleton<IMongoClient>(sp => 
    new MongoClient("mongodb://localhost:27017"));

// Entity Framework Core DbContext configuration with MongoDB provider
builder.Services.AddDbContext<BikeRentalDbContext>((services, options) =>
{
    var mongoClient = services.GetRequiredService<IMongoClient>();
    options.UseMongoDB(mongoClient, "bikerental");
});

var app = builder.Build();

// Map default endpoints for service health checks
app.MapDefaultEndpoints();

// Database initialization and data seeding
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<BikeRentalDbContext>();
        var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();

        // Ensure database is created
        context.Database.EnsureCreated();

        // Check if database already contains data
        if (!context.BikeModels!.Any())
        {
            var models = seeder.Models;
            var renters = seeder.Renters;
            var bikes = seeder.Bikes;
            var rentals = seeder.Rentals;

            // Seed database with test data
            context.BikeModels!.AddRange(models);
            context.Renters!.AddRange(renters);
            context.Bikes!.AddRange(bikes);
            context.Rentals!.AddRange(rentals);

            context.SaveChanges();
            Console.WriteLine("Database seeded with test data!");
        }
        else
        {
            Console.WriteLine("Database already contains data.");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error during database seeding: {ex.Message}");
    Console.WriteLine($"StackTrace: {ex.StackTrace}");
}

// Configure Swagger UI in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure middleware pipeline
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
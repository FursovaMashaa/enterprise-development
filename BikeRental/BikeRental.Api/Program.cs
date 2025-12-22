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
using BikeRental.ServiceDefaults;
using MongoDB.Driver;
using System.Text.Json.Serialization;
using System.Reflection;

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
builder.Services.AddSwaggerGen(c =>
{
    // Добавляем XML комментарии из текущего проекта (API)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
        Console.WriteLine($"Loaded XML comments from: {xmlPath}");
    }
    else
    {
        Console.WriteLine($"XML file not found: {xmlPath}");
    }

    // Добавляем XML комментарии из проекта Application.Contracts
    try
    {
        // Получаем сборку с DTO (например, используя BikeDto)
        var contractsAssembly = typeof(BikeRental.Application.Contracts.Bike.BikeDto).Assembly;
        var contractsXmlFile = $"{contractsAssembly.GetName().Name}.xml";
        
        // Ищем XML файл в нескольких местах
        var possiblePaths = new[]
        {
            Path.Combine(AppContext.BaseDirectory, contractsXmlFile),
            Path.Combine(Path.GetDirectoryName(contractsAssembly.Location) ?? "", contractsXmlFile),
            Path.Combine(Directory.GetCurrentDirectory(), "..", "BikeRental.Application.Contracts", "bin", "Debug", "net8.0", contractsXmlFile),
            Path.Combine(Directory.GetCurrentDirectory(), "..", "BikeRental.Application.Contracts", "bin", "Release", "net8.0", contractsXmlFile)
        };

        var contractsXmlLoaded = false;
        foreach (var path in possiblePaths)
        {
            if (File.Exists(path))
            {
                c.IncludeXmlComments(path, true); // true - включить комментарии из включаемых типов
                Console.WriteLine($"Loaded Contracts XML comments from: {path}");
                contractsXmlLoaded = true;
                break;
            }
        }

        if (!contractsXmlLoaded)
        {
            Console.WriteLine($"Contracts XML file not found. Searched in:");
            foreach (var path in possiblePaths)
            {
                Console.WriteLine($"  {path}");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error loading XML comments from Contracts assembly: {ex.Message}");
    }
});

// Добавьте MongoDB клиент через Aspire (как в примере)
builder.AddMongoDBClient("bikerental");

// Настройка DbContext через DI (как в примере)
builder.Services.AddDbContext<BikeRentalDbContext>((services, options) =>
{
    var db = services.GetRequiredService<IMongoDatabase>();
    options.UseMongoDB(db.Client, db.DatabaseNamespace.DatabaseName);
});

var app = builder.Build();

// Map default endpoints for service health checks
app.MapDefaultEndpoints();

// Database initialization and data seeding
try
{
    using var scope = app.Services.CreateScope();
    {
        var context = scope.ServiceProvider.GetRequiredService<BikeRentalDbContext>();
        var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();

        // Ensure database is created
        context.Database.EnsureCreated();

        // Check if database already contains data
        if (!context.BikeModels.Any())
        {
            var models = seeder.Models;
            var renters = seeder.Renters;
            var bikes = seeder.Bikes;
            var rentals = seeder.Rentals;

            // Seed database with test data
            context.BikeModels.AddRange(models);
            context.Renters.AddRange(renters);
            context.Bikes.AddRange(bikes);
            context.Rentals.AddRange(rentals);

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
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bike Rental API V1");
        c.RoutePrefix = "swagger"; // Делает Swagger доступным по /swagger
        c.DisplayRequestDuration(); // Показывать время выполнения запросов
    });
}

// Configure middleware pipeline
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
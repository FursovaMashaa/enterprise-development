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

builder.AddServiceDefaults();

// AutoMapper
builder.Services.AddAutoMapper(config => config.AddProfile<BikeRentalProfile>());

// Сидер данных
builder.Services.AddSingleton<DataSeeder>();

// Репозитории
builder.Services.AddScoped<IRepository<Bike, int>, BikeEfCoreRepository>();
builder.Services.AddScoped<IRepository<BikeModel, int>, BikeModelEfCoreRepository>();
builder.Services.AddScoped<IRepository<Renter, int>, RenterEfCoreRepository>();
builder.Services.AddScoped<IRepository<Rental, int>, RentalEfCoreRepository>();

// Сервисы
builder.Services.AddScoped<IBikeService, BikeService>();
builder.Services.AddScoped<IBikeModelService, BikeModelService>();
builder.Services.AddScoped<IRenterService, RenterService>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMongoClient>(sp => 
    new MongoClient("mongodb://localhost:27017"));

builder.Services.AddDbContext<BikeRentalDbContext>((services, options) =>
{
    var mongoClient = services.GetRequiredService<IMongoClient>();
    options.UseMongoDB(mongoClient, "bikerental");
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Автоматическое заполнение БД - ИСПРАВЛЕННАЯ ВЕРСИЯ
try
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<BikeRentalDbContext>();
        var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();

        // Создадим базу данных и коллекции если их нет
        context.Database.EnsureCreated();

        // Проверяем, есть ли данные в BikeModels (можно любую коллекцию)
        if (!context.BikeModels!.Any())
        {
            var models = seeder.Models;
            var renters = seeder.Renters;
            var bikes = seeder.Bikes;
            var rentals = seeder.Rentals;

            context.BikeModels!.AddRange(models);
            context.Renters!.AddRange(renters);
            context.Bikes!.AddRange(bikes);
            context.Rentals!.AddRange(rentals);

            context.SaveChanges();
            Console.WriteLine("База данных заполнена тестовыми данными!");
        }
        else
        {
            Console.WriteLine("База данных уже содержит данные.");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка при заполнении БД: {ex.Message}");
    Console.WriteLine($"StackTrace: {ex.StackTrace}");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
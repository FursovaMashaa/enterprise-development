using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace BikeRental.Infrastructure.EfCore;

/// <summary>
/// Entity Framework Core database context for the bike rental system using MongoDB as the backing store.
/// Configures entity mappings, relationships, and MongoDB-specific settings for all domain entities.
/// </summary>
public class BikeRentalDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BikeRentalDbContext"/> class
    /// </summary>
    /// <param name="options">DbContext options for configuration</param>
    public BikeRentalDbContext(DbContextOptions<BikeRentalDbContext> options) : base(options)
    {
        Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
    }

    /// <summary>
    /// Gets or sets the bike models collection in the database
    /// </summary>
    public DbSet<BikeModel>? BikeModels { get; set; }

    /// <summary>
    /// Gets or sets the bikes collection in the database
    /// </summary>
    public DbSet<Bike>? Bikes { get; set; }

    /// <summary>
    /// Gets or sets the renters collection in the database
    /// </summary>
    public DbSet<Renter>? Renters { get; set; }

    /// <summary>
    /// Gets or sets the rentals collection in the database
    /// </summary>
    public DbSet<Rental>? Rentals { get; set; }

    /// <summary>
    /// Configures the database provider and connection settings for the context
    /// </summary>
    /// <param name="optionsBuilder">Options builder for configuring DbContext options</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMongoDB("mongodb://localhost:27017", "bikerental");
        }
        
        optionsBuilder.EnableThreadSafetyChecks(false);
    }

    /// <summary>
    /// Configures the entity mappings and MongoDB-specific configurations for all domain entities
    /// </summary>
    /// <param name="modelBuilder">Model builder for configuring entity mappings</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BikeModel>(builder =>
        {
            builder.ToCollection("bike_models");

            builder.HasKey(bm => bm.Id);
            builder.Property(bm => bm.Id)
                    .HasElementName("_id")
                    .ValueGeneratedOnAdd();

            builder.Property(bm => bm.BikeType)
                    .IsRequired()
                    .HasElementName("bike_type");

            builder.Property(bm => bm.WheelSize)
                .HasElementName("wheel_size");
            
            builder.Property(bm => bm.MaxPassengerWeight)
                .HasElementName("max_passenger_weight");
            
            builder.Property(bm => bm.BikeWeight)
                .HasElementName("bike_weight");
            
            builder.Property(bm => bm.BrakeType)
                .HasElementName("brake_type");
            
            builder.Property(bm => bm.ModelYear)
                .HasElementName("model_year");
            
            builder.Property(bm => bm.PricePerHour)
                .IsRequired()
                .HasElementName("price_per_hour");
        });

        modelBuilder.Entity<Bike>(builder =>
        {
            builder.ToCollection("bikes");

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                .HasElementName("_id")
                .ValueGeneratedOnAdd();

            builder.Property(b => b.SerialNumber)
                .IsRequired()
                .HasMaxLength(50)
                .HasElementName("serial_number");

            builder.Property(b => b.Color)
                .HasMaxLength(30)
                .HasElementName("color");

            builder.Property(b => b.ModelId)
                .IsRequired()
                .HasElementName("model_id");
                
            builder.Ignore(b => b.Model); 
        });

        modelBuilder.Entity<Renter>(builder =>
        {
            builder.ToCollection("renters");
            
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id)
                .HasElementName("_id")
                .ValueGeneratedOnAdd();
            
            builder.Property(r => r.LastName)
                .IsRequired()
                .HasMaxLength(50)
                .HasElementName("last_name");
            
            builder.Property(r => r.FirstName)
                .IsRequired()
                .HasMaxLength(50)
                .HasElementName("first_name");
            
            builder.Property(r => r.MiddleName)
                .HasMaxLength(50)
                .HasElementName("middle_name");
            
            builder.Property(r => r.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20)
                .HasElementName("phone_number");
        });

        // Configure Rental entity mapping
        modelBuilder.Entity<Rental>(builder =>
        {
            builder.ToCollection("rentals");
            
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id)
                .HasElementName("_id")
                .ValueGeneratedOnAdd();
            
            builder.Property(r => r.StartTime)
                .IsRequired()
                .HasElementName("start_time");
            
            builder.Property(r => r.DurationHours)
                .IsRequired()
                .HasElementName("duration_hours");
            
            builder.Property(r => r.BikeId)
                .IsRequired()
                .HasElementName("bike_id");
            
            builder.Property(r => r.RenterId)
                .IsRequired()
                .HasElementName("renter_id");
            
            builder.Ignore(r => r.Bike);   
            builder.Ignore(r => r.Renter); 
        });
    }
}
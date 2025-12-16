using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace BikeRental.Infrastructure.EfCore;

public class BikeRentalDbContext(DbContextOptions<BikeRentalDbContext> options) : DbContext(options)
{
    public DbSet<BikeModel>? BikeModels { get; set; }
    public DbSet<Bike>? Bikes { get; set; }
    public DbSet<Renter>? Renters { get; set; }
    public DbSet<Rental>? Rentals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BikeModel>(builder =>
        {
            builder.ToCollection("bike_models");

            builder.HasKey(bm => bm.Id);
            builder.Property(bm => bm.Id)
                    .HasElementName("_id")
                    .ValueGeneratedOnAdd();;

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
                .HasElementName("price_per_hour")
                .HasPrecision(18, 2);
        });

        modelBuilder.Entity<Bike>(builder =>
        {
            builder.ToCollection("bikes");

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                .HasElementName("_id")
                .ValueGeneratedOnAdd();;

            builder.Property(b => b.SerialNumber)
                .IsRequired()
                .HasMaxLength(50)
                .HasElementName("serial_number");

            builder.Property(b => b.Color)
                .HasMaxLength(30)
                .HasElementName("color");

            builder.Property(b => b.Model)
                .IsRequired()
                .HasElementName("model_id");
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
            
            builder.Property(r => r.Bike)
                .IsRequired()
                .HasElementName("bike_id");
            
            builder.Property(r => r.Renter)
                .IsRequired()
                .HasElementName("renter_id");
        });
    }
}

        
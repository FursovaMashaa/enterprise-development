using BikeRental.Domain.DataSeed;
using BikeRental.Domain.Entities;
using Xunit;

namespace BikeRental.Domain.Tests;

public class BikeRentalTests(DataSeed.DataSeed seed) : IClassFixture<DataSeed.DataSeed>
{
    /// <summary>
    ///     Retrieves and validates all bicycles categorized as sport type for rental availability
    /// </summary>
    [Fact(DisplayName = "Sport Bicycle Inventory Check")]
    public void GetAllSportBicycles()
    {
        var sportBikes = seed.Bikes
            .Where(b => b.Model.BikeType == BikeType.Sport)
            .Select(b => new
            {
                b.Id,
                b.SerialNumber,
                b.Color,
                ModelId = b.Model.Id,
                Type = b.Model.BikeType
            })
            .ToList();

        Assert.Equal(3, sportBikes.Count);
        Assert.Contains(sportBikes, b => b.SerialNumber == "RD0022024");
        Assert.Contains(sportBikes, b => b.SerialNumber == "SPT0052025");
        Assert.All(sportBikes, bike => Assert.Equal(BikeType.Sport, bike.Type));
    }

    /// <summary>
    ///     Analyzes and ranks bicycle models by total rental revenue to identify top performers
    /// </summary>
    [Fact(DisplayName = "Revenue Analysis - Top Bicycle Models")]
    public void AnalyzeTopRevenueModels()
    {
        var revenueByModel = seed.Rentals
            .GroupBy(r => r.Bike.Model.Id)
            .Select(g => new
            {
                ModelId = g.Key,
                Revenue = g.Sum(r => r.DurationHours * r.Bike.Model.PricePerHour),
                RentalCount = g.Count()
            })
            .OrderByDescending(x => x.Revenue)
            .ThenBy(x => x.ModelId)
            .Take(5)
            .ToList();

        Assert.Equal(5, revenueByModel.Count);
        
        var expectedRevenue = new Dictionary<int, decimal>
        {
            [9] = 111.00m,
            [5] = 75.00m,
            [6] = 64.40m,
            [3] = 60.50m,
            [2] = 60.00m
        };

        foreach (var row in revenueByModel)
        {
            Assert.Equal(expectedRevenue[row.ModelId], Math.Round(row.Revenue, 2));
        }
    }

    /// <summary>
    ///     Evaluates bicycle models based on total rental duration to determine usage patterns
    /// </summary>
    [Fact(DisplayName = "Usage Patterns - Most Rented Models")]
    public void EvaluateMostUsedModels()
    {
        var usageByModel = seed.Rentals
            .GroupBy(r => r.Bike.Model)
            .Select(g => new
            {
                Model = g.Key,
                TotalRentalHours = g.Sum(r => r.DurationHours),
                AverageRentalTime = g.Average(r => r.DurationHours)
            })
            .OrderByDescending(x => x.TotalRentalHours)
            .Take(5)
            .ToList();

        Assert.Equal(5, usageByModel.Count);
        
        Assert.All(usageByModel, model => 
        {
            Assert.True(model.TotalRentalHours > 0);
            Assert.True(model.AverageRentalTime > 0);
        });
    }

    /// <summary>
    ///     Calculates comprehensive rental duration statistics for business analysis
    /// </summary>
    [Fact(DisplayName = "Rental Duration Analytics")]
    public void CalculateRentalStatistics()
    {
        var rentalDurations = seed.Rentals
            .Select(r => new { r.Id, r.DurationHours })
            .ToList();

        var minRental = rentalDurations.Min(r => r.DurationHours);
        var maxRental = rentalDurations.Max(r => r.DurationHours);
        var avgRental = rentalDurations.Average(r => r.DurationHours);

        Assert.True(minRental >= 1, "Minimum rental should be at least 1 hour");
        Assert.True(maxRental <= 24, "Maximum rental should be reasonable");
        Assert.True(avgRental >= minRental && avgRental <= maxRental);
    }

    /// <summary>
    ///     Measures total utilization hours for each bicycle category in the rental fleet
    /// </summary>
    [Theory(DisplayName = "Fleet Utilization by Category")]
    [InlineData(BikeType.Sport, 16)]
    [InlineData(BikeType.Mountain, 12)]
    [InlineData(BikeType.Road, 23)]
    [InlineData(BikeType.Hybrid, 9)]
    public void MeasureCategoryUtilization(BikeType category, int expectedUtilization)
    {
        var actualHours = seed.Rentals
            .Where(rent => rent.Bike.Model.BikeType == category)
            .Sum(rent => rent.DurationHours);

        Assert.Equal(expectedUtilization, actualHours);
    }   

    /// <summary>
    ///     Identifies most frequent customers based on rental transaction volume
    /// </summary>
    [Fact(DisplayName = "Customer Loyalty Analysis")]
    public void IdentifyFrequentCustomers()
    {
        var customerActivity = seed.Rentals
            .GroupBy(r => r.Renter)
            .Select(g => new
            {
                Customer = g.Key,
                RentalFrequency = g.Count(),
                TotalRentalHours = g.Sum(r => r.DurationHours),
                CustomerSince = g.Min(r => r.StartTime)
            })
            .OrderByDescending(x => x.RentalFrequency)
            .ThenByDescending(x => x.TotalRentalHours)
            .ToList();

        var topFrequency = customerActivity.Max(x => x.RentalFrequency);
        var loyalCustomers = customerActivity.Where(x => x.RentalFrequency == topFrequency).ToList();

        Assert.True(topFrequency >= 1, "Top customers should have at least one rental");
        Assert.NotEmpty(loyalCustomers);
        Assert.All(loyalCustomers, customer => Assert.Equal(topFrequency, customer.RentalFrequency));
    }
}
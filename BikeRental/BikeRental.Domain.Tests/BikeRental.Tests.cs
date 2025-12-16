using BikeRental.Domain.DataSeeder;
using BikeRental.Domain.Enums;

namespace BikeRental.Domain.Tests;

public class BikeRentalTests(BikeRental.Domain.DataSeeder.DataSeeder seed) : IClassFixture<BikeRental.Domain.DataSeeder.DataSeeder>
{
    /// <summary>
    /// Retrieves and validates all bicycles categorized as sport type for rental availability
    /// </summary>
    [Fact(DisplayName = "Sport Bicycle Inventory Check")]
    public void GetAllSportBicycles()
    {
        var expectedCount = 3;
        var expectedSerialNumber1 = "RD0022024";
        var expectedSerialNumber2 = "SPT0052025";

        var sportBikes = seed.Bikes
            .Where(b => seed.Models.FirstOrDefault(m => m.Id == b.ModelId)?.BikeType == BikeType.Sport)
            .ToList();

        Assert.Equal(expectedCount, sportBikes.Count);
        Assert.Contains(sportBikes, b => b.SerialNumber == expectedSerialNumber1);
        Assert.Contains(sportBikes, b => b.SerialNumber == expectedSerialNumber2);
        Assert.All(sportBikes, bike => 
            Assert.Equal(BikeType.Sport, seed.Models.First(m => m.Id == bike.ModelId).BikeType));
    }

    /// <summary>
    /// Analyzes and ranks bicycle models by total rental revenue to identify top performers
    /// </summary>
    [Fact(DisplayName = "Revenue Analysis - Top Bicycle Models")]
    public void AnalyzeTopRevenueModels()
    {
        var expectedTopCount = 5;
        var expectedRevenue = new Dictionary<int, decimal>
        {
            [9] = 111.00m,
            [5] = 75.00m,
            [6] = 64.40m,
            [3] = 60.50m,
            [2] = 60.00m
        };

        var revenueByModel = seed.Rentals
            .Select(r => new 
            {
                Rental = r,
                Model = seed.Models.FirstOrDefault(m => m.Id == 
                    seed.Bikes.FirstOrDefault(b => b.Id == r.BikeId)?.ModelId)
            })
            .Where(x => x.Model != null)
            .GroupBy(x => x.Model.Id)
            .Select(g => new
            {
                ModelId = g.Key,
                Revenue = g.Sum(x => x.Rental.DurationHours * x.Model.PricePerHour),
                RentalCount = g.Count()
            })
            .OrderByDescending(x => x.Revenue)
            .ThenBy(x => x.ModelId)
            .Take(expectedTopCount)
            .ToList();

        Assert.Equal(expectedTopCount, revenueByModel.Count);
        
        var actualRevenue = revenueByModel.ToDictionary(r => r.ModelId, r => r.Revenue);
        foreach (var expected in expectedRevenue)
        {
            Assert.Contains(expected.Key, actualRevenue.Keys);
            Assert.Equal(expected.Value, actualRevenue[expected.Key], 0.01m);
        }
    }

    /// <summary>
    /// Evaluates bicycle models based on total rental duration to determine usage patterns
    /// </summary>
    [Fact(DisplayName = "Usage Patterns - Most Rented Models")]
    public void EvaluateMostUsedModels()
    {
        var expectedTopCount = 5;
        var expectedTotalHours = new Dictionary<int, int>
        {
            [3] = 11,
            [6] = 7,
            [10] = 7,
            [8] = 6,
            [9] = 6
        };

        var usageByModel = seed.Rentals
            .Select(r => new 
            {
                Rental = r,
                Model = seed.Models.FirstOrDefault(m => m.Id == 
                    seed.Bikes.FirstOrDefault(b => b.Id == r.BikeId)?.ModelId)
            })
            .Where(x => x.Model != null)
            .GroupBy(x => x.Model.Id)
            .Select(g => new
            {
                ModelId = g.Key,
                TotalRentalHours = g.Sum(x => x.Rental.DurationHours),
                AverageRentalTime = g.Average(x => x.Rental.DurationHours)
            })
            .OrderByDescending(x => x.TotalRentalHours)
            .Take(expectedTopCount)
            .ToList();

        Assert.Equal(expectedTopCount, usageByModel.Count);

        var actualHours = usageByModel.ToDictionary(ubm => ubm.ModelId, ubm => ubm.TotalRentalHours);
        foreach (var expected in expectedTotalHours)
        {
            Assert.Contains(expected.Key, actualHours.Keys);
            Assert.Equal(expected.Value, actualHours[expected.Key]);
        }
        Assert.All(usageByModel, ubm => Assert.True(ubm.AverageRentalTime > 0));
    }

    /// <summary>
    /// Calculates comprehensive rental duration statistics for business analysis
    /// </summary>
    [Fact(DisplayName = "Rental Duration Analytics")]
    public void CalculateRentalStatistics()
    {
        var expectedMinRental = 1;
        var expectedMaxRental = 6;
        var expectedAvgRental = 3.0;

        var rentalDurations = seed.Rentals
            .Select(r => r.DurationHours)
            .ToList();

        var minRental = rentalDurations.Min();
        var maxRental = rentalDurations.Max();
        var avgRental = rentalDurations.Average();

        Assert.Equal(expectedMinRental, minRental);
        Assert.Equal(expectedMaxRental, maxRental);
        Assert.Equal(expectedAvgRental, avgRental, 0.01);
    }

    /// <summary>
    /// Measures total utilization hours for each bicycle category in the rental fleet
    /// </summary>
    [Theory(DisplayName = "Fleet Utilization by Category")]
    [InlineData(BikeType.Sport, 16)]
    [InlineData(BikeType.Mountain, 12)]
    [InlineData(BikeType.Road, 23)]
    [InlineData(BikeType.Hybrid, 9)]
    public void MeasureCategoryUtilization(BikeType category, int expectedUtilization)
    {
        var actualHours = seed.Rentals
            .Select(r => new 
            {
                Rental = r,
                Model = seed.Models.FirstOrDefault(m => m.Id == 
                    seed.Bikes.FirstOrDefault(b => b.Id == r.BikeId)?.ModelId)
            })
            .Where(x => x.Model?.BikeType == category)
            .Sum(x => x.Rental.DurationHours);

        Assert.Equal(expectedUtilization, actualHours);
    }

    /// <summary>
    /// Identifies most frequent customers based on rental transaction volume
    /// </summary>
    [Fact(DisplayName = "Customer Loyalty Analysis")]
    public void IdentifyFrequentCustomers()
    {
        var expectedTopFrequency = 3;
        var expectedLoyalCustomersCount = 5;

        var customerActivity = seed.Rentals
            .GroupBy(r => r.RenterId)
            .Select(g => new
            {
                Customer = seed.Renters.FirstOrDefault(r => r.Id == g.Key),
                RentalFrequency = g.Count(),
                TotalRentalHours = g.Sum(r => r.DurationHours),
                CustomerSince = g.Min(r => r.StartTime)
            })
            .Where(x => x.Customer != null)
            .OrderByDescending(x => x.RentalFrequency)
            .ThenByDescending(x => x.TotalRentalHours)
            .ToList();

        var topFrequency = customerActivity.Max(x => x.RentalFrequency);
        var loyalCustomers = customerActivity
            .Where(x => x.RentalFrequency == topFrequency)
            .ToList();

        Assert.Equal(expectedTopFrequency, topFrequency);
        Assert.Equal(expectedLoyalCustomersCount, loyalCustomers.Count);
        Assert.All(loyalCustomers, customer =>
            Assert.Equal(expectedTopFrequency, customer.RentalFrequency));
    }
}
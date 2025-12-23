using BikeRental.Application.Contracts.Rental;
using Bogus;

namespace BikeRental.Generator.Nats.Host;

/// <summary>
/// Generator for test rental data.
/// </summary>
public static class RentalGenerator
{
    /// <summary>
    /// Generates a batch of test rental records.
    /// </summary>
    /// <param name="count">Number of records to generate.</param>
    /// <returns>List of test rental data.</returns>
    public static IList<RentalCreateUpdateDto> GenerateBatch(int count) =>
        new Faker<RentalCreateUpdateDto>()
            .CustomInstantiator(f => new RentalCreateUpdateDto(
                StartTime: DateTime.UtcNow.AddHours(f.Random.Int(1, 72)),
                DurationHours: f.Random.Int(1, 24),
                BikeId: f.Random.Int(1, 10),
                RenterId: f.Random.Int(1, 20)
            ))
            .Generate(count);
}
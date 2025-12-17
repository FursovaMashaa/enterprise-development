namespace BikeRental.Application.Contracts.Bike;

/// <summary>
/// Data transfer object representing a bike entity.
/// Contains all properties for viewing bike information, including its unique identifier.
/// </summary>
/// <param name="Id">Unique identifier of the bike</param>
/// <param name="SerialNumber">Unique serial number identifier for the bike</param>
/// <param name="Color">Optional color description of the bike</param>
/// <param name="ModelId">Identifier of the bike model this bike belongs to</param>
public record BikeDto(
    int Id, 
    string SerialNumber,
    string? Color,
    int ModelId 
);
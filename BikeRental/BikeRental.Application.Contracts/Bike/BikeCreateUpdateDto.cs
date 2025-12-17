namespace BikeRental.Application.Contracts.Bike;

/// <summary>
/// Data transfer object for creating or updating bike information.
/// Contains the essential properties required for bike creation and modification operations.
/// </summary>
/// <param name="SerialNumber">Unique serial number identifier for the bike</param>
/// <param name="Color">Optional color description of the bike</param>
/// <param name="ModelId">Identifier of the bike model this bike belongs to</param>
public record BikeCreateUpdateDto(
    string SerialNumber,
    string? Color,
    int ModelId
);
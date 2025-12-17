namespace BikeRental.Application.Contracts.BikeModel;

/// <summary>
/// Data transfer object for creating or updating bike model information.
/// Contains the essential properties required for bike model creation and modification operations.
/// </summary>
/// <param name="WheelSize">Optional wheel size in inches</param>
/// <param name="MaxPassengerWeight">Optional maximum passenger weight capacity in kilograms</param>
/// <param name="BikeWeight">Optional weight of the bike in kilograms</param>
/// <param name="BrakeType">Optional description of the brake system type</param>
/// <param name="ModelYear">Optional manufacturing year of the model</param>
/// <param name="PricePerHour">Rental price per hour for this model</param>
/// <param name="BikeType">Optional bike type identifier (0: Sport, 1: Mountain, 2: Road, 3: Hybrid)</param>
public record BikeModelCreateUpdateDto(
    double? WheelSize,
    double? MaxPassengerWeight,
    double? BikeWeight,
    string? BrakeType,
    int? ModelYear,
    decimal PricePerHour,
    int? BikeType
);
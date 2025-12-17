using BikeRental.Domain.Enums;

namespace BikeRental.Application.Contracts.BikeModel;

/// <summary>
/// Data transfer object representing a bike model entity.
/// Contains all properties for viewing bike model information, including its unique identifier and type.
/// </summary>
/// <param name="Id">Unique identifier of the bike model</param>
/// <param name="BikeType">Type/category of the bike (Sport, Mountain, Road, Hybrid)</param>
/// <param name="WheelSize">Optional wheel size in inches</param>
/// <param name="MaxPassengerWeight">Optional maximum passenger weight capacity in kilograms</param>
/// <param name="BikeWeight">Optional weight of the bike in kilograms</param>
/// <param name="BrakeType">Optional description of the brake system type</param>
/// <param name="ModelYear">Optional manufacturing year of the model</param>
/// <param name="PricePerHour">Rental price per hour for this model</param>
public record BikeModelDto(
    int Id, 
    BikeType BikeType, 
    double? WheelSize,
    double? MaxPassengerWeight,
    double? BikeWeight,
    string? BrakeType,
    int? ModelYear, 
    decimal PricePerHour
);
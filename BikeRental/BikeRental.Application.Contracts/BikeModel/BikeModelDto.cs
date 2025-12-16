using BikeRental.Domain.Enums;

namespace BikeRental.Application.Contracts.BikeModel;

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
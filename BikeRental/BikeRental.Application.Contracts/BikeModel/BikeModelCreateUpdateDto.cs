namespace BikeRental.Application.Contracts.BikeModel;

public record BikeModelCreateUpdateDto(
    double? WheelSize,
    double? MaxPassengerWeight,
    double? BikeWeight,
    string? BrakeType,
    int? ModelYear,
    decimal PricePerHour,
    int? BikeType
);

namespace BikeRental.Application.Contracts.Bike;

public record BikeCreateUpdateDto(
    string SerialNumber,
    string? Color,
    int ModelId
);
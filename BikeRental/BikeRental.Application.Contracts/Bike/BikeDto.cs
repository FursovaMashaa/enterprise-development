namespace BikeRental.Application.Contracts.Bike;

public record BikeDto(
    int Id, 
    string SerialNumber,
    string? Color,
    int ModelId 
);
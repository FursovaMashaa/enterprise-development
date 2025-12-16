namespace BikeRental.Application.Contracts.Rental;

public record RentalCreateUpdateDto(
    DateTime StartTime,
    int DurationHours,
    int BikeId,
    int RenterId
);
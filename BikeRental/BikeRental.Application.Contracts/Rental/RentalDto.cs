namespace BikeRental.Application.Contracts.Rental;

public record RentalDto(
    int Id, 
    DateTime StartTime,
    int DurationHours, 
    int BikeId, 
    int RenterId 
);
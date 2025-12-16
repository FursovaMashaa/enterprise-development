namespace BikeRental.Application.Contracts.Renter;

public record RenterDto(
    int Id,
    string LastName,
    string FirstName,
    string? MiddleName,
    string PhoneNumber
);
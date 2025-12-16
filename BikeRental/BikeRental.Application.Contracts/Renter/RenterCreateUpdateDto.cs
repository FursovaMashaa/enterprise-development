namespace BikeRental.Application.Contracts.Renter;

public record RenterCreateUpdateDto(
    string LastName,
    string FirstName,
    string? MiddleName,
    string PhoneNumber
);
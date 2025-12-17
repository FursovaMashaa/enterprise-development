namespace BikeRental.Application.Contracts.Renter;

/// <summary>
/// Data transfer object for creating or updating renter information.
/// Contains the essential properties required for renter creation and modification operations.
/// </summary>
/// <param name="LastName">Last name/surname of the renter</param>
/// <param name="FirstName">First name/given name of the renter</param>
/// <param name="MiddleName">Optional middle name/patronymic of the renter</param>
/// <param name="PhoneNumber">Contact phone number of the renter</param>
public record RenterCreateUpdateDto(
    string LastName,
    string FirstName,
    string? MiddleName,
    string PhoneNumber
);
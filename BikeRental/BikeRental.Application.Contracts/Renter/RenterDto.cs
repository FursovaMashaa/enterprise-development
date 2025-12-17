namespace BikeRental.Application.Contracts.Renter;

/// <summary>
/// Data transfer object representing a renter entity.
/// Contains all properties for viewing renter information, including their unique identifier and contact details.
/// </summary>
/// <param name="Id">Unique identifier of the renter</param>
/// <param name="LastName">Last name/surname of the renter</param>
/// <param name="FirstName">First name/given name of the renter</param>
/// <param name="MiddleName">Optional middle name/patronymic of the renter</param>
/// <param name="PhoneNumber">Contact phone number of the renter</param>
public record RenterDto(
    int Id,
    string LastName,
    string FirstName,
    string? MiddleName,
    string PhoneNumber
);
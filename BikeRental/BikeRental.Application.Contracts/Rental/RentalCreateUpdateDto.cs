namespace BikeRental.Application.Contracts.Rental;

/// <summary>
/// Data transfer object for creating or updating rental transaction information.
/// Contains the essential properties required for rental creation and modification operations.
/// </summary>
/// <param name="StartTime">Date and time when the rental period begins</param>
/// <param name="DurationHours">Duration of the rental in hours</param>
/// <param name="BikeId">Identifier of the bike being rented</param>
/// <param name="RenterId">Identifier of the renter who is renting the bike</param>
public record RentalCreateUpdateDto(
    DateTime StartTime,
    int DurationHours,
    int BikeId,
    int RenterId
);
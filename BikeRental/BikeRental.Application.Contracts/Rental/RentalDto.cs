namespace BikeRental.Application.Contracts.Rental;

/// <summary>
/// Data transfer object representing a rental transaction entity.
/// Contains all properties for viewing rental information, including its unique identifier and related entity IDs.
/// </summary>
/// <param name="Id">Unique identifier of the rental transaction</param>
/// <param name="StartTime">Date and time when the rental period began</param>
/// <param name="DurationHours">Duration of the rental in hours</param>
/// <param name="BikeId">Identifier of the bike that was rented</param>
/// <param name="RenterId">Identifier of the renter who rented the bike</param>
public record RentalDto(
    int Id, 
    DateTime StartTime,
    int DurationHours, 
    int BikeId, 
    int RenterId 
);
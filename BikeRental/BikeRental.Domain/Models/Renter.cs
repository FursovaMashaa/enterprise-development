namespace BikeRental.Domain.Models;

/// <summary>
/// Represents a customer who can rent bicycles from the rental service
/// </summary>
public class Renter
{
    /// <summary>
    /// Unique identifier for the renter
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Last name/surname of the renter
    /// </summary>
    public required string LastName { get; set; }

    /// <summary>
    /// First name of the renter
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Middle name or patronymic of the renter
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// Contact phone number of the renter
    /// </summary>
    public required string PhoneNumber { get; set; }
}
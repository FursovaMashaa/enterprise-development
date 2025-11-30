namespace BikeRental.Domain.Models;

/// <summary>
///     Represents a rental transaction where a bicycle is rented by a customer
/// </summary>
public class Rental
{
    /// <summary>
    ///     Unique identifier for the rental transaction
    /// </summary>
    public required int Id { get; set; }
    
    /// <summary>
    ///     Date and time when the rental period began
    /// </summary>
    public required DateTime StartTime { get; set; }
    
    /// <summary>
    ///     Duration of the rental in hours
    /// </summary>
    public required int DurationHours { get; set; } 
    
    /// <summary>
    ///     Reference to the rented bicycle
    /// </summary>
    public required Bike Bike { get; set; }
    
    /// <summary>
    ///     Reference to the customer who rented the bicycle
    /// </summary>
    public required Renter Renter { get; set; }
}
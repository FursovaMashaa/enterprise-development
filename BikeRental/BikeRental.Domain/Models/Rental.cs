using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRental.Domain.Models;

/// <summary>
/// Represents a rental transaction where a bicycle is rented by a customer
/// </summary>
public class Rental
{
    /// <summary>
    /// Unique identifier for the rental transaction
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Date and time when the rental period began
    /// </summary>
    public required DateTime StartTime { get; set; }

    /// <summary>
    /// Duration of the rental in hours
    /// </summary>
    public required int DurationHours { get; set; }

    /// <summary>
    /// Reference to the rented bicycle
    /// </summary>
   [NotMapped]
    public Bike? Bike { get; set; }

    /// <summary>
    /// Reference to the customer who rented the bicycle
    /// </summary>
    [NotMapped]
    public Renter? Renter { get; set; }

    public required int BikeId { get; set; }

    public required int RenterId { get; set; }
}
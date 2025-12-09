namespace BikeRental.Domain.Models;

/// <summary>
/// Represents a physical bicycle instance available for rental
/// </summary>
public class Bike
{
    /// <summary>
    ///     Unique identifier for the bicycle
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Manufacturer's serial number of the bicycle
    /// </summary>
    public required string SerialNumber { get; set; }

    /// <summary>
    /// Color of the bicycle
    /// </summary>
    public string? Color { get; set; }

    /// <summary>
    /// Reference to the bicycle model specifications
    /// </summary>
    public required BikeModel Model { get; set; }
}
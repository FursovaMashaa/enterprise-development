using BikeRental.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRental.Domain.Models;

/// <summary>
/// Bicycle model containing technical specifications and rental pricing
/// </summary>
public class BikeModel
{
    /// <summary>
    /// Unique identifier for the bicycle model
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Type/category of the bicycle
    /// </summary>
    public required BikeType BikeType { get; set; }

    /// <summary>
    /// Diameter of the wheels in inches
    /// </summary>
    public double? WheelSize { get; set; }

    /// <summary>
    /// Maximum recommended passenger weight in kilograms
    /// </summary>
    public double? MaxPassengerWeight { get; set; }

    /// <summary>
    /// Weight of the bicycle in kilograms
    /// </summary>
    public double? BikeWeight { get; set; }

    /// <summary>
    /// Type of braking system (e.g., disc, rim, coaster)
    /// </summary>
    public string? BrakeType { get; set; }

    /// <summary>
    /// Year the model was released
    /// </summary>
    public int? ModelYear { get; set; }

    /// <summary>
    /// Rental cost per hour in local currency
    /// </summary>
    public required decimal PricePerHour { get; set; }
}
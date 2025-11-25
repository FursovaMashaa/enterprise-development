using BikeRental.Domain.Entities;

namespace BikeRental.Domain.Models;

public class BikeModel
{
    public required int Id { get; set; }
    public required BikeType BikeType { get; set; }
    public double? WheelSize { get; set; } 
    public double? MaxPassengerWeight { get; set; }
    public double? BikeWeight { get; set; } 
    public string? BrakeType { get; set; }
    public int? ModelYear { get; set; }
    public required decimal PricePerHour { get; set; } 
}
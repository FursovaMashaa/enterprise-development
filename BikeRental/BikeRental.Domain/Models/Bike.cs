namespace BikeRental.Domain.Models;

public class Bike
{
    public required int Id { get; set; }
    public required string SerialNumber { get; set; } 
    public required string Color { get; set; }
    public int ModelId { get; set; } 
}
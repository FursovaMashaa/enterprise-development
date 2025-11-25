namespace BikeRental.Domain.Models;
public class Rental
{
    public required int Id { get; set; }
    public required DateTime StartTime { get; set; }
    public required int DurationHours { get; set; } 
    public required Bike BikeId { get; set; }
    public required Renter RenterId { get; set; }
}
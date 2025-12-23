namespace BikeRental.Infrastructure.Nats.Options;

public sealed class NatsOptions
{
    public const string SectionName = "Nats";
    public required string StreamName { get; init; }
    public required string SubjectName { get; init; }
}
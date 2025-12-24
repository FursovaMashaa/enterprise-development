namespace BikeRental.Application.Contracts.Nats;

/// <summary>
/// Typed configuration options for NATS message broker
/// Contains parameters used by both producer and consumer services
/// </summary>
public sealed class NatsOptions
{
    /// <summary>
    /// Configuration section name used for binding these options
    /// </summary>
    public const string SectionName = "Nats";
    
    /// <summary>
    /// Name of the JetStream stream where messages are published and consumed from
    /// </summary>
    public required string StreamName { get; init; }
    
    /// <summary>
    /// Subject name used for message publishing and stream configuration
    /// </summary>
    public required string SubjectName { get; init; }
}
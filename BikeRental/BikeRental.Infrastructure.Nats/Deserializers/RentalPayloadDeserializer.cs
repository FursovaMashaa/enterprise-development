using BikeRental.Application.Contracts.Rental;
using NATS.Client.Core;
using System.Buffers;
using System.Text.Json;

namespace BikeRental.Infrastructure.Nats.Deserializers;

/// <summary>
/// NATS message deserializer for rental data batches.
/// </summary>
public class BikeRentalPayloadDeserializer : INatsDeserialize<IList<RentalCreateUpdateDto>>
{
    /// <summary>
    /// Deserializes a byte buffer to a list of rental DTOs.
    /// </summary>
    /// <param name="buffer">The byte sequence containing JSON data.</param>
    /// <returns>Deserialized list of rental DTOs, or null if deserialization fails.</returns>
    public IList<RentalCreateUpdateDto>? Deserialize(in ReadOnlySequence<byte> buffer)
    {
        var reader = new Utf8JsonReader(buffer, isFinalBlock: true, state: default);
        return JsonSerializer.Deserialize<IList<RentalCreateUpdateDto>>(ref reader);
    }
        
}
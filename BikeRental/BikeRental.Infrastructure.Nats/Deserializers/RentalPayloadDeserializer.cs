using BikeRental.Application.Contracts.Rental;
using NATS.Client.Core;
using System.Buffers;
using System.Text.Json;

namespace BikeRental.Infrastructure.Nats.Deserializers;

/// <summary>
/// NATS message deserializer for rental data batches.
/// Implements <see cref="INatsDeserialize{T}"/> for converting byte sequences
/// to lists of <see cref="RentalCreateUpdateDto"/> objects.
/// </summary>
public class BikeRentalPayloadDeserializer : INatsDeserialize<IList<RentalCreateUpdateDto>>
{
   /// <summary>
    /// Deserializes a byte sequence from NATS message into a list of rental DTOs.
    /// Uses <see cref="Utf8JsonReader"/> for efficient JSON parsing without
    /// intermediate byte array allocation.
    /// </summary>
    /// <param name="buffer">The byte sequence containing JSON-serialized rental data.</param>
    /// <returns>
    /// Deserialized list of <see cref="RentalCreateUpdateDto"/> objects,
    /// or null if deserialization fails due to invalid JSON format.
    /// </returns>
    public IList<RentalCreateUpdateDto>? Deserialize(in ReadOnlySequence<byte> buffer)
    {
        var reader = new Utf8JsonReader(buffer, isFinalBlock: true, state: default);
        return JsonSerializer.Deserialize<IList<RentalCreateUpdateDto>>(ref reader);
    }
        
}
using BikeRental.Application.Contracts.Rental;
using NATS.Client.Core;
using System.Buffers;
using System.Text.Json;

namespace BikeRental.Infrastructure.Nats.Deserializers;

/// <summary>
/// NATS message deserializer for rental data batches.
/// </summary>
internal class BikeRentalPayloadDeserializer : INatsDeserialize<IList<RentalCreateUpdateDto>>
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    /// <summary>
    /// Deserializes a byte buffer to a list of rental DTOs.
    /// </summary>
    /// <param name="buffer">The byte sequence containing JSON data.</param>
    /// <returns>Deserialized list of rental DTOs, or null if deserialization fails.</returns>
    public IList<RentalCreateUpdateDto>? Deserialize(in ReadOnlySequence<byte> buffer) =>
        JsonSerializer.Deserialize<IList<RentalCreateUpdateDto>>(buffer.ToArray(), _options);
}
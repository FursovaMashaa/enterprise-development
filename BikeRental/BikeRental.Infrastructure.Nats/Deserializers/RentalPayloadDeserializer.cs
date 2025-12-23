using BikeRental.Application.Contracts.Rental;
using NATS.Client.Core;
using System.Buffers;
using System.Text.Json;

namespace BikeRental.Infrastructure.Nats.Deserializers;

internal class BikeRentalPayloadDeserializer : INatsDeserialize<IList<RentalCreateUpdateDto>>
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public IList<RentalCreateUpdateDto>? Deserialize(in ReadOnlySequence<byte> buffer) =>
        JsonSerializer.Deserialize<IList<RentalCreateUpdateDto>>(buffer.ToArray(), _options);
}
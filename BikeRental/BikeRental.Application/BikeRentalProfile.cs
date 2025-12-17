using AutoMapper;
using BikeRental.Application.Contracts.Bike;
using BikeRental.Application.Contracts.BikeModel;
using BikeRental.Application.Contracts.Rental;
using BikeRental.Application.Contracts.Renter;
using BikeRental.Domain.Models;

namespace BikeRental.Application;

/// <summary>
/// AutoMapper profile configuration for mapping between domain entities and DTOs in the bike rental system.
/// Defines bidirectional mappings for all entity types and their corresponding DTOs.
/// </summary>
public class BikeRentalProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BikeRentalProfile"/> class
    /// and configures all entity-to-DTO mappings for the bike rental system.
    /// </summary>
    public BikeRentalProfile()
    { 
        // Bike Model mappings
        CreateMap<BikeModel, BikeModelDto>();
        CreateMap<BikeModelCreateUpdateDto, BikeModel>();
        
        // Bike mappings
        CreateMap<Bike, BikeDto>();
        CreateMap<BikeCreateUpdateDto, Bike>();
        
        // Rental mappings
        CreateMap<Rental, RentalDto>();
        CreateMap<RentalCreateUpdateDto, Rental>();
        
        // Renter mappings
        CreateMap<Renter, RenterDto>();
        CreateMap<RenterCreateUpdateDto, Renter>();
    }
}
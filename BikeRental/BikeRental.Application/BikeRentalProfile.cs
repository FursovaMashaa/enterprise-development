using AutoMapper;
using BikeRental.Application.Contracts.Bike;
using BikeRental.Application.Contracts.BikeModel;
using BikeRental.Application.Contracts.Rental;
using BikeRental.Application.Contracts.Renter;
using BikeRental.Domain.Models;

namespace BikeRental.Application;

public class BikeRentalProfile : Profile
{
    public BikeRentalProfile()
    { 
        // Bike Models
        CreateMap<BikeModel, BikeModelDto>();
        CreateMap<BikeModelCreateUpdateDto, BikeModel>();
        
        // Bikes
        CreateMap<Bike, BikeDto>();
        CreateMap<BikeCreateUpdateDto, Bike>();
        
        // Rents
        CreateMap<Rental, RentalDto>();
        CreateMap<RentalCreateUpdateDto, Rental>();
        
        // Renters
        CreateMap<Renter, RenterDto>();
        CreateMap<RenterCreateUpdateDto, Renter>();
    }
}
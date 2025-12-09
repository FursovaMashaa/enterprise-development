using BikeRental.Domain.Enums;
using BikeRental.Domain.Models;

namespace BikeRental.Domain.DataSeed;

/// <summary>
/// Provides test data for the BikeRental domain models using your class structure
/// </summary>
public class DataSeeder
{
    /// <summary>
    /// Initializes a new instance of the DataSeed class with test data
    /// </summary>
    public DataSeeder()
    {
        Models = InitializeBikeModels();
        Renters = InitializeRenters();
        Bikes = InitializeBikes();
        Rentals = InitializeRentals();
    }

    /// <summary>
    /// Collection of available bike models
    /// </summary>
    public List<BikeModel> Models { get; } = [];

    /// <summary>
    /// Collection of physical bikes
    /// </summary>
    public List<Bike> Bikes { get; } = [];

    /// <summary>
    /// Collection of customers who can rent bikes
    /// </summary>
    public List<Renter> Renters { get; } = [];

    /// <summary>
    /// Collection of rental transactions
    /// </summary>
    public List<Rental> Rentals { get; } = [];

    /// <summary>
    /// Initializes the list of bike models with specifications
    /// </summary>
    private static List<BikeModel> InitializeBikeModels()
    {
        return
        [
            new()
            {
                Id = 1,
                BikeType = BikeType.Mountain,
                WheelSize = 29.0,
                MaxPassengerWeight = 120,
                BikeWeight = 14.5,
                BrakeType = "Disc Hydraulic",
                ModelYear = 2024,
                PricePerHour = 8.50m
            },
            new()
            {
                Id = 2,
                BikeType = BikeType.Sport,
                WheelSize = 28.0,
                MaxPassengerWeight = 100,
                BikeWeight = 9.2,
                BrakeType = "Rim Brake",
                ModelYear = 2024,
                PricePerHour = 12.00m
            },
            new()
            {
                Id = 3,
                BikeType = BikeType.Road,
                WheelSize = 26.0,
                MaxPassengerWeight = 110,
                BikeWeight = 12.8,
                BrakeType = "Coaster Brake",
                ModelYear = 2023,
                PricePerHour = 5.50m
            },
            new()
            {
                Id = 4,
                BikeType = BikeType.Hybrid,
                WheelSize = 27.5,
                MaxPassengerWeight = 115,
                BikeWeight = 13.1,
                BrakeType = "Disc Mechanical",
                ModelYear = 2024,
                PricePerHour = 7.80m
            },
            new()
            {
                Id = 5,
                BikeType = BikeType.Sport,
                WheelSize = 28.0,
                MaxPassengerWeight = 105,
                BikeWeight = 8.9,
                BrakeType = "Disc Hydraulic",
                ModelYear = 2025,
                PricePerHour = 15.00m
            },
            new()
            {
                Id = 6,
                BikeType = BikeType.Mountain,
                WheelSize = 27.5,
                MaxPassengerWeight = 125,
                BikeWeight = 15.2,
                BrakeType = "Disc Hydraulic",
                ModelYear = 2023,
                PricePerHour = 9.20m
            },
            new()
            {
                Id = 7,
                BikeType = BikeType.Road,
                WheelSize = 28.0,
                MaxPassengerWeight = 95,
                BikeWeight = 10.5,
                BrakeType = "Rim Brake",
                ModelYear = 2024,
                PricePerHour = 6.80m
            },
            new()
            {
                Id = 8,
                BikeType = BikeType.Hybrid,
                WheelSize = 26.0,
                MaxPassengerWeight = 118,
                BikeWeight = 14.0,
                BrakeType = "Disc Mechanical",
                ModelYear = 2023,
                PricePerHour = 8.00m
            },
            new()
            {
                Id = 9,
                BikeType = BikeType.Sport,
                WheelSize = 29.0,
                MaxPassengerWeight = 108,
                BikeWeight = 9.8,
                BrakeType = "Disc Hydraulic",
                ModelYear = 2025,
                PricePerHour = 18.50m
            },
            new()
            {
                Id = 10,
                BikeType = BikeType.Road,
                WheelSize = 26.0,
                MaxPassengerWeight = 112,
                BikeWeight = 11.3,
                BrakeType = "Coaster Brake",
                ModelYear = 2022,
                PricePerHour = 4.90m
            }
        ];
    }

    /// <summary>   
    /// Initializes the list of physical bikes
    /// </summary>
    private List<Bike> InitializeBikes()
    {
        return
        [
            new()
            {
                Id = 1,
                SerialNumber = "MTN0012024",
                Color = "Forest Green",
                Model = Models[0]
            },
            new()
            {
                Id = 2,
                SerialNumber = "RD0022024",
                Color = "Racing Red",
                Model = Models[1]
            },
            new()
            {
                Id = 3,
                SerialNumber = "CTY0032023",
                Color = "Sky Blue",
                Model = Models[2]
            },
            new()
            {
                Id = 4,
                SerialNumber = "HYB0042024",
                Color = "Matte Black",
                Model = Models[3]
            },
            new()
            {
                Id = 5,
                SerialNumber = "SPT0052025",
                Color = "Carbon Gray",
                Model = Models[4]
            },
            new()
            {
                Id = 6,
                SerialNumber = "MTN0062023",
                Color = "Orange",
                Model = Models[5]
            },
            new()
            {
                Id = 7,
                SerialNumber = "RD0072024",
                Color = "Yellow",
                Model = Models[6]
            },
            new()
            {
                Id = 8,
                SerialNumber = "CTY0082023",
                Color = "White",
                Model = Models[7]
            },
            new()
            {
                Id = 9,
                SerialNumber = "HYB0092025",
                Color = "Purple",
                Model = Models[8]
            },
            new()
            {
                Id = 10,
                SerialNumber = "SPT0102022",
                Color = "Blue",
                Model = Models[9]
            }
        ];
    }

    /// <summary>
    /// Initializes the list of renters/customers
    /// </summary>
    private static List<Renter> InitializeRenters()
    {
        return
        [
            new()
            {
                Id = 1,
                LastName = "Ivanov",
                FirstName = "Alexey",
                MiddleName = "Petrovich",
                PhoneNumber = "+7 901 123-45-67"
            },
            new()
            {
                Id = 2,
                LastName = "Smirnova",
                FirstName = "Ekaterina",
                MiddleName = "Sergeevna",
                PhoneNumber = "+7 902 234-56-78"
            },
            new()
            {
                Id = 3,
                LastName = "Kuznetsov",
                FirstName = "Dmitry",
                MiddleName = "Viktorovich",
                PhoneNumber = "+7 903 345-67-89"
            },
            new()
            {
                Id = 4,
                LastName = "Popova",
                FirstName = "Anna",
                MiddleName = "Alexandrovna",
                PhoneNumber = "+7 904 456-78-90"
            },
            new()
            {
                Id = 5,
                LastName = "Sokolov",
                FirstName = "Mikhail",
                MiddleName = "Igorevich",
                PhoneNumber = "+7 905 567-89-01"
            },
            new()
            {
                Id = 6,
                LastName = "Fedorov",
                FirstName = "Sergey",
                MiddleName = "Nikolaevich",
                PhoneNumber = "+7 906 678-90-12"
            },
            new()
            {
                Id = 7,
                LastName = "Orlova",
                FirstName = "Olga",
                MiddleName = "Vladimirovna",
                PhoneNumber = "+7 907 789-01-23"
            },
            new()
            {
                Id = 8,
                LastName = "Lebedev",
                FirstName = "Andrey",
                MiddleName = "Borisovich",
                PhoneNumber = "+7 908 890-12-34"
            },
            new()
            {
                Id = 9,
                LastName = "Kozlova",
                FirstName = "Natalia",
                MiddleName = "Pavlovna",
                PhoneNumber = "+7 909 901-23-45"
            },
            new()
            {
                Id = 10,
                LastName = "Morozov",
                FirstName = "Vladimir",
                MiddleName = "Anatolievich",
                PhoneNumber = "+7 910 012-34-56"
            }
        ];
    }

    /// <summary>
    /// Initializes the list of rental transactions
    /// </summary>
    private List<Rental> InitializeRentals()
    {
        return
        [
            new()
            {
                Id = 1,
                StartTime = new DateTime(2024, 1, 10, 9, 0, 0),
                DurationHours = 3,
                Bike = Bikes[0],
                Renter = Renters[0]
            },
            new()
            {
                Id = 2,
                StartTime = new DateTime(2024, 1, 12, 14, 30, 0),
                DurationHours = 2,
                Bike = Bikes[1],
                Renter = Renters[1]
            },
            new()
            {
                Id = 3,
                StartTime = new DateTime(2024, 1, 15, 10, 0, 0),
                DurationHours = 5,
                Bike = Bikes[2],
                Renter = Renters[2]
            },
            new()
            {
                Id = 4,
                StartTime = new DateTime(2024, 1, 18, 16, 45, 0),
                DurationHours = 1,
                Bike = Bikes[3],
                Renter = Renters[3]
            },
            new()
            {
                Id = 5,
                StartTime = new DateTime(2024, 1, 20, 11, 0, 0),
                DurationHours = 4,
                Bike = Bikes[4],
                Renter = Renters[4]
            },
            new()
            {
                Id = 6,
                StartTime = new DateTime(2024, 1, 22, 13, 15, 0),
                DurationHours = 2,
                Bike = Bikes[0],
                Renter = Renters[1]
            },
            new()
            {
                Id = 7,
                StartTime = new DateTime(2024, 1, 25, 15, 30, 0),
                DurationHours = 3,
                Bike = Bikes[1],
                Renter = Renters[2]
            },
            new()
            {
                Id = 8,
                StartTime = new DateTime(2024, 1, 28, 8, 45, 0),
                DurationHours = 6,
                Bike = Bikes[2],
                Renter = Renters[0]
            },
            new()
            {
                Id = 9,
                StartTime = new DateTime(2024, 2, 1, 12, 0, 0),
                DurationHours = 2,
                Bike = Bikes[3],
                Renter = Renters[3]
            },
            new()
            {
                Id = 10,
                StartTime = new DateTime(2024, 2, 3, 17, 20, 0),
                DurationHours = 1,
                Bike = Bikes[4],
                Renter = Renters[4]
            },
            new()
            {
                Id = 11,
                StartTime = new DateTime(2024, 2, 5, 10, 30, 0),
                DurationHours = 4,
                Bike = Bikes[5],
                Renter = Renters[5]
            },
            new()
            {
                Id = 12,
                StartTime = new DateTime(2024, 2, 8, 14, 0, 0),
                DurationHours = 3,
                Bike = Bikes[6],
                Renter = Renters[6]
            },
            new()
            {
                Id = 13,
                StartTime = new DateTime(2024, 2, 12, 11, 15, 0),
                DurationHours = 2,
                Bike = Bikes[7],
                Renter = Renters[7]
            },
            new()
            {
                Id = 14,
                StartTime = new DateTime(2024, 2, 15, 16, 45, 0),
                DurationHours = 5,
                Bike = Bikes[8],
                Renter = Renters[8]
            },
            new()
            {
                Id = 15,
                StartTime = new DateTime(2024, 2, 18, 9, 30, 0),
                DurationHours = 1,
                Bike = Bikes[9],
                Renter = Renters[9]
            },
            new()
            {
                Id = 16,
                StartTime = new DateTime(2024, 2, 20, 13, 0, 0),
                DurationHours = 3,
                Bike = Bikes[5],
                Renter = Renters[0]
            },
            new()
            {
                Id = 17,
                StartTime = new DateTime(2024, 2, 22, 15, 20, 0),
                DurationHours = 2,
                Bike = Bikes[6],
                Renter = Renters[1]
            },
            new()
            {
                Id = 18,
                StartTime = new DateTime(2024, 2, 25, 10, 45, 0),
                DurationHours = 4,
                Bike = Bikes[7],
                Renter = Renters[2]
            },
            new()
            {
                Id = 19,
                StartTime = new DateTime(2024, 2, 28, 17, 30, 0),
                DurationHours = 1,
                Bike = Bikes[8],
                Renter = Renters[3]
            },
            new()
            {
                Id = 20,
                StartTime = new DateTime(2024, 3, 2, 12, 15, 0),
                DurationHours = 6,
                Bike = Bikes[9],
                Renter = Renters[4]
            }
        ];
    }
}
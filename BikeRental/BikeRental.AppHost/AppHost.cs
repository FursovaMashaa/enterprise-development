var builder = DistributedApplication.CreateBuilder(args);

// Add MongoDB container with database configuration
var mongo = builder.AddMongoDB("mongo").AddDatabase("bikerental");

builder.AddProject<Projects.BikeRental_Api>("bikerental-api")
       .WithReference(mongo)  
       .WaitFor(mongo);     

builder.Build().Run();
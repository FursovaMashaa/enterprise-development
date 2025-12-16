var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder.AddMongoDB("mongo").AddDatabase("bikerental");

builder.AddProject<Projects.BikeRental_Api>("bikerental-api")
       .WithReference(mongo)  
       .WaitFor(mongo);     

builder.Build().Run();
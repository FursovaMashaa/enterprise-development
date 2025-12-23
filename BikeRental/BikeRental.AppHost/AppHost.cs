var builder = DistributedApplication.CreateBuilder(args);

var natsUserName = builder.AddParameter("NatsLogin", secret: true);
var natsPassword = builder.AddParameter("NatsPassword", secret: true);
var natsStream = builder.AddParameter("NatsStream", "bikerental");
var natsSubject = builder.AddParameter("NatsSubject", "bikerental.events");

var mongo = builder.AddMongoDB("mongo").AddDatabase("bikerental");
var nats = builder.AddNats("bikerental-nats", 
        userName: natsUserName, 
        password: natsPassword, 
        port: 4222)
    .WithJetStream()
    .WithArgs("-m", "8222")
    .WithHttpEndpoint(port: 8222, targetPort: 8222);

builder.AddContainer("bikerental-nui", "ghcr.io/nats-nui/nui")
    .WithReference(nats)
    .WaitFor(nats)
    .WithHttpEndpoint(port: 31311, targetPort: 31311);

var generator = builder.AddProject<Projects.BikeRental_Generator_Nats_Host>("bikerental-generator-nats-host")
    .WithReference(nats)
    .WithEnvironment("Nats:StreamName", natsStream)
    .WithEnvironment("Nats:SubjectName", natsSubject)
    .WaitFor(nats);

var api = builder.AddProject<Projects.BikeRental_Api>("bikerental-api")
    .WithReference(mongo)
    .WithReference(nats)
    .WithEnvironment("Nats:StreamName", natsStream)
    .WithEnvironment("Nats:SubjectName", natsSubject)
    .WaitFor(mongo)
    .WaitFor(nats);

builder.Build().Run();
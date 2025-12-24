using BikeRental.Application.Contracts.Nats;
using BikeRental.Generator.Nats.Host;
using BikeRental.Generator.Nats.Host.Interfaces;
using BikeRental.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddOptions<NatsOptions>()
    .Bind(builder.Configuration.GetSection(NatsOptions.SectionName))
    .Validate(o => !string.IsNullOrWhiteSpace(o.StreamName), "Nats:StreamName is required")
    .Validate(o => !string.IsNullOrWhiteSpace(o.SubjectName), "Nats:SubjectName is required")
    .ValidateOnStart();
    
builder.AddServiceDefaults();

builder.AddNatsClient("bikerental-nats");
builder.Services.AddScoped<IProducerService, BikeRentalNatsProducer>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var assemblies = AppDomain.CurrentDomain.GetAssemblies()
    .Where(a => a.GetName().Name!.StartsWith("BikeRental"))
    .Distinct();

    foreach (var assembly in assemblies)
    {
        var xmlFile = $"{assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
            options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
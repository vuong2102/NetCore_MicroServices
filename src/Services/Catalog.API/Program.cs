using JasperFx;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddCarter();
builder.Services.AddMediatR(configuration => 
    configuration.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddMarten(options =>
{
    options.Connection(configuration.GetConnectionString("PostgreDatabase"));
    options.AutoCreateSchemaObjects = AutoCreate.All;
}).UseLightweightSessions();


var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();
app.Run();

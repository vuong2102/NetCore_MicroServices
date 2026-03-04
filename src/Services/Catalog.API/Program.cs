var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
var config = builder.Configuration;
var assembly = typeof(Program).Assembly;
var connectionString = config.GetConnectionString("Database")
    ?? throw new InvalidOperationException("Connection string 'Database' not found");

// Add services to the container.
builder.Services.AddCarter();
builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssembly(assembly);
    configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
    configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddMarten(options =>
{
    options.Connection(connectionString);
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
    builder.Services.InitializeMartenWith<CatalogInitialData>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks().AddNpgSql(connectionString);

builder.Services.AddScoped<IBasketRepository, BasketRepository>();



var app = builder.Build();  
  
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.MapCarter();

app.UseExceptionHandler(option => { });

app.UseHealthChecks("/health", 
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();



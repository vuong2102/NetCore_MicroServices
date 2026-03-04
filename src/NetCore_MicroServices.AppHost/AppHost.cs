var builder = DistributedApplication.CreateBuilder(args);

// 1. Add Postgres
var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin();

// 2. Add database
var catalogDb = postgres.AddDatabase("catalogdb");

// 3. Add Catalog API & link DB
builder.AddProject<Projects.Catalog_API>("catalog-api")
       .WithReference(catalogDb);

builder.AddProject<Projects.Basket_API>("basket-api");

builder.AddProject<Projects.Discount_Grpc>("discount-grpc");

builder.AddProject<Projects.Ordering_API>("ordering-api");

builder.AddProject<Projects.YarnApiGateways>("yarnapigateways");

builder.Build().Run();

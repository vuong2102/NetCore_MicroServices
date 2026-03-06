# NetCore MicroServices

A microservices-based e-commerce backend built with ASP.NET Core.  
The solution demonstrates API Gateway routing, service-to-service gRPC calls, Redis caching, containerized infrastructure, and domain-oriented service boundaries.

## Architecture

```text
Client
  |
  v
YARP API Gateway
  |----------------------> Catalog API (Product CRUD) ------------> PostgreSQL
  |----------------------> Basket API (Cart/Checkout) ------------> PostgreSQL + Redis
  |                                 |
  |                                 +---- gRPC ----> Discount.Grpc ----> SQL Server
  |
  +----------------------> Ordering API (in progress)
                                      ^
                                      |
                             RabbitMQ events (in progress)
```

## Tech Stack

- ASP.NET Core Web API (minimal APIs + Carter)
- gRPC
- RabbitMQ (MassTransit integration in progress)
- Redis
- SQL Server
- YARP API Gateway
- Docker / Docker Compose
- xUnit (testing strategy; test projects in progress)
- PostgreSQL (currently used by Catalog and Basket services)

## Features

- Product CRUD
  - Create, read, update, delete products in Catalog service.
- Basket management
  - Create basket, get basket, delete basket, checkout basket.
- Order management
  - Service boundary and gateway route are prepared; implementation is in progress.
- Inter-service communication
  - Basket service calls Discount service via gRPC.
- Caching
  - Basket data is cached with Redis.

## Services

| Service | Purpose | Protocol | Data Store |
|---|---|---|---|
| `Catalog.API` | Product catalog and CRUD operations | HTTP REST | PostgreSQL |
| `Basket.API` | Basket operations and checkout flow | HTTP REST + gRPC client | PostgreSQL + Redis |
| `Discount.Grpc` | Discount lookup and management | gRPC | SQL Server |
| `YarnApiGateways` | Single entry point and route forwarding | HTTP reverse proxy (YARP) | N/A |
| `Ordering` | Order lifecycle management | HTTP + async events (planned) | SQL Server (planned) |

## API Overview

### Catalog API

- `GET /products`
- `GET /products/{id}`
- `GET /products/category/{category}`
- `POST /products`
- `PUT /products`
- `DELETE /products/{id}`

### Basket API

- `GET /basket/{userName}`
- `POST /basket`
- `DELETE /basket/{userName}`
- `POST /basket/checkout`

### Discount gRPC

`DiscountProtoService` methods:

- `GetDiscount`
- `CreateDiscount`
- `UpdateDiscount`
- `DeleteDiscount`

## Project Structure

```text
src/
  ApiGateway/
    YarnApiGateways/
  BuildingBlocks/
  Services/
    Catalog.API/
    Basket.API/
    Discount/Discount.Grpc/
    Ordering/
  NetCore_MicroServices.AppHost/
  NetCore_MicroServices.ServiceDefaults/
  docker-compose.yml
  docker-compose.override.yml
```

## Getting Started

### Prerequisites

- .NET 10 SDK
- Docker Desktop

### Run with Docker Compose

```bash
cd src
docker compose up -d --build
```

Typical exposed ports from `docker-compose.override.yml`:

- Catalog API: `http://localhost:6000`
- Basket API: `http://localhost:6001`
- Discount gRPC: `https://localhost:6062`
- YARP Gateway: `http://localhost:6004`
- Redis: `localhost:6379`
- SQL Server (Discount): `localhost:1433`

### Run services locally (without full containerized app)

1. Start infrastructure dependencies:

```bash
cd src
docker compose up -d catalogdb basketdb discountdb distributedcache
```

2. Run each service in separate terminals:

```bash
dotnet run --project Services/Catalog.API
dotnet run --project Services/Basket.API
dotnet run --project Services/Discount/Discount.Grpc
dotnet run --project ApiGateway/YarnApiGateways
```

## Configuration

- Connection strings are defined per service in `appsettings.json`.
- Docker-specific overrides are in `docker-compose.override.yml`.
- Development credentials are currently hardcoded for local setup. Replace with secure secrets for real environments.

## Testing

This solution is designed to use `xUnit` for unit and integration testing.

Run tests with:

```bash
dotnet test
```

## Roadmap

- Complete `Ordering` service implementation.
- Enable RabbitMQ event publishing/consuming for checkout to order flow.
- Add dedicated `xUnit` test projects and CI validation.
- Expand observability and resilience policies across all services.

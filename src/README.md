Microservices E-Commerce System
1. Introduction

This project is a microservices-based e-commerce system built with .NET.
The system is designed using a distributed architecture where each service is responsible for a specific business domain.

The goal of this project is to practice building scalable backend systems using Microservices Architecture, API Gateway, Message Queue, and Containerization.

2. Architecture

The system includes the following services:

API Gateway (YARP)
Acts as the entry point for all client requests and routes them to the appropriate services.

Order Service
Handles order creation, cancellation, and order management.

Product Service
Manages product information such as creating, updating, and retrieving products.

Message Broker (RabbitMQ)
Enables asynchronous communication between services.

Cache (Redis)
Improves performance by caching frequently accessed data.

Database (SQL Server)
Stores system data for each service.

Services communicate through:

REST API

gRPC for high-performance service-to-service communication

RabbitMQ for event-driven messaging


3. Running the Project
    1. Clone repository
    git clone https://github.com/your-repo/microservice-project.git
    2. Run using Docker
    docker-compose up -d
    3. Access services

    API Gateway: http://localhost:5245
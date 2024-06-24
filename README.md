# GeniusExchangeConverter

## Overview

GeniusExchangeConverter is a .NET-based web API that provides currency conversion functionalities by interacting with the Open Exchange Rates API. The project demonstrates the use of several modern .NET technologies, libraries, and best practices including dependency injection, factory pattern, AutoMapper, Entity Framework Core, Refit, unit testing, and custom middleware.

### Technologies and Libraries Used

- **.NET 8**: The main framework for building the web API.
- **Entity Framework Core**: For database interactions using the code-first approach.
- **PostgreSQL**: The relational database used in the project.
- **AutoMapper**: For object-to-object mapping.
- **Refit**: For generating type-safe HTTP clients.
- **NUnit**: The testing framework for unit tests.
- **Moq**: For mocking dependencies in unit tests.
- **AutoFixture**: For generating test data in unit tests.
- **Swashbuckle (Swagger)**: For generating API documentation.
- **ASP.NET Core Middleware**: Custom middleware for exception handling.
- **InMemoryDatabase**: For integration testing with a real database.

### Getting Started

#### Prerequisites

- .NET 8 SDK
- PostgreSQL

### Setup

1. **Clone the repository**:

    ```bash
    git clone https://github.com/kerim878/GeniusExchangeConverter.git
    ```

2. **Navigate to the project directory**:

    ```bash
    cd GeniusExchangeConverter
    ```

3. **Set up the database**:
    - Ensure PostgreSQL is running.
    - Update the connection string in `appsettings.json` or `appsettings.Development.json`.

4. **Apply database migrations(migrations can be applied automatically)**:

    ```bash
    dotnet ef database update --project GeniusExchangeConverter.Infrastructure --startup-project GeniusExchangeConverter.Api
    ```

5. **Run the application**:

    ```bash
    dotnet run --project GeniusExchangeConverter.Api
    ```

6. **Run the tests**:

    ```bash
    dotnet test
    ```

### Project Workflow

#### Testing

The project uses NUnit, Moq, and AutoFixture for unit testing. Tests are located in the `GeniusExchangeConverter.{layer}.Tests` project. Key components covered by tests include:

- **ExchangeConversionRepository**: Tests for saving and retrieving conversion logs.
- **OpenExchangeRateClient**: Tests for interacting with the Open Exchange Rates API, including handling cancellation and exceptions.

#### Error Handling

Custom error handling middleware is implemented to provide consistent error responses using `ProblemDetails`. This middleware catches exceptions thrown during request processing and formats the response according to the RFC 7807 standard.

#### API Documentation

The project uses Swashbuckle to generate Swagger documentation for the API. This documentation is available at `/swagger` when the application is running.

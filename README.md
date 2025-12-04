# Modular Monolith Template

## Project Description

This project is a modern, well-structured template for building maintainable Modular Monolith applications in .NET. It demonstrates how to organize code into separate modules that are loosely coupled yet deployed as a single application. The template shows proper domain modeling, clean separation of concerns, and smart handling of cross-module communication - all without overwhelming complexity. For a comprehensive introduction, see [Building a Modular Monolith with Vertical Slice Architecture in .NET](https://antondevtips.com/blog/building-a-modular-monolith-with-vertical-slice-architecture-in-dotnet).

## Modules Description

The application consists of 4 main modules:

- **Users Module**: Handles user authentication and authorization using JWT tokens
- **Shipments Module**: Manages the full shipment lifecycle from creation to delivery
- **Carriers Module**: Manages shipping carriers/companies that deliver packages
- **Stocks Module**: Tracks product inventory and availability

**Common Packages:** Contains shared libraries, utilities, and interfaces used across modules

## Module Communication

Modules are designed with clear boundaries and specific communication paths:

- **Shipments Module** calls the Carriers and Stocks modules to verify carriers exist and check product availability
- **Authentication Flow**: Shipments, Carriers, and Stocks modules require users to be authenticated but don't directly call the Users module. Instead, the user obtains a JWT and refresh token from the Users module which is used to authorize requests to other modules. Learn more about the implementation in [How to Implement Refresh Tokens and Token Revocation in ASP.NET Core](https://antondevtips.com/blog/how-to-implement-refresh-tokens-and-token-revocation-in-aspnetcore) and [Authentication and Authorization Best Practices in ASP.NET Core](https://antondevtips.com/blog/authentication-and-authorization-best-practices-in-aspnetcore)

## NuGet Packages

- **FluentValidation**: Provides a fluent interface for building strongly-typed validation rules
- **Entity Framework Core**: ORM for database access with clean separation of persistence concerns
- **ASP.NET Core**: Web framework for building REST APIs (Minimal APIs are being used)
- **OpenTelemetry**: Collects metrics, logs, and traces for observability ([Getting Started with OpenTelemetry in .NET with Jaeger and Seq](https://antondevtips.com/blog/getting-started-with-open-telemetry-in-dotnet-with-jaeger-and-seq))
- **Swagger/OpenAPI**: Generates API documentation with a testable UI

## Docker Setup

The application uses Docker Compose to run all required services:

1. **shipping-modular-monolith**: Main application container built from the ./ModularMonolith.Host/Dockerfile
2. **postgres**: PostgreSQL database for storing all module data
3. **seq**: Structured logging platform for centralized log collection and analysis
4. **jaeger**: Distributed tracing system for tracking requests across modules

### Running the Project

1. Ensure Docker and Docker Compose are installed
2. Navigate to the project root directory
3. Run `docker-compose up -d`
4. Access the API at http://localhost:5000/swagger
5. Access Seq dashboard at http://localhost:8081
6. Access Jaeger UI at http://localhost:16686

The configuration includes:
- OpenTelemetry export to Jaeger for distributed tracing
- PostgreSQL with persistent volume mounted at ./docker_data/pgdata
- Seq with persistent volume mounted at ./docker_data/seq
- All services connected via a bridge network named docker-web

## Project Structure

- **/src**: Main source code directory
  - **/ModularMonolith.Host**: Entry point for the application that configures and runs all modules
  - **/Common**: Shared abstractions, extensions, and utilities
  - **/Users**: Authentication and user management
  - **/Carriers**: Shipping carrier management 
  - **/Stocks**: Inventory management
  - **/Shipments**: Shipment processing and tracking

Each module follows a consistent structure with dedicated projects for:
- **Domain**: Core business logic, entities, and rules
- **Features**: Application use cases organized as vertical slices
- **Infrastructure**: External dependencies and persistence
- **Tests.Unit**: Unit tests for isolated components
- **Tests.Integration**: Integration tests for complete features

## API Endpoints

### Users Module
- `POST /api/users/register`: Register a new user
- `POST /api/users/login`: Authenticate and receive JWT token

### Carriers Module
- `POST /api/carriers`: Create a new carrier
- `GET /api/carriers/{name}`: Get carrier by name

### Stocks Module
- `POST /api/stocks`: Create a new stock item
- `GET /api/stocks/{productName}`: Get stock by product name

### Shipments Module
- `POST /api/shipments`: Create a new shipment
- `GET /api/shipments/{number}`: Get shipment by number
- `POST /api/shipments/process/{number}`: Update shipment to processing status
- `POST /api/shipments/dispatch/{number}`: Update shipment to dispatched status
- `POST /api/shipments/transit/{number}`: Update shipment to in-transit status
- `POST /api/shipments/deliver/{number}`: Update shipment to delivered status
- `POST /api/shipments/receive/{number}`: Update shipment to received status
- `POST /api/shipments/cancel/{number}`: Cancel a shipment

## Tests

The project includes comprehensive tests:

- **Unit Tests**: Test individual components in isolation with mocked dependencies
  - Test specific business logic in domain entities
  - Test use case handlers with in-memory database
  - Validate business rules and constraints

  - **Integration Tests**: Test complete features from HTTP request to database ([ASP.NET Core Integration Testing Best Practices](https://antondevtips.com/blog/asp-net-core-integration-testing-best-practises))
  - Verify API endpoints with real HTTP calls
  - Test happy paths and error scenarios
  - Validate error responses and status codes

### Testing Projects

- **Modules.Common.Tests.Architecture**: Enforces architectural constraints using NetArchTest
- **Modules.Common.Result.Tests.Unit**: Tests the custom Result pattern implementation
- **Modules.Shipments.Tests.Unit**: Unit tests for the Shipments module's domain logic and handlers
- **Modules.Shipments.Tests.Integration**: End-to-end tests for Shipments module API endpoints

### Testing Packages

- **xUnit**: Primary testing framework with extensible test runners
- **NSubstitute**: Mocking library for creating test doubles
- **Microsoft.EntityFrameworkCore.InMemory**: In-memory database for unit testing
- **Microsoft.AspNetCore.Mvc.Testing**: WebApplicationFactory for integration testing
- **Respawn**: Database cleaner for resetting between tests
- **Testcontainers**: Provides Docker containers for integration testing with PostgreSQL
- **NetArchTest.Rules**: Enforces architectural rules and boundaries

## Seeding & Migrations

The application includes development seeding to populate test data:

- **UserSeedService**: Creates default users for testing (admin@test.com/Test1234!)
- **SeedService**: Creates sample shipments, stocks and carriers

Migrations run automatically in development mode with `context.Database.Migrate()` ensuring the database schema is up-to-date when the application starts.

## Architecture

### Vertical Slices with Clean Architecture

The project uses a combination of Vertical Slices and Clean Architecture ([The Best Way to Structure Your .NET Projects with Clean Architecture and Vertical Slices](https://antondevtips.com/blog/the-best-way-to-structure-your-dotnet-projects-with-clean-architecture-and-vertical-slices)):

- **Vertical Slices**: Features are organized by business functionality rather than technical layers ([Vertical Slice Architecture: The Best Ways to Structure Your Project](https://antondevtips.com/blog/vertical-slice-architecture-the-best-ways-to-structure-your-project))
- **Clean Architecture**: Core domain is isolated from infrastructure concerns

### Key Architecture Decisions

- **Manual Handlers**: Simple handler pattern used instead of MediatR to reduce unnecessary abstractions
- **Direct EF Core Usage**: Entity Framework Core is used directly in use cases without Repository and Unit of Work patterns
  - Features: Simpler code, better query optimization, natural transaction boundaries
  - EF Core already provides a unit of work and repository pattern

- **Manual Mapping**: Direct mapping between DTOs and domain objects for simplicity and control
- **Result Pattern**: Custom implementation that returns success or detailed errors without exceptions ([How to Replace Exceptions with Result Pattern in .NET](https://antondevtips.com/blog/how-to-replace-exceptions-with-result-pattern-in-dotnet))
- **Fluent Validation**: Provides clear, strongly-typed validation rules for all inputs

### Observability

- **OpenTelemetry (OTEL)**: Instrument code for metrics, logs, and traces
- **Seq**: Centralized structured logging ([Logging Best Practices in ASP.NET Core](https://antondevtips.com/blog/logging-best-practices-in-asp-net-core))
- **Jaeger**: Visualize request flows across modules ([How to Implement Structured Logging and Distributed Tracing for Microservices with Seq](https://antondevtips.com/blog/how-to-implement-structured-logging-and-distributed-tracing-for-microservices-with-seq))

## Development Configuration

The project includes several key configuration files that standardize development practices ([Best Practices for Increasing Code Quality in .NET Projects](https://antondevtips.com/blog/best-practices-for-increasing-code-quality-in-dotnet-projects)):

- **.editorconfig**: Enforces consistent coding styles across different editors and IDEs
  - Sets tab/space preferences, line endings, and trailing whitespace rules
  - Configures C# code style and formatting rules
  - Customizes code analyzer severity levels

- **Directory.Build.props**: Centralizes common build properties for all projects
  - Enables nullable reference types, implicit usings, and warning-as-error settings
  - Adds code analyzers (Meziantou, SonarAnalyzer, Roslynator) to improve code quality
  - Enforces code style rules during build

- **Directory.Packages.props**: Centralizes NuGet package version management
  - Ensures consistent package versions across all projects
  - Simplifies updates by changing versions in a single location

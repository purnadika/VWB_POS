# 🎯 VWB_POS Implementation Plan: Foundation Phase

## Understanding
You've approved a sophisticated DDD + CQRS architecture with zero magic strings, multi-language/currency support, and AI infrastructure from day one. This plan breaks down the **Foundation Phase** (User Management module) into atomic, executable steps that establish the patterns and infrastructure all subsequent modules will follow.

## Assumptions
- PostgreSQL 15 is available (or will be spun up via Docker)
- EF Core 10 migrations will drive schema creation
- MediatR pipeline with FluentValidation is the CQRS backbone
- Localization keys are constants (no runtime lookups for key names)
- Money value object is immutable and enforces currency correctness
- Domain events are published but not persisted to a separate event store (EventSourcing phase 2)
- Tests use xUnit + Moq for unit tests, TestContainers for integration tests
- Each feature folder follows strict folder/namespace conventions for discoverability

## Approach
We'll build the User Management module end-to-end, establishing patterns that scale to all future modules:

1. **Infrastructure Setup** (DI, migrations, EF Core config, seeding) — foundation for all data access
2. **Domain Layer** (User aggregate, value objects, domain events, repository interfaces) — pure domain logic, no dependencies
3. **Application Layer** (Commands, Queries, Handlers, DTOs, Validators, Localization service) — orchestration and cross-cutting concerns
4. **API Layer** (Controllers, error handling, OpenAPI) — HTTP contracts
5. **Testing** (Unit → Integration → E2E) — verify the entire flow works

This order ensures dependencies flow downward: API depends on Application, Application depends on Domain, Infrastructure implements Domain interfaces.

## Key Files
- `src/POS.Domain/Aggregates/UserManagement/User.cs` — aggregate root, encapsulates business rules
- `src/POS.Application/Features/UserManagement/Commands/CreateUserCommand.cs` — CQRS command entry point
- `src/POS.Infrastructure/Persistence/Repositories/UserRepository.cs` — implements IUserRepository
- `src/POS.WebAPI/Controllers/UserManagementController.cs` — HTTP endpoints
- `tests/POS.Domain.Tests/Features/UserManagement/UserAggregateTests.cs` — unit tests for domain logic
- `tests/POS.Integration.Tests/Features/UserManagement/UserManagementIntegrationTests.cs` — integration tests with real DB

## Risks & Open Questions
- **EF Core JSON columns for value objects**: Will use `ComplexProperty` in EF Core 10; verify schema generation
- **Password hashing**: BCrypt.Net-Next dependency needs to be added; confirm compatibility with .NET 10
- **Localization file location**: Starting with JSON files in `src/POS.Infrastructure/Resources/i18n/`; can migrate to DB later
- **Docker PostgreSQL for local dev**: Will use TestContainers for integration tests; local dev can use Docker Compose
- **AI event publishing**: Events are published to MediatR INotificationHandler; AI consumer wired but not tested until POS.AI integration
- **Roles & Permissions design**: Keeping simple for MVP (Enum-based roles); RBAC policies can be added later if needed

**Progress**: 16% [█░░░░░░░░░]

**Last Updated**: 2026-07-26 16.17.45

## 📝 Plan Steps
- 🔄 **Update project files and add NuGet dependencies**
- ✅ **Create core infrastructure: DbContext, EF Core configuration, migrations**
- ✅ **Create Domain layer: User aggregate, value objects, domain events**
-  **Create Infrastructure layer: Repository implementations, services**
-  **Create Application layer: Commands, Queries, Handlers, DTOs**
-  **Create API layer: Controllers, error handling, OpenAPI**
-  **Create comprehensive unit tests for User aggregate**
-  **Create integration tests with PostgreSQL Docker**
-  **Create E2E tests with Playwright**
-  **Wire AI infrastructure: Event consumers, tool definitions**
-  **Update documentation and commit**
-  **Verify build, tests pass, and project is ready for next module**


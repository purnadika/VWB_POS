# VWB_POS: DDD Modular Monolith with AI Infrastructure
## Architecture & Design Specification

**Document Date:** 2025-01-10  
**Status:** Design Approval Phase  
**Target Framework:** .NET 10  
**Architecture Pattern:** Domain-Driven Design (DDD) + CQRS + Domain Events  
**Deployment Model:** Modular Monolith (path to microservices)  
**Database:** PostgreSQL (shared, schema-isolated)  
**Testing:** Comprehensive Pyramid (Unit + Integration + E2E with Playwright)

---

## 1. Executive Summary

VWB_POS is a port of OpenSourcePOS (PHP) to modern .NET 10, built with:
- **Strict DDD principles** with aggregates, value objects, domain services
- **CQRS pattern** via MediatR for clear separation of concerns
- **Domain Events** for audit trail and AI agent consumption
- **Modular architecture** (separate feature .csproj files) enabling future microservices migration
- **AI infrastructure** from day one using .NET 10 Semantic Kernel
- **Zero magic strings**: All constants, enums, and translatable strings via centralized i18n system
- **Multi-currency & multi-language support** built into core architecture

---

## 2. Architectural Layers

### 2.1 Layer Diagram

```
┌─────────────────────────────────────────────────────────┐
│           API Layer (POS.WebAPI)                         │
│     Controllers, OpenAPI, Error Handling                │
└──────────────────┬──────────────────────────────────────┘
				   │
┌──────────────────▼──────────────────────────────────────┐
│      Application Layer (POS.Application.*)              │
│  MediatR Commands/Queries, Handlers, DTOs, Behaviors   │
│  Validation, Mapping, Cross-Cutting Concerns           │
└──────────────────┬──────────────────────────────────────┘
				   │
┌──────────────────▼──────────────────────────────────────┐
│         Domain Layer (POS.Domain)                        │
│  Aggregates, Entities, Value Objects, Domain Services   │
│  Domain Events, Repository Interfaces, Business Rules   │
└──────────────────┬──────────────────────────────────────┘
				   │
┌──────────────────▼──────────────────────────────────────┐
│      Infrastructure Layer (POS.Infrastructure)          │
│  EF Core, PostgreSQL, Repositories, Event Publishing    │
│  Logging, External Services, Configuration              │
└──────────────────┬──────────────────────────────────────┘
				   │
┌──────────────────▼──────────────────────────────────────┐
│    AI Infrastructure (POS.AI)                            │
│  Semantic Kernel, Agent Framework, Tool Definitions     │
│  Event Consumers, Audit Logging, AI-Accessible Queries  │
└─────────────────────────────────────────────────────────┘
```

### 2.2 Layer Responsibilities

| Layer | Responsibility | SOLID Focus |
|-------|---------------|----|
| **API** | HTTP contracts, routing, response formatting | Single Responsibility (endpoints do one thing) |
| **Application** | Orchestration, validation, DTOs, cross-cutting behaviors | Interface Segregation (small, focused handlers) |
| **Domain** | Business logic, invariants, aggregates, domain services | Encapsulation, business rules enforcement |
| **Infrastructure** | Persistence, external APIs, technical concerns | Dependency Inversion (repositories abstract data source) |
| **AI** | Agent reasoning, tool execution, semantic understanding | Open/Closed (extend via domain events, not modification) |

---

## 3. Modular Monolith Structure

### 3.1 Project Layout

```
src/
├── POS.Domain/                          # Core domain logic (no dependencies except System)
│   ├── Common/
│   │   ├── BaseEntity.cs                # Aggregate root base
│   │   ├── DomainEvent.cs               # Domain event base
│   │   └── Result.cs                    # Result<T> pattern
│   ├── Enums/                           # Domain constants (no magic strings)
│   │   ├── UserRole.cs
│   │   ├── PermissionType.cs
│   │   ├── ProductAttributeType.cs
│   │   ├── PaymentMethod.cs
│   │   ├── InventoryMovementType.cs
│   │   ├── SaleStatus.cs
│   │   ├── CurrencyCode.cs              # ISO 4217 codes
│   │   └── LanguageCode.cs              # ISO 639-1 codes
│   ├── ValueObjects/
│   │   ├── Money.cs                     # Currency + Amount (multi-currency)
│   │   ├── Email.cs
│   │   ├── PhoneNumber.cs
│   │   ├── Address.cs
│   │   ├── Barcode.cs
│   │   └── Percentage.cs
│   ├── Interfaces/
│   │   ├── Repositories/
│   │   │   ├── IRepository<T>.cs
│   │   │   ├── IUnitOfWork.cs
│   │   │   └── [Module-specific repositories]
│   │   └── Services/
│   │       ├── ITaxCalculationService.cs
│   │       └── [Module-specific services]
│   └── Aggregates/                      # Each module's aggregate root
│       ├── UserManagement/
│       │   ├── User.cs                  # Aggregate Root
│       │   ├── Role.cs                  # Entity
│       │   ├── Permission.cs            # Entity
│       │   └── Events/
│       │       ├── UserCreatedEvent.cs
│       │       ├── RoleAssignedEvent.cs
│       │       └── PermissionGrantedEvent.cs
│       ├── ProductCatalog/
│       │   ├── Product.cs               # Aggregate Root
│       │   ├── ProductCategory.cs       # Entity
│       │   ├── ProductAttribute.cs      # Entity
│       │   └── Events/
│       │       ├── ProductCreatedEvent.cs
│       │       ├── PriceChangedEvent.cs
│       │       └── StockUpdatedEvent.cs
│       └── [...other modules]

├── POS.Application/                     # Application services, CQRS
│   ├── Common/
│   │   ├── Behaviors/
│   │   │   ├── ValidationBehavior.cs
│   │   │   ├── LoggingBehavior.cs
│   │   │   └── PerformanceBehavior.cs
│   │   ├── Exceptions/
│   │   │   ├── ValidationException.cs
│   │   │   └── NotFoundException.cs
│   │   └── Models/
│   │       └── PagedList.cs
│   ├── Features/                        # Each feature is a module
│   │   ├── UserManagement/
│   │   │   ├── Commands/
│   │   │   │   ├── CreateUserCommand.cs
│   │   │   │   ├── AssignRoleCommand.cs
│   │   │   │   └── [other commands]
│   │   │   ├── Queries/
│   │   │   │   ├── GetUserQuery.cs
│   │   │   │   └── ListUsersQuery.cs
│   │   │   ├── Handlers/
│   │   │   │   ├── CreateUserCommandHandler.cs
│   │   │   │   └── [other handlers]
│   │   │   ├── Events/
│   │   │   │   └── [Domain event consumers]
│   │   │   └── DTOs/
│   │   │       ├── CreateUserRequest.cs
│   │   │       └── UserResponse.cs
│   │   ├── ProductCatalog/
│   │   │   ├── Commands/
│   │   │   ├── Queries/
│   │   │   ├── Handlers/
│   │   │   ├── Events/
│   │   │   └── DTOs/
│   │   └── [...other modules]
│   ├── Constants/                       # Application-level constants (no magic strings)
│   │   ├── LocalizationKeys.cs          # All i18n keys
│   │   ├── ErrorMessages.cs             # Error message keys
│   │   ├── ValidationMessages.cs        # Validation keys
│   │   └── DefaultValues.cs             # Default settings
│   ├── Localization/
│   │   ├── ILocalizationService.cs      # Abstraction
│   │   └── LocalizationService.cs       # Implementation
│   ├── MultiCurrency/
│   │   ├── ICurrencyService.cs          # Abstraction
│   │   └── CurrencyService.cs           # Implementation
│   └── DependencyInjection.cs           # MediatR, FluentValidation registration

├── POS.Infrastructure/                  # Technical implementation
│   ├── Persistence/
│   │   ├── Contexts/
│   │   │   └── ApplicationDbContext.cs  # EF Core DbContext
│   │   ├── Repositories/
│   │   │   ├── Repository.cs            # Generic repository
│   │   │   ├── UserRepository.cs
│   │   │   └── [Module-specific repositories]
│   │   ├── Migrations/
│   │   │   └── [EF Core migrations]
│   │   └── Seeds/
│   │       └── InitialDataSeeder.cs     # Seed base currencies, languages
│   ├── Services/
│   │   ├── TaxCalculationService.cs
│   │   ├── LocalizationService.cs
│   │   ├── CurrencyService.cs
│   │   └── [Other implementations]
│   ├── Events/
│   │   ├── DomainEventPublisher.cs
│   │   └── EventHandlers/
│   │       └── [Domain event consumers]
│   ├── Logging/
│   │   ├── LoggingBehavior.cs
│   │   └── AuditTrail.cs
│   ├── Configuration/
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json
│   │   └── [Environment-specific]
│   └── DependencyInjection.cs

├── POS.AI/                              # AI agent infrastructure
│   ├── Agents/
│   │   ├── POSAssistantAgent.cs         # Main agent orchestrator
│   │   └── AgentContext.cs              # Agent state and reasoning
│   ├── Tools/
│   │   ├── IAgentTool.cs                # Base tool interface
│   │   ├── UserManagementTools.cs       # User-related queries/actions
│   │   ├── ProductCatalogTools.cs       # Product-related queries/actions
│   │   └── [...module-specific tools]
│   ├── EventConsumers/
│   │   └── AIDomainEventConsumer.cs     # Capture events for AI reasoning
│   ├── Semantic/
│   │   ├── SemanticQueries.cs           # AI-friendly query structure
│   │   └── ReasoningContext.cs
│   ├── Prompts/
│   │   ├── SystemPrompt.cs
│   │   └── [...task-specific prompts]
│   └── DependencyInjection.cs

├── POS.WebAPI/                          # ASP.NET Core host
│   ├── Controllers/
│   │   ├── UserManagementController.cs
│   │   ├── ProductCatalogController.cs
│   │   └── [...module-specific controllers]
│   ├── Middleware/
│   │   ├── ErrorHandlingMiddleware.cs
│   │   ├── LocalizationMiddleware.cs
│   │   └── AuditLoggingMiddleware.cs
│   ├── OpenAPI/
│   │   └── OpenApiConfiguration.cs
│   ├── Program.cs                       # DI Container setup
│   └── appsettings.json

tests/
├── POS.Domain.Tests/
│   └── Features/
│       ├── UserManagement/
│       │   ├── UserAggregateTests.cs
│       │   └── PermissionTests.cs
│       ├── ProductCatalog/
│       │   └── ProductAggregateTests.cs
│       └── Common/
│           ├── ValueObjectTests.cs
│           └── MoneyTests.cs
├── POS.Application.Tests/
│   └── Features/
│       ├── UserManagement/
│       │   ├── CreateUserCommandHandlerTests.cs
│       │   └── AssignRoleCommandHandlerTests.cs
│       ├── ProductCatalog/
│       │   └── [Handler tests]
│       └── Common/
│           ├── LocalizationServiceTests.cs
│           └── CurrencyServiceTests.cs
├── POS.Integration.Tests/              # NEW: Integration tests with Docker PostgreSQL
│   └── Features/
│       ├── UserManagement/
│       │   └── UserManagementIntegrationTests.cs
│       ├── ProductCatalog/
│       │   └── ProductCatalogIntegrationTests.cs
│       └── Common/
│           └── DatabaseFixture.cs      # PostgreSQL Docker container
└── POS.UI.Tests/
	└── Features/
		├── UserManagement/
		│   └── UserManagementE2ETests.cs
		└── ProductCatalog/
			└── ProductCatalogE2ETests.cs
```

---

## 4. Module Definition (Modular Monolith)

### 4.1 Module Structure

Each module is **one .csproj** containing:
- Domain aggregates (in POS.Domain)
- Application commands/queries (in POS.Application.{ModuleName})
- Infrastructure repositories (in POS.Infrastructure)
- API controllers (in POS.WebAPI)
- Comprehensive tests (in test projects)

**Example: UserManagement Module**

```
POS.Domain/Aggregates/UserManagement/
├── User.cs (Aggregate Root)
├── Role.cs (Entity)
├── Permission.cs (Entity)
└── Events/
	├── UserCreatedEvent.cs
	├── RoleAssignedEvent.cs
	└── PermissionGrantedEvent.cs

POS.Application/Features/UserManagement/
├── Commands/
│   ├── CreateUserCommand.cs
│   └── AssignRoleCommand.cs
├── Queries/
│   ├── GetUserQuery.cs
│   └── ListUsersQuery.cs
├── Handlers/
│   ├── CreateUserCommandHandler.cs
│   └── GetUserQueryHandler.cs
├── Events/
│   └── UserCreatedEventHandler.cs
└── DTOs/
	├── CreateUserRequest.cs
	└── UserResponse.cs

POS.WebAPI/Controllers/
└── UserManagementController.cs

tests/
├── POS.Domain.Tests/Features/UserManagement/
├── POS.Application.Tests/Features/UserManagement/
└── POS.Integration.Tests/Features/UserManagement/
```

### 4.2 Module Dependencies

**Allowed Dependencies** (Dependency Injection rules):
```
POS.WebAPI
  ↓ depends on
POS.Application.{ModuleName} (via IServiceCollection.AddApplicationServices)
POS.AI (via IServiceCollection.AddAIServices)
  ↓ depends on
POS.Domain (interfaces only, no implementations)
  ↓ depends on
System.* (only .NET standard libraries)

POS.Infrastructure
  ↓ depends on
POS.Domain (implementations of repository interfaces)
  ↓ depends on
EntityFrameworkCore, PostgreSQL drivers

```

**NO circular dependencies. NO cross-module command calls.**

---

## 5. Multi-Language & Multi-Currency Architecture

### 5.1 Localization Strategy

**Zero Magic Strings Principle:**

Every user-facing string must be:
1. A constant key in `LocalizationKeys.cs`
2. Looked up via `ILocalizationService`
3. Stored in translation database or JSON files

**Implementation:**

```csharp
// LocalizationKeys.cs (Constants, no magic strings!)
namespace POS.Application.Constants
{
	public static class LocalizationKeys
	{
		// UserManagement
		public const string UserCreated = "user.created";
		public const string UserNotFound = "user.not.found";
		public const string InvalidEmailFormat = "user.invalid.email";

		// ProductCatalog
		public const string ProductCreated = "product.created";
		public const string ProductNotFound = "product.not.found";
		public const string LowStockWarning = "product.low.stock";

		// Generic
		public const string OperationSuccessful = "operation.successful";
		public const string ValidationFailed = "validation.failed";
	}
}

// ILocalizationService - abstraction
public interface ILocalizationService
{
	string GetString(string key, string locale = null);
	string GetString(string key, Dictionary<string, object> parameters, string locale = null);
	IEnumerable<string> GetAvailableLanguages();
}

// Usage in Application Layer
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserResponse>>
{
	private readonly ILocalizationService _localizationService;

	public async Task<Result<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
	{
		// Business logic...

		// Success message uses localized key
		var successMessage = _localizationService.GetString(
			LocalizationKeys.UserCreated, 
			locale: request.PreferredLanguage
		);

		return Result<UserResponse>.Success(userData, successMessage);
	}
}

// Usage in API
[HttpGet("{id}")]
public async Task<IActionResult> GetUser(Guid id, [FromHeader(Name = "Accept-Language")] string locale = "en")
{
	var query = new GetUserQuery { UserId = id, Locale = locale };
	var result = await _mediator.Send(query);

	return Ok(new { message = result.Message, data = result.Data });
}
```

**Translation Storage:**

Option A: **Database-backed** (for runtime updates)
```sql
CREATE TABLE Translations (
	Id UUID PRIMARY KEY,
	Language VARCHAR(10) NOT NULL,  -- "en", "fr", "es", etc.
	Key VARCHAR(255) NOT NULL,
	Value TEXT NOT NULL,
	UNIQUE(Language, Key)
);
```

Option B: **JSON files** (for fast startup, version control)
```
src/POS.Infrastructure/Resources/
├── i18n/
│   ├── en.json
│   ├── fr.json
│   ├── es.json
│   └── de.json
```

**Recommendation:** Start with JSON files (simpler, versioned), migrate to database if real-time updates needed.

### 5.2 Multi-Currency Strategy

**Money Value Object** (enforces currency):

```csharp
public class Money : ValueObject
{
	public decimal Amount { get; }
	public string CurrencyCode { get; }  // ISO 4217 (USD, EUR, GBP, etc.)

	public Money(decimal amount, string currencyCode)
	{
		if (!CurrencyService.IsValidCurrency(currencyCode))
			throw new DomainException($"Invalid currency: {currencyCode}");

		Amount = amount;
		CurrencyCode = currencyCode;
	}

	public static Money operator +(Money left, Money right)
	{
		if (left.CurrencyCode != right.CurrencyCode)
			throw new DomainException("Cannot add different currencies");

		return new Money(left.Amount + right.Amount, left.CurrencyCode);
	}
}

// Usage in Domain
public class Product
{
	public Money Price { get; private set; }  // Stores currency + amount

	public void UpdatePrice(decimal newAmount, string currencyCode)
	{
		var newPrice = new Money(newAmount, currencyCode);
		// Business logic...
		Price = newPrice;
		PublishEvent(new PriceChangedEvent(this.Id, newPrice));
	}
}
```

**Currency Service** (exchange rates, conversions):

```csharp
public interface ICurrencyService
{
	IEnumerable<(string Code, string Name)> GetAvailableCurrencies();
	Money Convert(Money amount, string targetCurrency);
	decimal GetExchangeRate(string fromCurrency, string toCurrency);
}

// Usage in Application Layer
public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, Result<SaleResponse>>
{
	private readonly ICurrencyService _currencyService;

	public async Task<Result<SaleResponse>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
	{
		var saleTotal = new Money(request.Total, request.CurrencyCode);

		// Business logic with currency awareness
		var tax = _taxCalculationService.CalculateTax(saleTotal);
		var finalAmount = saleTotal + tax;

		// ... persistence

		return Result<SaleResponse>.Success(saleData);
	}
}
```

**Database Schema** (currency and language configuration):

```sql
CREATE TABLE Currencies (
	Code VARCHAR(3) PRIMARY KEY,
	Name VARCHAR(100) NOT NULL,
	Symbol VARCHAR(10) NOT NULL,
	IsActive BOOLEAN NOT NULL DEFAULT TRUE
);

CREATE TABLE Languages (
	Code VARCHAR(5) PRIMARY KEY,
	Name VARCHAR(100) NOT NULL,
	NativeName VARCHAR(100),
	IsActive BOOLEAN NOT NULL DEFAULT TRUE
);

-- Seed with ISO standards
INSERT INTO Currencies VALUES ('USD', 'United States Dollar', '$', TRUE);
INSERT INTO Currencies VALUES ('EUR', 'Euro', '€', TRUE);
INSERT INTO Currencies VALUES ('GBP', 'British Pound', '£', TRUE);

INSERT INTO Languages VALUES ('en', 'English', 'English', TRUE);
INSERT INTO Languages VALUES ('fr', 'French', 'Français', TRUE);
INSERT INTO Languages VALUES ('es', 'Spanish', 'Español', TRUE);
```

---

## 6. Enums & Constants (No Magic Strings)

### 6.1 Domain Enums

```csharp
// POS.Domain/Enums/UserRole.cs
public enum UserRole
{
	Admin = 1,
	Manager = 2,
	Cashier = 3,
	Warehouse = 4,
	Supervisor = 5
}

// POS.Domain/Enums/PermissionType.cs
public enum PermissionType
{
	CreateUser = 1,
	EditUser = 2,
	DeleteUser = 3,
	ViewReports = 4,
	ManageInventory = 5,
	ProcessSales = 6,
	ProcessPayments = 7,
	ViewAuditLog = 8
}

// POS.Domain/Enums/PaymentMethod.cs
public enum PaymentMethod
{
	Cash = 1,
	CreditCard = 2,
	DebitCard = 3,
	Check = 4,
	MobilePayment = 5,
	Cryptocurrency = 6
}

// POS.Domain/Enums/SaleStatus.cs
public enum SaleStatus
{
	Pending = 1,
	Completed = 2,
	Refunded = 3,
	Cancelled = 4
}

// ... more enums per module
```

### 6.2 Application Constants

```csharp
// POS.Application/Constants/ValidationMessages.cs
public static class ValidationMessages
{
	public const string EmailRequired = "validation.email.required";
	public const string EmailInvalid = "validation.email.invalid";
	public const string PasswordTooShort = "validation.password.too.short";
	public const string PasswordNoSpecialChar = "validation.password.special.char";
}

// POS.Application/Constants/ErrorMessages.cs
public static class ErrorMessages
{
	public const string UserNotFound = "error.user.not.found";
	public const string EmailAlreadyExists = "error.email.already.exists";
	public const string InvalidCredentials = "error.invalid.credentials";
	public const string InsufficientPermissions = "error.insufficient.permissions";
}

// POS.Application/Constants/DefaultValues.cs
public static class DefaultValues
{
	public const string DefaultLanguage = "en";
	public const string DefaultCurrency = "USD";
	public const int DefaultPageSize = 20;
	public const int MaxPageSize = 100;
}
```

### 6.3 Configuration Constants

```csharp
// POS.Infrastructure/Configuration/ConfigurationConstants.cs
public static class ConfigurationConstants
{
	public const string ConnectionStringKey = "Database:ConnectionString";
	public const string LogLevelKey = "Logging:LogLevel:Default";
	public const string JwtSecretKey = "Authentication:JwtSecret";
	public const string JwtExpiryMinutesKey = "Authentication:JwtExpiryMinutes";
	public const string EnableAIAgentKey = "Features:EnableAIAgent";
}
```

---

## 7. Domain Events & AI Infrastructure

### 7.1 Domain Events

All aggregates publish events for audit trail and AI agent reasoning:

```csharp
// POS.Domain/Common/DomainEvent.cs
public abstract class DomainEvent
{
	public Guid EventId { get; } = Guid.NewGuid();
	public DateTime OccurredAt { get; } = DateTime.UtcNow;
	public string EventType { get; set; }
}

// POS.Domain/Aggregates/UserManagement/Events/UserCreatedEvent.cs
public class UserCreatedEvent : DomainEvent
{
	public Guid UserId { get; set; }
	public string Email { get; set; }
	public UserRole Role { get; set; }
	public DateTime CreatedAt { get; set; }
}

// Usage in aggregate
public class User : AggregateRoot
{
	public static User Create(string email, string passwordHash, UserRole role)
	{
		var user = new User
		{
			Id = Guid.NewGuid(),
			Email = email,
			PasswordHash = passwordHash,
			Role = role,
			CreatedAt = DateTime.UtcNow,
			IsActive = true
		};

		user.AddDomainEvent(new UserCreatedEvent
		{
			UserId = user.Id,
			Email = email,
			Role = role,
			CreatedAt = user.CreatedAt
		});

		return user;
	}
}
```

### 7.2 AI Infrastructure

**Event-Driven AI Reasoning:**

```csharp
// POS.AI/Agents/POSAssistantAgent.cs
public class POSAssistantAgent
{
	private readonly IAIAssistant _assistant;
	private readonly List<IAgentTool> _tools;
	private readonly ILogger<POSAssistantAgent> _logger;

	public async Task<string> ProcessUserRequest(string request, string userId, string locale)
	{
		// Provide context to agent
		var context = new AgentContext
		{
			UserId = userId,
			Locale = locale,
			AvailableTools = _tools.Select(t => t.GetToolDefinition()).ToList()
		};

		// Agent uses tools to gather data and reason
		var response = await _assistant.CompleteAsync(request, context);

		// Log for audit trail
		_logger.LogInformation($"AI Agent handled request from {userId}: {request}");

		return response;
	}
}

// POS.AI/Tools/UserManagementTools.cs
public class UserManagementTools : IAgentTool
{
	private readonly IMediator _mediator;

	public string GetToolDefinition()
	{
		return @"
		{
			'name': 'user_management',
			'description': 'User management operations',
			'functions': [
				{
					'name': 'create_user',
					'description': 'Create a new user',
					'parameters': ['email', 'role', 'permissions']
				},
				{
					'name': 'list_active_users',
					'description': 'Get all active users',
					'parameters': ['role_filter', 'limit']
				}
			]
		}";
	}

	public async Task<string> ExecuteAsync(string functionName, Dictionary<string, object> parameters)
	{
		if (functionName == "create_user")
		{
			var command = new CreateUserCommand 
			{ 
				Email = parameters["email"].ToString(),
				Role = (UserRole)Enum.Parse(typeof(UserRole), parameters["role"].ToString())
			};

			var result = await _mediator.Send(command);
			return result.IsSuccess ? "User created successfully" : result.Message;
		}

		// ... more functions

		throw new ArgumentException($"Unknown function: {functionName}");
	}
}

// POS.AI/EventConsumers/AIDomainEventConsumer.cs
public class AIDomainEventConsumer : INotificationHandler<DomainEventNotification<UserCreatedEvent>>
{
	private readonly IAuditTrailService _auditTrail;

	public async Task Handle(DomainEventNotification<UserCreatedEvent> notification, CancellationToken cancellationToken)
	{
		var evt = notification.DomainEvent;

		// Log event for AI reasoning
		await _auditTrail.LogEventAsync(new AuditLog
		{
			EventType = evt.EventType,
			EventData = JsonSerializer.Serialize(evt),
			OccurredAt = evt.OccurredAt
		});
	}
}
```

---

## 8. Testing Strategy

### 8.1 Testing Pyramid

```
		  ╱╲
		 ╱  ╲  E2E Tests (Playwright AI-assisted)
		╱────╲ ~10-15% - Critical user journeys
	   ╱      ╲
	  ╱────────╲ Integration Tests (xUnit + PostgreSQL Docker)
	 ╱          ╲ ~30-40% - Aggregate behavior, repositories
	╱────────────╲
   ╱              ╲ Unit Tests (xUnit + Moq)
  ╱────────────────╲ ~50-60% - Domain logic, handlers, services
 ╱──────────────────╲
```

### 8.2 Test Folder Structure

```
tests/
├── POS.Domain.Tests/
│   ├── Features/
│   │   ├── UserManagement/
│   │   │   ├── UserAggregateTests.cs
│   │   │   ├── RoleTests.cs
│   │   │   └── PermissionTests.cs
│   │   ├── ProductCatalog/
│   │   │   ├── ProductAggregateTests.cs
│   │   │   └── ProductCategoryTests.cs
│   │   └── Common/
│   │       ├── MoneyTests.cs
│   │       ├── EmailTests.cs
│   │       └── BarcodeTests.cs
│   └── Fixtures/
│       └── TestDataBuilder.cs

├── POS.Application.Tests/
│   ├── Features/
│   │   ├── UserManagement/
│   │   │   ├── CreateUserCommandHandlerTests.cs
│   │   │   ├── AssignRoleCommandHandlerTests.cs
│   │   │   └── GetUserQueryHandlerTests.cs
│   │   ├── ProductCatalog/
│   │   │   └── CreateProductCommandHandlerTests.cs
│   │   └── Common/
│   │       ├── LocalizationServiceTests.cs
│   │       └── CurrencyServiceTests.cs
│   └── Fixtures/
│       └── MockRepositories.cs

├── POS.Integration.Tests/
│   ├── Features/
│   │   ├── UserManagement/
│   │   │   └── UserManagementIntegrationTests.cs
│   │   ├── ProductCatalog/
│   │   │   └── ProductCatalogIntegrationTests.cs
│   │   └── Common/
│   │       └── MultiLanguageCurrencyTests.cs
│   ├── Infrastructure/
│   │   ├── DatabaseFixture.cs         # PostgreSQL Docker container
│   │   └── WebApplicationFactory.cs   # ASP.NET Core test host
│   └── Helpers/
│       └── TestDataSeeder.cs

└── POS.UI.Tests/
	├── Features/
	│   ├── UserManagement/
	│   │   ├── UserCreationE2ETests.cs
	│   │   └── LoginE2ETests.cs
	│   └── ProductCatalog/
	│       └── ProductCatalogE2ETests.cs
	└── Fixtures/
		└── BrowserFixture.cs          # Playwright configuration
```

### 8.3 Test Example: Zero Magic Strings

```csharp
// Domain Unit Test
[Fact]
public void CreateUser_WithValidEmail_ShouldSucceed()
{
	// Arrange
	var email = new Email("test@example.com");
	var role = UserRole.Cashier;  // Enum, not magic string

	// Act
	var user = User.Create(email.Value, "hashed_pwd", role);

	// Assert
	Assert.NotNull(user);
	Assert.Equal(role, user.Role);
	Assert.Single(user.DomainEvents);
	Assert.IsType<UserCreatedEvent>(user.DomainEvents.First());
}

// Application Command Handler Test with Localization
[Fact]
public async Task CreateUserCommand_WithInvalidEmail_ShouldReturnError()
{
	// Arrange
	var localizationServiceMock = new Mock<ILocalizationService>();
	localizationServiceMock
		.Setup(x => x.GetString(LocalizationKeys.UserNotFound, null))
		.Returns("User not found");  // Localized message via key constant

	var handler = new CreateUserCommandHandler(
		repositoryMock.Object,
		localizationServiceMock.Object
	);

	var command = new CreateUserCommand 
	{ 
		Email = "invalid-email",
		Role = UserRole.Cashier
	};

	// Act
	var result = await handler.Handle(command, CancellationToken.None);

	// Assert
	Assert.False(result.IsSuccess);
	Assert.Equal(LocalizationKeys.UserNotFound, result.Message);  // Message key, not string
}

// Integration Test with Multi-Currency
[Fact]
public async Task CreateProduct_WithMultipleCurrencies_ShouldStoreCorrectly()
{
	// Arrange
	using var dbContext = _factory.CreateDbContext();
	var product = new Product
	{
		Id = Guid.NewGuid(),
		Name = "Test Product",
		Price = new Money(99.99m, DefaultValues.DefaultCurrency)  // Constant currency code
	};

	// Act
	dbContext.Products.Add(product);
	await dbContext.SaveChangesAsync();

	// Assert
	var stored = await dbContext.Products.FirstAsync();
	Assert.Equal(99.99m, stored.Price.Amount);
	Assert.Equal(DefaultValues.DefaultCurrency, stored.Price.CurrencyCode);
}
```

---

## 9. SOLID Principles Applied

| Principle | Implementation |
|-----------|-----------------|
| **Single Responsibility** | Each aggregate, service, handler has one reason to change. E.g., `Money` only handles currency arithmetic, not storage. |
| **Open/Closed** | Extend via domain events & handlers, not modification. New AI features = new event consumers, not changing existing aggregates. |
| **Liskov Substitution** | All repository implementations conform to `IRepository<T>`, all services to their interfaces. |
| **Interface Segregation** | `IRepository<T>` (generic), `IUserRepository` (specific), `ITaxCalculationService` (focused). No fat interfaces. |
| **Dependency Inversion** | Controllers depend on `IMediator`, not concrete handlers. Handlers depend on `IRepository<T>`, not DbContext. |

---

## 10. Development Phase Timeline

| Phase | Modules | Weeks | Focus |
|-------|---------|-------|-------|
| **Foundation** | User Management (with roles & permissions) | 1-2 | DDD + CQRS + i18n + multi-currency scaffolding |
| **Catalog** | Product Catalog (products, categories, attributes) | 2-3 | Aggregate relationships, value objects |
| **Operations** | Inventory, Suppliers, Customers, Sales, Warehouses | 4-6 | Complex aggregates, domain services |
| **AI Integration** | Agent framework wired to all modules | 6-7 | Tool definitions, event consumers |
| **Polish** | Performance, security, deployment | 7+ | Optimization, hardening, Docker setup |

---

## 11. Technology Stack

| Component | Technology | Rationale |
|-----------|-----------|-----------|
| **Framework** | .NET 10 (C# 13) | Latest, agentic AI ready |
| **API** | ASP.NET Core 10 | Built-in middleware, OpenAPI support |
| **CQRS** | MediatR | Industry standard, clean separation |
| **Validation** | FluentValidation | Fluent API, reusable rules |
| **ORM** | Entity Framework Core 10 | PostgreSQL support, migrations |
| **Database** | PostgreSQL 15 | Powerful, open-source, JSON support |
| **Testing** | xUnit + Moq | Lightweight, flexible |
| **Integration Tests** | TestContainers + PostgreSQL Docker | Isolated, reproducible |
| **E2E Tests** | Playwright | Browser automation, AI-assisted |
| **AI** | .NET 10 Semantic Kernel | Native, performant, model-agnostic |
| **Logging** | Serilog | Structured logging, sinks |
| **Localization** | JSON files + Database | Flexible, version-controlled |

---

## 12. Key Design Decisions

1. **Modular Monolith (not microservices)** → Simpler now, path to services later
2. **Shared PostgreSQL with schema isolation** → Consistency, easier transactions
3. **DDD + CQRS from day one** → Future AI extensibility, clean boundaries
4. **Domain Events for everything** → Audit trail, AI reasoning, loose coupling
5. **No magic strings** → Testable, maintainable, translatable
6. **Money value object** → Type-safe multi-currency handling
7. **Full AI skeleton** → Agent framework ready to consume domain events
8. **Comprehensive testing** → High confidence, fast iteration

---

## 13. Success Criteria

- ✅ All modules have zero magic strings (constants or i18n keys only)
- ✅ Multi-language support functional by end of Foundation phase
- ✅ Multi-currency value objects enforced in domain
- ✅ Domain events published for all critical operations
- ✅ AI agent can query and reason about domain state
- ✅ Comprehensive test pyramid (50% unit, 40% integration, 10% E2E)
- ✅ All code follows SOLID principles
- ✅ API fully documented with OpenAPI/Swagger
- ✅ PostgreSQL schema generated from EF Core migrations
- ✅ Docker Compose for local dev + testing

---

## Appendix: Sample Code Sketches

### A.1 User Aggregate (DDD Example)

```csharp
namespace POS.Domain.Aggregates.UserManagement
{
	public class User : AggregateRoot
	{
		public Email Email { get; private set; }
		public string PasswordHash { get; private set; }
		public UserRole Role { get; private set; }
		public List<Permission> Permissions { get; private set; } = new();
		public DateTime CreatedAt { get; private set; }
		public bool IsActive { get; private set; }

		// Factory method
		public static User Create(string email, string passwordHash, UserRole role)
		{
			var user = new User
			{
				Id = Guid.NewGuid(),
				Email = new Email(email),
				PasswordHash = passwordHash,
				Role = role,
				CreatedAt = DateTime.UtcNow,
				IsActive = true
			};

			user.AddDomainEvent(new UserCreatedEvent
			{
				UserId = user.Id,
				Email = email,
				Role = role,
				CreatedAt = user.CreatedAt
			});

			return user;
		}

		// Behavior
		public void AssignRole(UserRole newRole)
		{
			if (Role == newRole) return;

			Role = newRole;
			AddDomainEvent(new RoleAssignedEvent
			{
				UserId = Id,
				OldRole = Role,
				NewRole = newRole
			});
		}

		public void GrantPermission(Permission permission)
		{
			if (Permissions.Contains(permission)) return;

			Permissions.Add(permission);
			AddDomainEvent(new PermissionGrantedEvent
			{
				UserId = Id,
				PermissionId = permission.Id
			});
		}
	}
}
```

### A.2 CreateUserCommand (CQRS Example)

```csharp
namespace POS.Application.Features.UserManagement.Commands
{
	public class CreateUserCommand : IRequest<Result<UserResponse>>
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public UserRole Role { get; set; }
		public string PreferredLanguage { get; set; } = DefaultValues.DefaultLanguage;
	}

	public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
	{
		public CreateUserCommandValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty()
				.EmailAddress()
				.WithMessage(ValidationMessages.EmailInvalid);

			RuleFor(x => x.Password)
				.MinimumLength(8)
				.WithMessage(ValidationMessages.PasswordTooShort)
				.Matches(@"[!@#$%^&*(),.?]")
				.WithMessage(ValidationMessages.PasswordNoSpecialChar);
		}
	}

	public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserResponse>>
	{
		private readonly IUserRepository _userRepository;
		private readonly ILocalizationService _localizationService;
		private readonly ILogger<CreateUserCommandHandler> _logger;

		public async Task<Result<UserResponse>> Handle(
			CreateUserCommand request,
			CancellationToken cancellationToken)
		{
			try
			{
				var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
				var user = User.Create(request.Email, passwordHash, request.Role);

				await _userRepository.AddAsync(user, cancellationToken);
				await _userRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

				var message = _localizationService.GetString(
					LocalizationKeys.UserCreated,
					locale: request.PreferredLanguage
				);

				_logger.LogInformation($"User created: {user.Id}");

				return Result<UserResponse>.Success(
					new UserResponse { Id = user.Id, Email = user.Email.Value },
					message
				);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error creating user: {ex.Message}");
				var errorMessage = _localizationService.GetString(
					ErrorMessages.UserNotFound,  // Appropriate error key
					locale: request.PreferredLanguage
				);
				return Result<UserResponse>.Failure(errorMessage);
			}
		}
	}
}
```

---

## Document Status

**Status:** Ready for Review  
**Next Steps:**
1. User reviews this design specification
2. Any required changes/clarifications
3. Approval → Implementation plan creation
4. Implementation begins with User Management module

---

**Document Version:** 1.0  
**Last Updated:** 2025-01-10

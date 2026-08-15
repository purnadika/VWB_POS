# NETPOS — Architecture & Developer Notes

## Technology Stack

| Layer        | Technology                                  |
|--------------|---------------------------------------------|
| Backend API  | ASP.NET Core 10 (Minimal API + Controllers) |
| ORM          | Entity Framework Core 10 (Code-First)       |
| Database     | PostgreSQL 15                               |
| Auth         | JWT Bearer + BCrypt.Net-Next 4.0.2          |
| CQRS         | MediatR                                     |
| Validation   | FluentValidation                            |
| Frontend     | React 18 + TypeScript (Vite)                |
| E2E Tests    | Playwright                                  |

---

## Backend Architecture

The backend follows **Clean Architecture** with strict dependency inversion:

```
POS.WebAPI  →  POS.Application  →  POS.Domain
                    ↑
             POS.Infrastructure
```

### Project Responsibilities

#### `POS.Domain`
- Pure domain logic with no external dependencies.
- Contains: Entities, Aggregates, Value Objects, Domain Events, Repository Interfaces.
- Key entities: `User`, `Item`, `Sale`, `Customer`, `Employee`, `AuthLog`, `Message`, `Report`, `AppConfig`, etc.
- Soft-delete pattern: entities have `IsDeleted`, `DeletedDate`, `DeletedBy` fields.

#### `POS.Application`
- Orchestration layer using MediatR CQRS pattern.
- Each feature has its own folder under `Features/`:
  - `Commands/` — Write operations (Create, Update, Delete)
  - `Queries/` — Read operations (GetAll, GetById)
  - `Handlers/` — MediatR handlers implementing commands/queries
  - `DTOs/` — Data Transfer Objects for API contracts
- Depends on Domain interfaces only (not Infrastructure).
- Includes BCrypt dependency for `LoginCommandHandler`.

#### `POS.Infrastructure`
- Implements repository interfaces defined in Domain.
- Contains: `POSDbContext`, Repository implementations, EF Core configurations.
- Includes BCrypt.Net-Next 4.0.2 for password operations.

#### `POS.WebAPI`
- ASP.NET Core host.
- Controllers map HTTP endpoints to MediatR commands/queries.
- `Program.cs` handles DI registration, middleware, and database seeding.

---

## Authentication Flow

```
Client → POST /api/v1/usermanagement/login
           ↓
     LoginCommandHandler
           ↓
     IUserRepository.FindByEmailAsync(email)
           ↓
     BCrypt.Verify(password, user.PasswordHash)
           ↓ (success)
     Generate JWT token
     Log to AuthLogs (IsSuccess=true)
           ↓ (failure)
     Log to AuthLogs (IsSuccess=false, FailureReason)
     Return 401
```

**Token Storage:** JWT is stored in browser `localStorage` under key `auth_token`.  
**Token Usage:** Sent as `Authorization: Bearer <token>` header on every API request via `utils/api.ts`.

---

## Frontend Architecture

```
src/
├── contexts/
│   └── AuthContext.tsx       # JWT state, login(), logout()
├── layouts/
│   ├── AdminLayout.tsx       # Sidebar nav + functional Logout
│   └── PosLayout.tsx         # POS header + functional Logout
├── components/
│   └── CrudDataTable.tsx     # Reusable table/modal/CRUD component
├── pages/
│   ├── LoginPage.tsx         # Auth form with inline error display
│   ├── PosPage.tsx           # POS screen with toast notifications
│   └── [Module]Page.tsx      # Admin module pages (use CrudDataTable)
└── utils/
    └── api.ts                # fetchApi() — central HTTP client
```

### `utils/api.ts` — Error Handling

All API errors are normalized before being thrown:
1. Parse the response body as text.
2. Attempt `JSON.parse()` on the text.
3. Extract `.error` → `.message` → `.title` → raw text → HTTP status text.
4. Throw `ApiError(status, message)` with clean human-readable message.

This ensures every `catch (err: any)` block receives `err.message` as a plain string, not a JSON blob.

### `CrudDataTable.tsx` — Shared CRUD UI

Used by all admin module pages. Accepts:
- `endpoint` — API base path (e.g., `/customers`)
- `columns` — Table column definitions with optional `render()` function
- `formFields` — Modal form field definitions (name, label, type)
- `primaryKey` — Defaults to `'id'`

Error handling:
- **Load errors** → Logged to console (table shows "No records found")
- **Save errors** → Inline red banner inside the modal (not a popup alert)
- **Delete errors** → `alert()` with specific server error message

---

## Database — Key Tables

| Table        | Purpose                                      |
|--------------|----------------------------------------------|
| `Users`      | Authentication accounts (BCrypt hashed pw)   |
| `Employees`  | Employee records (separate from auth users)  |
| `AuthLogs`   | Audit log of every login attempt             |
| `Items`      | Product catalog                              |
| `Sales`      | Completed POS transactions                   |
| `Customers`  | Customer records                             |
| `Suppliers`  | Supplier records                             |
| `GiftCards`  | Gift card records                            |
| `Expenses`   | Business expenses                            |
| `Receivings` | Inventory receiving records                  |
| `Taxes`      | Tax configuration                            |
| `Messages`   | Internal messages                            |
| `Reports`    | Saved report definitions                     |
| `AppConfigs` | Key/value application configuration          |

---

## Known Gotchas

1. **`IUnitOfWork` does not expose a generic `Repository<T>`** — repositories must be injected directly as their specific interface (e.g., `IUserRepository`).
2. **BCrypt.Net-Next version conflict** — Must be pinned to `4.0.2` in `POS.Infrastructure.csproj` to avoid downgrade errors.
3. **Database seeding is idempotent** — `SeedDataAsync` in `Program.cs` checks for existing records before inserting. Seeding runs on every app start.
4. **EF Core migration** must be run manually after schema changes: `dotnet ef migrations add <Name> --project POS.Infrastructure --startup-project POS.WebAPI`.

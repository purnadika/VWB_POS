# NETPOS Changelog

All notable changes to this project are documented here in reverse chronological order.

---

## [Unreleased] — 2026-08-02

### Localization (Strings + Currency)
- **`LocaleContext.tsx`** created — reads `locale`, `currency`, and `language` keys from the `/api/configuration` endpoint at startup. Provides `formatCurrency(amount)` and `formatDate(date)` helpers using the native `Intl` browser API.
- **`LocaleProvider`** wraps the entire React app in `App.tsx` so every page has access.
- **Currency formatting** replaced all hardcoded `$` + `.toFixed(2)` in `PosPage.tsx` with `formatCurrency()` — now locale-aware (e.g. `Rp 25.000` for IDR, `$25.00` for USD).
- **Default locale seeded**: `locale=id-ID`, `currency=IDR`, `language=id` added to `AppConfig` during database seeding.
- **Indonesian translation** (`id.json`) added to `POS.Infrastructure/Resources/i18n/` — complete coverage of all `LocalizationKeys` constants.
- **English translation** (`en.json`) was already present.
- Locale/currency/language can be changed live from the **Settings** admin page (key/value editor) — no code change needed.

### Item Category (Normalization)
- `Category` field on `Item` changed from a free-text `string` to a **proper FK** (`CategoryId → ItemCategory`).
- Created `ItemCategory` entity (`Name`, `Description`) with full CQRS stack: `GetItemCategoriesQuery`, `CreateItemCategoryCommand`, `UpdateItemCategoryCommand`, `DeleteItemCategoryCommand`.
- Created `ItemCategoriesController` at `GET/POST/PUT/DELETE /api/item-categories`.
- Created `ItemCategoriesPage.tsx` with full CRUD table and added to Admin sidebar + router at `/admin/item-categories`.
- EF Core migration `AddItemCategory` created and applied.
- Seeded default `Electronics` category in `Program.cs`.

### Authentication
- **Replaced mock authentication** with real BCrypt password validation inside `LoginCommandHandler`.
  - `IUserRepository.FindByEmailAsync()` is used to fetch the user record by email.
  - `BCrypt.Net.BCrypt.Verify()` validates the provided password against the stored hash.
  - Invalid credentials now correctly return `401 Unauthorized` with `{"error": "Invalid credentials"}`.
- **Seeded real admin account** in `Program.cs` (`SeedDataAsync`) with BCrypt-hashed password for `admin@example.com`.
  - Seeding is idempotent (checks before inserting to avoid duplicates).
- **Auth Audit Logging**: All login attempts (success and failure) are now written to the `AuthLogs` table via `IAuthLogRepository`, recording email, timestamp, IP, success/failure status, and failure reason.

### UI — Logout
- **Added functional Logout** to `AdminLayout.tsx` (sidebar button) — was previously a static button with no handler.
- **Added Logout button to `PosLayout.tsx`** (POS screen header) — users on the POS screen can now sign out without navigating to Admin first.
- Both call `useAuth().logout()` which clears `localStorage` and redirects to `/login`.

### UI — Error Message Rendering
- **Fixed raw JSON error display** across the entire application.
  - `utils/api.ts` (`fetchApi`) now parses JSON error responses and extracts the human-readable message from `.error`, `.message`, or `.title` fields before throwing.
  - Previously, raw JSON blobs like `{"error":"Invalid credentials"}` were displayed directly in the UI.
- **`CrudDataTable.tsx`** — Save/Update errors now render as an **inline red banner inside the modal** instead of a browser `alert()` popup.
  - Error state is cleared when the modal is re-opened.
- **`CrudDataTable.tsx`** — Delete errors now show the specific server error message via `alert()` instead of the generic fallback.
- **`PosPage.tsx`** — Checkout success and failure replaced `alert()` with an **animated inline notification banner** (green for success, red for error) that:
  - Auto-dismisses after 4 seconds on success.
  - Shows the real API error message on failure.
  - Can be manually dismissed by clicking.

---

## Foundation Phase — 2026-07-26 to 2026-08-01

### Backend — Domain & Persistence
- Added `Message` and `Report` entities to `POS.Domain`.
- Added `AuthLog` entity to `POS.Domain` for audit trail (fields: Id, Email, IpAddress, IsSuccess, FailureReason, AttemptedAt).
- Added `DeletedDate` and `DeletedBy` soft-delete fields to relevant entities.
- Updated `POSDbContext.cs` with new entity configurations.
- Created and applied EF Core migration: `AddMessagesReportsAppConfigs`.

### Backend — CQRS & Controllers
- Implemented real CQRS handlers (MediatR) for: **GiftCards, Expenses, Receivings, Taxes, Messages, Reports, AppConfig (Settings)**.
- Replaced all stub handlers with real DB persistence.
- Created missing `MessagesController.cs`.
- Implemented `LoginCommandHandler` with real BCrypt auth (see Authentication section above).

### Frontend — Admin Pages
- Updated all admin page modules to use real API fields instead of placeholders:
  - `GiftCardsPage.tsx`, `ExpensesPage.tsx`, `ReceivingsPage.tsx`, `TaxesPage.tsx`, `MessagesPage.tsx`, `ReportsPage.tsx`, `SettingsPage.tsx`
- `SettingsPage.tsx` correctly maps to `/configuration` endpoint.

### Testing — Playwright E2E
- Created E2E test specs for: `giftcards.spec.ts`, `expenses.spec.ts`, `receivings.spec.ts`, `taxes.spec.ts`, `messages.spec.ts`, `settings.spec.ts`.
- Tests interact with the real API (not mocks) and verify full CRUD lifecycle.

---

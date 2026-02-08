# Changelog

All notable changes to this project will be documented in this file.  
Project follows Clean Architecture principles and incremental training exercises.

---

## [Exercise 1] – Clean Architecture & Basic API

### Added

- ASP.NET Core Web API project with Clean Architecture structure
- Separation of layers:
  - Domain
  - Application
  - Presentation
- Basic Order API (CRUD in-memory)
- DTOs for request & response
- Swagger/OpenAPI integration
- Global middleware (custom headers)
- Basic validation & error handling

---

## [Exercise 2] – Database Integration & Authorization

### Added

- Entity Framework Core integration (SQL Server)
- Database migrations
- Persistent entities:
  - Users
  - Orders
  - OrderItems
  - Customers
  - Products
  - Categories
- JWT Authentication (Access Token)
- User registration & login API
- Role-based authorization (ADMIN / USER)
- Order ownership logic:
  - USER can only access own orders
  - ADMIN can access all orders
- Seeder for default admin user
- Product & Category CRUD APIs
- Decimal precision configuration for monetary fields

### Changed

- Order storage migrated from in-memory to database
- Order now linked to User & Customer via foreign keys

---

## [Exercise 3] – Advanced Features & Integration

### Added

- Email Notification Service
  - SMTP integration (MailKit)
  - Configurable via `appsettings.json`
- SignalR real-time notifications
  - Order status update broadcast
  - Node.js SignalR client example
- Order status update notification pipeline
- Excel export feature using ClosedXML
  - Export orders to `.xlsx`
- OrderExportService
- Real-time + async service integration

### Fixed

- JSON circular reference issue using DTO mapping
- Foreign key constraint errors on Order creation
- Enum serialization issues
- SignalR client async execution issue in Node.js

---

## [Exercise 4] – Products & Inventory Management

### Added

- Products domain with full CRUD support:
  - Create product (ADMIN only)
  - Update product (ADMIN only)
  - Delete product (ADMIN only)
  - View product list and product detail
- Product inventory support (`Stock` field)
- Automatic stock deduction when creating an order
- Validation to prevent ordering beyond available stock
- Product management UI (Angular):
  - Product list page
  - Product detail page
  - Product create & edit form (Reactive Forms)
- Role-based UI behavior (ADMIN vs USER)
- Global toast notification system
- Global HTTP error interceptor
- Session expiration handling (auto logout + redirect)
- SignalR real-time order status updates with safe reconnect handling

### 🔧 Changed

- Extended `Product` entity with `Stock`
- Updated Product DTOs to include inventory data
- Updated order creation flow to:
  - Validate product availability
  - Deduct stock atomically during order creation
- Improved Angular routing structure using a shared layout
- Refactored Angular forms to use Reactive Forms
- Improved SSR safety by guarding browser-only operations

### 🛠 Fixed

- Fixed multiple SSR-related `401 Unauthorized` errors
- Fixed Angular hydration issues causing stuck loading states
- Fixed SignalR infinite reconnect loop when token expired
- Fixed token lifecycle issues (expired token handling)
- Fixed TypeScript union Observable subscribe error
- Fixed UI inconsistencies (missing labels, textarea styling)
- Fixed product selection and payload mapping issues during order creation

### 🔐 Security

- Enforced authentication on all protected API endpoints
- Enforced role-based authorization for product management
- Implemented centralized handling for expired or invalid JWT tokens

### 🧠 Technical Notes

- Order creation and stock deduction are handled in a single EF Core transaction
- Inventory validation is enforced at backend level (not only UI)
- Angular HTTP interceptors are used for:
  - Authorization header injection
  - Global error handling
- SignalR connections are disabled when authentication is invalid
- Designed to be SSR-safe with Angular v21+

---

© 2026 Naufal Hilmiaji – Training Project

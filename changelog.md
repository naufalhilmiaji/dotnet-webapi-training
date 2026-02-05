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

## [Unreleased / Next Steps]
- Refresh token implementation
- Pagination & filtering
- Background jobs (Hangfire / Quartz)
- Unit & integration testing
- API versioning
- Audit logs
- Docker & deployment setup

---

© 2026 Naufal Hilmiaji – Training Project

# NhjDotnetApi

**NhjDotnetApi** adalah contoh **ASP.NET Core Web API** yang dibangun menggunakan **Clean Architecture**, **JWT Authentication**, dan **Entity Framework Core**, sebagai bagian dari **backend training exercise**.

Project ini mensimulasikan **Ordering System** dengan fitur **authentication, authorization, order ownership, real-time notification (SignalR), email notification**, dan **export data ke Excel**.

---

## ✨ Fitur Utama

* 🔐 **User Registration & Login (JWT)**
* 🛡️ **Role-based Authorization (ADMIN & USER)**
* 👤 **Order Ownership (USER hanya akses order miliknya)**
* 📦 **Order dengan multiple items**
* 🔄 **Update status order (ADMIN only)**
* 📡 **Real-time notification (SignalR)**
* 📧 **Email notification**
* 📊 **Export Order ke Excel (ClosedXML)**
* 🧱 **Clean Architecture**
* 🗄️ **SQL Server + EF Core (Migration)**

---

## 🏗️ Arsitektur Project (Clean Architecture)

```
NhjDotnetApi
│
├── Domain
│   └── Entities
│       ├── User.cs
│       ├── Order.cs
│       ├── OrderItem.cs
│       └── OrderStatus.cs
│
├── Application
│   ├── Contracts
│   │   ├── IAuthService.cs
│   │   ├── IOrderService.cs
│   │   ├── IEmailService.cs
│   │   └── IOrderNotificationService.cs
│   └── Services
│       ├── AuthService.cs
│       ├── OrderService.cs
│       ├── EmailService.cs
│       └── OrderExportService.cs
│
├── Persistence
│   ├── AppDbContext.cs
│   └── Migrations
│
├── Presentation
│   ├── Controllers
│   │   ├── AuthController.cs
│   │   ├── OrdersController.cs
│   │   ├── EmailNotificationController.cs
│   │   └── ExportController.cs
│   ├── Hubs
│   │   └── OrderHub.cs
│   └── Models (DTOs)
│
├── Program.cs
├── README.md
├── CHANGELOG.md
└── Postman
    ├── NhjDotnetApi.postman_collection.json
    └── NhjDotnetApi.postman_environment.json
```

### Prinsip yang Diterapkan

* Controller **hanya handle HTTP**
* Business logic berada di **Application layer**
* Entity **tidak langsung diekspos ke API**
* Authorization & ownership **divalidasi di service**
* Infrastructure (DB, Email, SignalR) **terpisah**

---

## 🔐 Authentication & Authorization

### Register User

```
POST /api/users/register
```

### Login

```
POST /api/auth/login
```

Response:

```json
{
  "success": true,
  "token": "<JWT_TOKEN>",
  "role": "USER"
}
```

### Authorization

* Token dikirim via header:

```
Authorization: Bearer <JWT_TOKEN>
```

* ADMIN memiliki akses penuh
* USER hanya dapat mengakses **order miliknya sendiri**

---

## 🛒 Order API

### Create Order

```
POST /api/orders
```

```json
{
  "customerId": "GUID",
  "items": [
    {
      "productName": "Mouse",
      "quantity": 2,
      "price": 150000
    }
  ]
}
```

### Get Orders

* ADMIN → semua order
* USER → hanya order miliknya

```
GET /api/orders
```

### Update Order Status (ADMIN)

```
PUT /api/orders/{id}/status
```

```json
{
  "newStatus": 2
}
```

---

## 📡 Real-time Notification (SignalR)

* Endpoint:

```
/orderHub
```

* Notifikasi dikirim saat status order berubah
* Mendukung client **Node.js**

---

## 📧 Email Notification

* Menggunakan **SMTP**
* Digunakan untuk simulasi notifikasi sistem
* Konfigurasi melalui `appsettings.json`

---

## 📊 Export Order ke Excel

```
GET /api/export/orders
```

* Menghasilkan file `.xlsx`
* Menggunakan **ClosedXML**
* Cocok untuk kebutuhan reporting

---

## 🧪 API Testing (Postman)

Folder:

```
Postman/
```

Berisi:

* Collection
* Environment
* Siap digunakan untuk:

  * Register
  * Login
  * Order
  * Update Status
  * Export

---

## 🚀 Menjalankan Project

```bash
dotnet restore
dotnet ef database update
dotnet run
```

Atau saat development:

```bash
dotnet watch run
```

Swagger:

```
http://localhost:5197/swagger
```

---

## 🎯 Tujuan Training

Project ini bertujuan untuk melatih:

* Clean Architecture di ASP.NET Core
* JWT Authentication & Authorization
* EF Core + Migration
* Ownership & role-based access
* Real-time system (SignalR)
* Integrasi email & export data

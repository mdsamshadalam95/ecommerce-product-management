# E-Commerce Product Management System

A full-stack e-commerce product management system built using ASP.NET Core Web API, Angular, Entity Framework Core, and SQL Server.

The application provides product catalog management, category management, inventory tracking, authentication, role-based authorization, and an administrative dashboard.

The project is designed as a production-style full-stack application and can be extended later with Docker, Azure, and Azure DevOps CI/CD.

---

## 1. Project Overview

### Business Requirement

The system manages products for an e-commerce platform.

Administrators can:

* Create products
* Update products
* Soft-delete products
* View products
* Search products
* Filter products
* Sort products
* Manage categories
* Manage inventory
* Adjust stock
* View inventory transaction history
* Monitor low-stock products

Regular users can:

* Login
* View active products
* Search products
* Filter products
* View product details

The system uses role-based authorization to control access.

---

## 2. Business Roles

### Admin

Admin users can:

* Login
* Create products
* Update products
* Soft-delete products
* Manage categories
* Adjust inventory
* View inventory history
* View dashboard information

### Regular User

Regular users can:

* Login
* View products
* Search products
* Filter products
* View product details

Regular users cannot:

* Create products
* Update products
* Delete products
* Manage categories
* Adjust inventory

---

# 3. Technology Stack

## Backend

* C#
* ASP.NET Core Web API
* Entity Framework Core
* ASP.NET Core Identity
* JWT Authentication
* REST APIs
* Swagger/OpenAPI
* SQL Server
* FluentValidation
* Serilog

## Frontend

* Angular
* TypeScript
* Angular Router
* Reactive Forms
* HttpClient
* RxJS
* Angular Material

## Testing

* xUnit
* Moq
* ASP.NET Core integration testing
* Angular testing

## DevOps

* Git
* GitHub
* Azure DevOps
* Azure Pipelines

## Optional

* Docker
* Docker Compose
* Azure App Service
* Azure SQL Database

---

# 4. High-Level Architecture

```text
                         ┌──────────────────────┐
                         │      Angular UI      │
                         │                      │
                         │ Admin Dashboard      │
                         │ Product Management   │
                         │ Category Management  │
                         │ Inventory Management │
                         └──────────┬───────────┘
                                    │
                                  HTTPS
                                    │
                                    ▼
                         ┌──────────────────────┐
                         │ ASP.NET Core Web API │
                         │                      │
                         │ Controllers          │
                         │ Authentication       │
                         │ Authorization        │
                         │ Validation           │
                         │ Exception Handling   │
                         └──────────┬───────────┘
                                    │
                                    ▼
                         ┌──────────────────────┐
                         │ Application Layer    │
                         │                      │
                         │ Product Service      │
                         │ Category Service     │
                         │ Inventory Service    │
                         │ Auth Service         │
                         └──────────┬───────────┘
                                    │
                                    ▼
                         ┌──────────────────────┐
                         │ Infrastructure       │
                         │                      │
                         │ EF Core              │
                         │ SQL Server           │
                         │ Identity             │
                         └──────────┬───────────┘
                                    │
                                    ▼
                         ┌──────────────────────┐
                         │      SQL Server      │
                         └──────────────────────┘
```

---

# 5. Repository Structure

```text
ecommerce-product-management/
│
├── .github/
│   └── copilot-instructions.md
│
├── backend/
│   ├── ECommerce.Api/
│   ├── ECommerce.Application/
│   ├── ECommerce.Domain/
│   ├── ECommerce.Infrastructure/
│   └── ECommerce.Tests/
│
├── frontend/
│   └── ecommerce-admin/
│
├── database/
│   ├── scripts/
│   └── README.md
│
├── docker/
│
├── azure-devops/
│
├── docs/
│
├── .gitignore
└── README.md
```

---

# 6. Backend Architecture

The backend follows a layered architecture.

```text
ECommerce.Api
       │
       ▼
ECommerce.Application
       │
       ▼
ECommerce.Domain

ECommerce.Infrastructure
       │
       ├── EF Core
       ├── SQL Server
       └── ASP.NET Identity
```

## Projects

### ECommerce.Api

Responsible for:

* HTTP endpoints
* Controllers
* Middleware
* Authentication configuration
* Authorization
* Swagger
* Dependency injection

### ECommerce.Application

Responsible for:

* Business logic
* Services
* DTOs
* Interfaces
* Validators
* Application workflows

### ECommerce.Domain

Responsible for:

* Entities
* Enums
* Domain concepts
* Business rules

### ECommerce.Infrastructure

Responsible for:

* EF Core
* DbContext
* Database configuration
* Entity configurations
* Migrations
* Repositories
* ASP.NET Identity

### ECommerce.Tests

Responsible for:

* Unit tests
* Integration tests
* API tests

---

# 7. Database Design

The initial database contains the following logical tables.

```text
Categories
    │
    │ 1:N
    ▼
Products
    │
    ├─────────────── 1:1 ─────────────── Inventory
    │
    └─────────────── 1:N ─────────────── InventoryTransactions
```

Authentication uses ASP.NET Core Identity tables.

---

# 8. Main Tables

## Categories

| Column      | Type          | Description          |
| ----------- | ------------- | -------------------- |
| CategoryId  | INT           | Primary key          |
| Name        | NVARCHAR(100) | Category name        |
| Description | NVARCHAR(500) | Category description |
| IsActive    | BIT           | Active status        |
| CreatedAt   | DATETIME2     | Creation timestamp   |
| UpdatedAt   | DATETIME2     | Last update          |
| CreatedBy   | INT           | Creating user        |
| UpdatedBy   | INT           | Updating user        |

---

## Products

| Column      | Type          | Description         |
| ----------- | ------------- | ------------------- |
| ProductId   | INT           | Primary key         |
| SKU         | NVARCHAR(50)  | Unique product code |
| Name        | NVARCHAR(200) | Product name        |
| Description | NVARCHAR(MAX) | Product description |
| CategoryId  | INT           | Foreign key         |
| Price       | DECIMAL(18,2) | Selling price       |
| CostPrice   | DECIMAL(18,2) | Product cost        |
| Status      | INT           | Product status      |
| Brand       | NVARCHAR(100) | Brand               |
| ImageUrl    | NVARCHAR(500) | Product image       |
| IsDeleted   | BIT           | Soft delete         |
| CreatedAt   | DATETIME2     | Creation timestamp  |
| UpdatedAt   | DATETIME2     | Last update         |
| CreatedBy   | INT           | Creating user       |
| UpdatedBy   | INT           | Updating user       |

---

## Inventory

| Column            | Type      | Description         |
| ----------------- | --------- | ------------------- |
| InventoryId       | INT       | Primary key         |
| ProductId         | INT       | Foreign key         |
| QuantityAvailable | INT       | Current quantity    |
| ReservedQuantity  | INT       | Reserved quantity   |
| ReorderLevel      | INT       | Low-stock threshold |
| LastUpdatedAt     | DATETIME2 | Last update         |

Available quantity:

```text
Available Quantity =
QuantityAvailable - ReservedQuantity
```

---

## InventoryTransactions

| Column                 | Type          | Description        |
| ---------------------- | ------------- | ------------------ |
| InventoryTransactionId | BIGINT        | Primary key        |
| ProductId              | INT           | Foreign key        |
| TransactionType        | INT           | Transaction type   |
| Quantity               | INT           | Quantity changed   |
| PreviousQuantity       | INT           | Previous stock     |
| NewQuantity            | INT           | New stock          |
| ReferenceNumber        | NVARCHAR(100) | Related reference  |
| Remarks                | NVARCHAR(500) | Transaction reason |
| CreatedAt              | DATETIME2     | Creation timestamp |
| CreatedBy              | INT           | User               |

Inventory transactions provide an audit trail.

Example:

```text
Opening Stock    +100
Purchase          +50
Sale              -10
Damage             -5
Adjustment         +10
---------------------
Current Stock     145
```

---

# 9. Product Status

The application uses:

```text
Draft
Active
Inactive
```

A deleted product is represented using:

```text
IsDeleted = true
```

Products should normally be soft-deleted rather than physically removed.

---

# 10. Inventory Transaction Types

Initial transaction types:

```text
OpeningStock
Purchase
Sale
Return
Damage
Adjustment
```

---

# 11. API Design

Base URL:

```text
/api
```

## Authentication

```http
POST /api/auth/register
POST /api/auth/login
POST /api/auth/refresh
POST /api/auth/logout
GET  /api/auth/me
```

---

## Products

```http
GET    /api/products
GET    /api/products/{id}
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}
```

Product listing supports:

* Pagination
* Search
* Category filtering
* Status filtering
* Price filtering
* Sorting

Example:

```http
GET /api/products?pageNumber=1&pageSize=20&search=iphone&categoryId=1
```

---

## Categories

```http
GET    /api/categories
GET    /api/categories/{id}
POST   /api/categories
PUT    /api/categories/{id}
DELETE /api/categories/{id}
```

---

## Inventory

```http
GET  /api/inventory/{productId}
POST /api/inventory/{productId}/adjust
GET  /api/inventory/{productId}/transactions
GET  /api/inventory/low-stock
```

---

# 12. Authorization Rules

Admin-only operations:

```text
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}

POST   /api/categories
PUT    /api/categories/{id}
DELETE /api/categories/{id}

POST   /api/inventory/{productId}/adjust
```

Authenticated users:

```text
GET /api/products
GET /api/products/{id}
GET /api/categories
```

---

# 13. Standard HTTP Status Codes

```text
200 OK
201 Created
204 No Content
400 Bad Request
401 Unauthorized
403 Forbidden
404 Not Found
409 Conflict
500 Internal Server Error
```

---

# 14. API Response Standards

Successful responses should be consistent.

Example:

```json
{
  "success": true,
  "message": "Product created successfully.",
  "data": {}
}
```

Validation error:

```json
{
  "success": false,
  "message": "Validation failed.",
  "errors": [
    {
      "field": "sku",
      "message": "SKU is required."
    }
  ]
}
```

---

# 15. DTO Rules

EF Core entities must not be exposed directly through API controllers.

Use DTOs.

Example:

```text
CreateProductRequest
UpdateProductRequest
ProductResponse
ProductListResponse
```

---

# 16. Angular Application

Angular uses feature-based architecture.

```text
src/app/
│
├── core/
│   ├── guards/
│   ├── interceptors/
│   ├── services/
│   └── models/
│
├── shared/
│   ├── components/
│   ├── directives/
│   └── pipes/
│
└── features/
    ├── auth/
    ├── dashboard/
    ├── products/
    ├── categories/
    └── inventory/
```

---

# 17. Angular Screens

## Authentication

```text
/login
```

## Dashboard

```text
/dashboard
```

Dashboard displays:

```text
Total Products
Total Categories
Low Stock Products
Out of Stock Products
```

## Products

```text
/products
/products/create
/products/:id
/products/:id/edit
```

## Categories

```text
/categories
/categories/create
/categories/:id/edit
```

## Inventory

```text
/inventory
/inventory/:productId
/inventory/:productId/history
```

---

# 18. Angular Authentication Flow

```text
Login
  │
  ▼
POST /api/auth/login
  │
  ▼
JWT
  │
  ▼
AuthService
  │
  ▼
HTTP Interceptor
  │
  ▼
Authorization: Bearer <token>
  │
  ▼
ASP.NET Core API
```

---

# 19. Security Requirements

The application must:

* Use HTTPS in deployed environments
* Use JWT authentication
* Use role-based authorization
* Never store passwords as plain text
* Use ASP.NET Core Identity password hashing
* Validate all API inputs
* Avoid exposing database entities
* Avoid returning sensitive information
* Validate ownership/authorization where applicable
* Use secure configuration for secrets
* Never commit passwords, tokens, or connection strings containing credentials

---

# 20. Product Business Rules

1. SKU must be unique.
2. Product name is required.
3. Price must be greater than zero.
4. Category must exist.
5. Deleted products cannot appear in normal product listings.
6. Inactive products should not appear as active products.
7. Products should be soft-deleted.
8. Product SKU should not normally change after creation.

---

# 21. Inventory Business Rules

1. Inventory cannot become negative.
2. Every inventory change must create an inventory transaction.
3. Inventory transactions are append-only.
4. Previous quantity must match the current inventory quantity before modification.
5. New quantity must be calculated by the server.
6. Inventory history must not be manually modified.
7. Low-stock products are products where available quantity is below the reorder level.

---

# 22. Validation

Backend validation is mandatory.

Examples:

```text
SKU required
Name required
CategoryId required
Price > 0
Quantity >= 0
ReorderLevel >= 0
```

Business validation must be performed on the server even if Angular performs client-side validation.

---

# 23. Exception Handling

The API should use centralized exception handling middleware.

Expected behavior:

```text
Controller
    │
    ▼
Service
    │
    ▼
Exception
    │
    ▼
Global Exception Middleware
    │
    ▼
Standard Error Response
```

Internal exception details must not be exposed to production clients.

---

# 24. Logging

Use structured logging.

Important events:

```text
User login
Product creation
Product update
Product deletion
Inventory adjustment
Authentication failure
Unhandled exception
API errors
```

---

# 25. Testing Strategy

Tests should cover:

### Product

* Create product
* Update product
* Get product
* Search products
* Pagination
* Duplicate SKU
* Invalid category
* Soft delete

### Category

* Create category
* Update category
* Delete category
* Duplicate category

### Inventory

* Increase inventory
* Decrease inventory
* Prevent negative inventory
* Inventory transaction creation
* Low-stock detection

### Security

* Admin authorization
* Regular user authorization
* Invalid JWT
* Missing JWT

---

# 26. Git Strategy

Branches:

```text
main
develop
feature/*
bugfix/*
```

Feature example:

```text
feature/product-crud
feature/jwt-authentication
feature/inventory-management
feature/angular-product-management
```

Pull requests should be used before merging into `develop` and `main`.

---

# 27. Development Phases

## Phase 1

Repository and architecture.

## Phase 2

.NET solution and project structure.

## Phase 3

Domain entities and database.

## Phase 4

EF Core migrations and seed data.

## Phase 5

Authentication and authorization.

## Phase 6

Product APIs.

## Phase 7

Category APIs.

## Phase 8

Inventory APIs.

## Phase 9

Angular application.

## Phase 10

Angular authentication and authorization.

## Phase 11

Product management UI.

## Phase 12

Category and inventory UI.

## Phase 13

Unit and integration testing.

## Phase 14

Docker.

## Phase 15

Azure DevOps CI/CD.

## Phase 16

Azure deployment.

---

# 28. Initial Development Milestone

The first working milestone should contain:

```text
ASP.NET Core API
        +
SQL Server
        +
EF Core
        +
JWT Authentication
        +
Admin/User Roles
        +
Product CRUD
        +
Category CRUD
        +
Inventory Management
        +
Angular UI
```

---

# 29. Future Enhancements

Potential future features:

* Shopping cart
* Orders
* Payments
* Customer management
* Product images
* Product variants
* Multiple warehouses
* Supplier management
* Purchase orders
* Discount management
* Product reviews
* Notifications
* Email
* Redis caching
* Azure Service Bus
* Microservices
* Elasticsearch
* Observability
* Application Insights

These should not be implemented in the initial MVP.

---

# 30. Definition of Done

A feature is complete only when:

* Code compiles
* API works
* Validation exists
* Authorization exists where required
* Unit tests exist
* Error handling exists
* Logging exists where appropriate
* Swagger is updated
* Angular integration works
* No secrets are committed
* Code follows project architecture
* Build succeeds
* Tests pass

---

# 31. Local Development

Required tools:

```text
.NET SDK
Node.js
Angular CLI
SQL Server
Git
Visual Studio
VS Code
```

Optional:

```text
Docker Desktop
Azure CLI
```

---

# 32. Planned Local URLs

Backend:

```text
https://localhost:7001
```

Swagger:

```text
https://localhost:7001/swagger
```

Angular:

```text
http://localhost:4200
```

These ports may be changed during implementation.

---

# 33. Project Principle

The project should prioritize:

```text
Clean architecture
Maintainability
Security
Testability
Performance
Readable code
Separation of concerns
API consistency
Real-world business rules
```

The application should be developed incrementally.

Do not generate the entire application in one step.

Implement one feature at a time, compile it, test it, and then continue to the next feature.

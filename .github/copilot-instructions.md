# Copilot Instructions — E-Commerce Product Management System

## 1. Project Context

This repository contains a full-stack e-commerce Product Management System.

The application consists of:

* ASP.NET Core Web API backend
* Angular frontend
* SQL Server database
* Entity Framework Core
* ASP.NET Core Identity
* JWT authentication
* Role-based authorization
* Automated testing
* Azure DevOps CI/CD
* Optional Docker and Azure deployment

The system manages:

* Products
* Categories
* Inventory
* Inventory transactions
* Users
* Roles

---

# 2. Critical Development Rule

Do NOT generate the entire application at once.

Development must happen incrementally.

For every requested feature:

1. Understand the existing architecture.
2. Inspect the existing code.
3. Identify affected projects/files.
4. Implement the smallest complete change.
5. Build the affected project.
6. Run relevant tests.
7. Fix compilation/test errors.
8. Report the files changed.
9. Only then move to the next feature.

Do not overwrite existing working code unnecessarily.

---

# 3. Architecture

Use a layered architecture.

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
       ▼
SQL Server
```

Responsibilities:

### ECommerce.Api

Contains:

* Controllers
* Middleware
* API configuration
* Dependency injection
* Authentication configuration
* Authorization
* Swagger

### ECommerce.Application

Contains:

* DTOs
* Interfaces
* Services
* Validators
* Business workflows

### ECommerce.Domain

Contains:

* Entities
* Enums
* Domain concepts
* Domain rules

### ECommerce.Infrastructure

Contains:

* EF Core
* DbContext
* Entity configurations
* Repositories
* Identity
* Database migrations

### ECommerce.Tests

Contains:

* Unit tests
* Integration tests

---

# 4. Dependency Direction

Follow these dependency rules:

```text
Api
 ↓
Application
 ↓
Domain

Infrastructure
 ↓
Application / Domain
```

Domain must remain independent from infrastructure.

Do not place EF Core-specific implementation inside the Domain project.

Do not place database access code inside controllers.

---

# 5. Backend Technology

Use:

* C#
* ASP.NET Core
* Entity Framework Core
* SQL Server
* ASP.NET Core Identity
* JWT
* Swagger/OpenAPI
* xUnit
* Moq

Use the .NET version selected when the solution is created. Keep all projects on the same target framework unless there is a documented reason not to.

Do not introduce a different framework version without explicit approval.

---

# 6. Entity Rules

Initial entities:

```text
Product
Category
Inventory
InventoryTransaction
ApplicationUser
RefreshToken
```

Use a common base entity where appropriate.

Example:

```csharp
public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
```

Audit properties should be used consistently.

---

# 7. Product Rules

Product fields include:

```text
ProductId
SKU
Name
Description
CategoryId
Price
CostPrice
Status
Brand
ImageUrl
IsDeleted
CreatedAt
UpdatedAt
CreatedBy
UpdatedBy
```

Rules:

* SKU must be unique.
* SKU is required.
* Product name is required.
* Price must be greater than zero.
* Category must exist.
* Products use soft deletion.
* Deleted products must not appear in normal listings.
* Do not physically delete products unless explicitly required.
* SKU should normally not change after creation.

---

# 8. Category Rules

Category fields:

```text
CategoryId
Name
Description
IsActive
CreatedAt
UpdatedAt
CreatedBy
UpdatedBy
```

Rules:

* Name is required.
* Category name should be unique.
* Products reference Category through CategoryId.
* Do not cascade-delete products automatically when deleting/deactivating categories.

---

# 9. Inventory Rules

Inventory fields:

```text
InventoryId
ProductId
QuantityAvailable
ReservedQuantity
ReorderLevel
LastUpdatedAt
```

Rules:

* Inventory quantity cannot become negative.
* Reserved quantity cannot be negative.
* Reorder level cannot be negative.
* Available quantity is:

QuantityAvailable - ReservedQuantity

Every inventory modification must create an InventoryTransaction.

The server must calculate:

```text
PreviousQuantity
NewQuantity
```

Do not trust the client to provide the final inventory quantity.

---

# 10. Inventory Transaction Rules

Transaction types:

```text
OpeningStock
Purchase
Sale
Return
Damage
Adjustment
```

Inventory transactions are append-only.

Do not update or delete historical inventory transactions.

Every transaction should record:

```text
ProductId
TransactionType
Quantity
PreviousQuantity
NewQuantity
ReferenceNumber
Remarks
CreatedAt
CreatedBy
```

---

# 11. Database Rules

Use SQL Server.

Use Entity Framework Core migrations.

Do not manually modify production database schema without a migration.

Use explicit EF Core configurations.

Prefer:

```text
IEntityTypeConfiguration<T>
```

over putting all database configuration in one huge DbContext file.

---

# 12. Database Indexes

Create appropriate indexes.

At minimum:

```text
Products.SKU
Products.CategoryId
Products.IsDeleted
Products.Status

Categories.Name

Inventory.ProductId

InventoryTransactions.ProductId
InventoryTransactions.CreatedAt
```

SKU must have a unique index.

---

# 13. Database Relationships

Required relationships:

```text
Category 1 ──── * Product

Product 1 ──── 1 Inventory

Product 1 ──── * InventoryTransaction
```

Configure relationships explicitly.

Avoid accidental cascade delete behavior.

---

# 14. DTO Rules

Never expose EF Core entities directly from API controllers.

Use DTOs.

Examples:

```text
CreateProductRequest
UpdateProductRequest
ProductResponse
ProductListResponse

CreateCategoryRequest
UpdateCategoryRequest
CategoryResponse

InventoryResponse
InventoryAdjustmentRequest
InventoryTransactionResponse
```

DTOs should contain only data required by the API contract.

---

# 15. Controller Rules

Controllers must remain thin.

Controllers should:

1. Receive HTTP request.
2. Validate basic request structure.
3. Call application service.
4. Return HTTP response.

Do not put business logic in controllers.

Bad:

```csharp
// Complex business logic inside controller
```

Good:

```csharp
var result = await _productService.CreateAsync(request);
return CreatedAtAction(...);
```

---

# 16. Service Rules

Business logic belongs in application services.

Examples:

```text
ProductService
CategoryService
InventoryService
AuthService
```

Services should:

* Validate business rules.
* Coordinate repositories/database operations.
* Map DTOs.
* Throw appropriate business exceptions.
* Return application results.

---

# 17. Repository Rules

Do not create repositories unnecessarily for every simple EF Core operation.

Use repositories when they provide meaningful abstraction or complex query behavior.

Avoid creating generic repositories merely because they are common in tutorials.

EF Core DbContext may be used directly within the Infrastructure/Application design when appropriate.

---

# 18. API Naming

Use RESTful naming.

Correct:

```http
GET    /api/products
GET    /api/products/{id}
POST   /api/products
PUT    /api/products/{id}
DELETE /api/products/{id}
```

Avoid:

```text
/getProducts
/createProduct
/updateProduct
/deleteProduct
```

---

# 19. API Pagination

Product list APIs must support pagination.

Use:

```text
pageNumber
pageSize
```

Example:

```http
GET /api/products?pageNumber=1&pageSize=20
```

Maximum page size should be limited to prevent excessive database queries.

---

# 20. API Filtering

Product listing should support:

```text
search
categoryId
status
minPrice
maxPrice
sortBy
sortOrder
pageNumber
pageSize
```

Filtering and sorting must be performed at the database level.

Do not load the entire product table into memory and then filter it.

---

# 21. EF Core Query Rules

Prefer:

```csharp
AsNoTracking()
```

for read-only queries.

Use projection:

```csharp
Select(...)
```

when returning DTOs.

Avoid unnecessary:

```csharp
Include(...)
```

when projection can solve the problem.

Do not retrieve unnecessary columns.

---

# 22. Async Rules

Database and API operations must use async APIs.

Prefer:

```csharp
await dbContext.Products.ToListAsync();
```

instead of:

```csharp
dbContext.Products.ToList();
```

Do not use `.Result` or `.Wait()` in asynchronous application code.

---

# 23. Validation

Server-side validation is mandatory.

Examples:

```text
SKU required
Name required
CategoryId required
Price > 0
Quantity >= 0
ReorderLevel >= 0
```

Client-side Angular validation does not replace server validation.

---

# 24. Error Handling

Use centralized exception handling middleware.

Do not write repetitive try/catch blocks in every controller.

Expected API responses:

```text
400 Bad Request
401 Unauthorized
403 Forbidden
404 Not Found
409 Conflict
500 Internal Server Error
```

Do not expose stack traces or sensitive exception details to production clients.

---

# 25. Authentication

Use ASP.NET Core Identity.

Never implement password hashing manually.

Use JWT authentication for API access.

Authentication flow:

```text
Login
 ↓
Validate credentials
 ↓
Generate JWT
 ↓
Return token
 ↓
Angular stores token securely
 ↓
HTTP interceptor adds Bearer token
 ↓
API validates JWT
```

---

# 26. Authorization

Roles:

```text
Admin
User
```

Admin-only operations:

```text
Create Product
Update Product
Delete Product
Create Category
Update Category
Delete Category
Adjust Inventory
```

Authenticated users can:

```text
View Products
View Product Details
View Categories
```

Use:

```csharp
[Authorize(Roles = "Admin")]
```

where appropriate.

Do not rely only on Angular route guards for security.

The API must enforce authorization.

---

# 27. Security

Never:

* Store plain-text passwords.
* Commit secrets.
* Commit JWT signing keys.
* Commit production connection strings.
* Trust client-provided authorization claims.
* Trust client-provided inventory final quantities.

Use configuration/environment variables/user secrets for development secrets.

---

# 28. Logging

Use structured logging.

Important events include:

```text
Authentication attempts
Authentication failures
Product creation
Product update
Product deletion
Inventory adjustment
Unhandled exceptions
Important business errors
```

Do not log:

```text
Passwords
JWT tokens
Refresh tokens
Sensitive personal data
```

---

# 29. Angular Architecture

Use feature-based architecture:

```text
core/
shared/
features/
```

Core contains:

```text
Authentication
Guards
Interceptors
Global services
Models
```

Features contain business functionality:

```text
auth
dashboard
products
categories
inventory
```

Shared contains reusable UI components, pipes, and directives.

---

# 30. Angular Rules

Use:

* TypeScript strict mode where supported.
* Reactive Forms.
* Angular Router.
* HttpClient.
* Route guards.
* HTTP interceptors.
* Lazy loading for major features where appropriate.

Avoid putting business logic directly inside templates.

Keep components focused on presentation and user interaction.

Use services for API communication.

---

# 31. Angular API Services

Examples:

```text
AuthService
ProductService
CategoryService
InventoryService
```

Do not call HTTP APIs directly from multiple components.

Use services.

---

# 32. Angular Authentication

Use:

```text
AuthService
AuthGuard
RoleGuard
AuthInterceptor
```

Authentication flow:

```text
Login Component
       ↓
AuthService
       ↓
POST /api/auth/login
       ↓
JWT
       ↓
Auth State
       ↓
AuthInterceptor
       ↓
Authorization Header
```

---

# 33. Angular Error Handling

Handle:

```text
401
403
404
409
500
```

appropriately.

For example:

```text
401 → redirect to login
403 → display access denied
404 → display not found
409 → display business conflict
500 → display generic server error
```

Do not expose raw backend stack traces.

---

# 34. UI Requirements

Product management should support:

```text
Search
Pagination
Sorting
Filtering
Create
Edit
View
Soft Delete
```

Product form should validate:

```text
Name
SKU
Category
Price
Cost Price
Initial Quantity
Reorder Level
```

---

# 35. Testing Rules

Every important business rule should have automated tests.

Minimum tests:

```text
Product creation
Duplicate SKU
Invalid category
Product update
Product soft delete
Product search
Product pagination

Inventory increase
Inventory decrease
Negative stock prevention
Inventory transaction creation

Admin authorization
User authorization
Authentication failure
```

Tests must be deterministic.

Do not write tests that depend on external services unless they are integration tests specifically designed for that purpose.

---

# 36. Code Quality

Follow:

* SOLID principles.
* Clean Code principles.
* Meaningful naming.
* Small methods.
* Single responsibility.
* Dependency injection.
* Nullable reference types.
* Async programming.

Avoid:

```text
God classes
God methods
Duplicated business logic
Magic strings
Magic numbers
Deeply nested conditionals
Unused code
Dead code
```

---

# 37. Naming Conventions

C#:

```text
PascalCase → Classes, Methods, Properties
camelCase → local variables, parameters
_interfaceName → Interfaces
```

Examples:

```text
ProductService
IProductService
CreateProductAsync
productId
```

Angular:

```text
product-list.component.ts
product.service.ts
auth.guard.ts
auth.interceptor.ts
```

---

# 38. Configuration

Use:

```text
appsettings.json
appsettings.Development.json
```

Do not store production secrets in source control.

Development secrets should use appropriate local secret storage.

Production secrets should come from environment/configuration management.

---

# 39. Swagger

Swagger must be enabled for API development.

Document:

* Endpoints
* Request models
* Response models
* Authorization requirements
* Important error responses

JWT authorization should be configured in Swagger.

---

# 40. Git Rules

Use feature branches.

Example:

```text
feature/product-crud
feature/jwt-authentication
feature/inventory
feature/angular-products
```

Do not directly develop large features on main.

Commit messages should be meaningful.

Examples:

```text
feat: add product CRUD APIs
feat: add JWT authentication
feat: add inventory adjustment
fix: prevent negative inventory
test: add product service tests
```

---

# 41. Pull Request Rules

Before merging:

```text
Build succeeds
Tests pass
No compiler warnings introduced unnecessarily
No secrets
API behavior documented
Architecture followed
```

---

# 42. Database Migration Rules

When entities change:

1. Update entity.
2. Update EF configuration.
3. Create migration.
4. Review migration.
5. Apply migration locally.
6. Test application.
7. Commit migration files.

Do not delete migrations simply to hide problems.

---

# 43. Seed Data

Development seed data should include:

### Roles

```text
Admin
User
```

### Admin

```text
admin@shop.local
```

### User

```text
user@shop.local
```

Use a documented development password only for local development.

Never use development seed passwords in production.

Seed sample:

```text
Electronics
Clothing
Books
Home Appliances
```

and several sample products.

---

# 44. Transaction Rules

Inventory modification and inventory transaction creation should happen atomically.

Conceptually:

```text
Begin Transaction
      ↓
Validate inventory
      ↓
Update Inventory
      ↓
Insert InventoryTransaction
      ↓
Commit
```

If any operation fails:

```text
Rollback
```

The inventory update and audit transaction must never become inconsistent.

---

# 45. Performance Rules

Avoid:

```text
SELECT *
```

when unnecessary.

Avoid loading thousands of records into memory.

Use:

```text
Pagination
Projection
AsNoTracking
Database filtering
Database sorting
Indexes
```

Use appropriate asynchronous database operations.

---

# 46. API Documentation

Every new endpoint should have:

```text
HTTP method
Route
Purpose
Request
Response
Authorization requirement
Possible errors
```

Swagger should remain functional.

---

# 47. Docker

Docker is optional during the initial development phase.

When Docker is introduced, provide:

```text
Dockerfile.backend
Dockerfile.frontend
docker-compose.yml
```

Do not introduce Docker complexity before the local application works.

---

# 48. Azure DevOps

CI pipeline should eventually:

```text
Restore
Build
Test
Publish
```

CD pipeline should eventually:

```text
Deploy DEV
Approval
Deploy PROD
```

Pipeline secrets must be stored in secure Azure DevOps mechanisms.

Never commit credentials into YAML.

---

# 49. Azure

Potential deployment architecture:

```text
Angular
   │
   ▼
Azure App Service / Static Web App

ASP.NET Core
   │
   ▼
Azure App Service

SQL Server
   │
   ▼
Azure SQL Database
```

Azure deployment should be implemented only after local functionality and CI are stable.

---

# 50. Do Not Over-Engineer

This is an important rule.

Do NOT introduce:

* Microservices
* Event sourcing
* CQRS
* Kafka
* RabbitMQ
* Kubernetes
* Redis
* Elasticsearch

during the initial MVP unless specifically requested.

Start with a modular monolith.

The architecture should allow these technologies to be introduced later if the business requires them.

---

# 51. Implementation Order

Always follow this sequence unless explicitly instructed otherwise:

```text
1. Repository setup

2. .NET solution

3. Domain entities

4. EF Core DbContext

5. SQL Server configuration

6. EF Core configurations

7. Initial migration

8. Database seed

9. ASP.NET Identity

10. JWT authentication

11. Role authorization

12. Category API

13. Product API

14. Inventory API

15. Backend tests

16. Angular application

17. Angular authentication

18. Angular routing/guards

19. Product UI

20. Category UI

21. Inventory UI

22. Angular tests

23. Docker

24. Azure DevOps CI

25. Azure DevOps CD

26. Azure deployment
```

---

# 52. Copilot Agent Behavior

Before modifying code:

* Inspect the repository.
* Inspect relevant existing files.
* Respect existing architecture.
* Do not recreate existing files unnecessarily.
* Do not change unrelated files.

When implementing:

* Explain what will be changed.
* Make focused changes.
* Compile/build.
* Run relevant tests.
* Fix errors.
* Summarize changes.

When requirements are ambiguous:

* Prefer the architecture defined in this document.
* Do not invent large features.
* Ask for clarification only when the ambiguity prevents safe implementation.

---

# 53. Definition of Done

A feature is complete only when:

```text
Code compiles
+
Tests pass
+
Validation exists
+
Authorization exists where required
+
Error handling exists
+
Logging exists where appropriate
+
Swagger is updated
+
Database migration exists when required
+
Angular integration works where applicable
+
No secrets are committed
```

---

# 54. Final Principle

Build this project like a real enterprise application.

Prioritize:

```text
Correctness
Security
Maintainability
Testability
Performance
Readability
Separation of concerns
Clean API design
```

Do not optimize for the smallest amount of code.

Do not generate code simply to satisfy a request if it violates the architecture.

Always prefer a clear, maintainable implementation that can be explained in a technical interview.

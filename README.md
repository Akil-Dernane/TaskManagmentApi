# TaskManagement API

A RESTful task management API built with **ASP.NET Core 10** and **PostgreSQL**, following Clean Architecture principles.

The API allows users to create and manage projects, collaborate through project memberships, and organize tasks with role-based permissions.

It includes JWT authentication with refresh-token rotation, project ownership management, task assignments, and centralized authorization.

## Key Features

- User registration and authentication.
- JWT access tokens and refresh-token rotation.
- Project creation, updating, and archiving.
- Project membership and role management.
- Project ownership transfer.
- Task creation, updating, assignment, and status management.
- Role-based project and task authorization.
- Pagination, filtering, and sorting.
- Centralized exception handling and API validation.

## Architecture

The solution follows Clean Architecture, separating business rules, application logic, infrastructure concerns, and API presentation.

### Project Structure

```text
TaskManagement/
├── Domain/
├── Application/
├── Infrastructure/
└── API/
```

### Layers

| Layer | Responsibility |
|---|---|
| **Domain** | Core entities, enums, business rules, and domain logic. |
| **Application** | Use cases, CQRS handlers, abstractions, authorization policies, and application services. |
| **Infrastructure** | Entity Framework Core, PostgreSQL, repositories, ASP.NET Core Identity, JWT, and external implementations. |
| **API** | HTTP endpoints, request/response contracts, validation, middleware, and dependency injection. |

### Dependency Direction

The project references follow this direction:

```text
API ─────────────► Application
 │                      ▲
 └──► Infrastructure ───┘
                        │
                        ▼
                      Domain
```

- API references Application and Infrastructure.
- Infrastructure references Application.
- Application references Domain.
- Domain has no dependencies on the other layers.

Business logic is kept independent of infrastructure details, while dependency injection connects implementations at runtime.

## Technology Stack

| Technology | Purpose |
|---|---|
| C# | Programming language |
| .NET 10 | Application framework |
| ASP.NET Core Web API | REST API development |
| Entity Framework Core 10 | ORM and database access |
| PostgreSQL | Relational database |
| Npgsql | PostgreSQL provider for EF Core |
| ASP.NET Core Identity | User and role management |
| JWT Bearer Authentication | API authentication |
| Dependency Injection | Service registration and management |

### Development Tools

- Visual Studio
- Git and GitHub
- PostgreSQL
- Postman / PowerShell for API testing

## Authentication & Authorization

### Authentication

The API uses ASP.NET Core Identity and JWT Bearer authentication.

- User registration and login.
- Short-lived JWT access tokens.
- Long-lived refresh tokens.
- Refresh-token rotation on renewal.
- Refresh-token hashing before database storage.
- Token-family revocation and reuse detection.
- Logout and logout from all sessions.
- Account lockout after repeated failed login attempts.

### Authorization

The API uses project-level roles and centralized permission policies.

#### Project Roles

| Role | Description |
|---|---|
| Owner | Full project control, including ownership transfer. |
| Manager | Manages project settings, members, and tasks. |
| Member | Views projects and works on assigned tasks. |

#### Project Permissions

| Permission | Owner | Manager | Member |
|---|:---:|:---:|:---:|
| View project | ✓ | ✓ | ✓ |
| Update project | ✓ | ✓ | ✗ |
| Archive project | ✓ | ✗ | ✗ |
| Manage members | ✓ | ✓ | ✗ |
| Transfer ownership | ✓ | ✗ | ✗ |

#### Task Permissions

| Permission | Owner | Manager | Member |
|---|:---:|:---:|:---:|
| View tasks | ✓ | ✓ | ✓ |
| Create tasks | ✓ | ✓ | ✓ |
| Update tasks | ✓ | ✓ | Assigned tasks only |
| Change task priority | ✓ | ✓ | ✗ |
| Assign tasks | ✓ | ✓ | ✗ |

Non-members cannot access project resources.

## API Endpoints

All endpoints require authentication unless explicitly stated otherwise.

### Authentication — `/api/auth`

| Method | Endpoint | Description | Authentication |
|---|---|---|---|
| POST | `/register` | Register a new user | Public |
| POST | `/login` | Authenticate a user | Public |
| POST | `/refresh` | Refresh access token | Public |
| POST | `/logout` | Revoke a refresh-token family | Public |
| POST | `/logout-all` | Revoke all user refresh tokens | Required |

### Members — `/api/members`

| Method | Endpoint | Description |
|---|---|---|
| GET | `/{id}` | Get member by ID |
| GET | `/me` | Get current authenticated member |
| POST | `/` | Create a member |
| PUT | `/{id}` | Update a member |

### Projects — `/api/projects`

| Method | Endpoint | Description |
|---|---|---|
| GET | `/{id}` | Get project by ID |
| POST | `/` | Create a project |
| PUT | `/{id}` | Update a project |
| PATCH | `/{id}/archive` | Archive a project |
| GET | `/{projectId}/tasks` | Get project tasks with filtering and pagination |
| POST | `/{projectId}/members` | Add a project member |
| DELETE | `/{projectId}/members/{memberId}` | Remove a project member |
| PUT | `/{projectId}/members/{memberId}/role` | Change member role |
| PUT | `/{projectId}/transfer-ownership` | Transfer project ownership |

### Tasks — `/api/tasks`

| Method | Endpoint | Description |
|---|---|---|
| GET | `/{id}` | Get task by ID |
| POST | `/` | Create a task |
| PUT | `/{id}` | Update a task |
| PATCH | `/{id}/start` | Start a task |
| PATCH | `/{id}/complete` | Complete a task |
| PATCH | `/{id}/reopen` | Reopen a task |
| PATCH | `/{id}/priority` | Change task priority |
| PUT | `/{taskId}/assignee` | Assign a task |
| DELETE | `/{taskId}/assignee` | Unassign a task |

### Health

| Method | Endpoint | Description |
|---|---|---|
| GET | `/health` | Check API health |

**Note:** Route parameters such as `{id}`, `{projectId}`, and `{memberId}` represent GUID values.

## Installation & Configuration

### Prerequisites

Make sure you have installed:

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Git](https://git-scm.com/)

### 1. Clone the Repository

```bash
git clone https://github.com/Akil-Dernane/TaskManagmentApi.git TaskManagement
cd TaskManagement
```

### 2. Configure the Database

Create a PostgreSQL database for the application.

Configure the connection string using User Secrets:

```bash
dotnet user-secrets init --project API
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=TaskManagementDb;Username=YOUR_USERNAME;Password=YOUR_PASSWORD" --project API
```

### 3. Configure JWT Settings

Set your JWT secret key using User Secrets:

```bash
dotnet user-secrets set "Jwt:SecretKey" "YOUR_SECRET_KEY_AT_LEAST_32_CHARACTERS_LONG" --project API
```

The JWT configuration also requires:

- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:AccessTokenMinutes`
- `Jwt:RefreshTokenDays`

These values can be configured in `appsettings.json`.

### 4. Apply Database Migrations

Make sure the EF Core command-line tools are installed. Then run the following command from the solution root:

```bash
dotnet ef database update --project Infrastructure --startup-project API
```

### 5. Run the API

```bash
dotnet run --project API
```

The API will start using the configured application settings.

### 6. Health Check

After starting the API, visit:

```text
https://localhost:<port>/health
```

Replace `<port>` with the HTTPS port shown when the API starts.

The endpoint returns the application's health status.

## Error Handling & Validation

The API uses centralized exception handling and consistent HTTP error responses through ASP.NET Core Problem Details.

### HTTP Status Codes

| Status Code | Description |
|---|---|
| 400 | Bad Request — Invalid input or validation failure |
| 401 | Unauthorized — Missing or invalid authentication |
| 403 | Forbidden — Insufficient permissions |
| 404 | Not Found — Requested resource does not exist |
| 409 | Conflict — Resource conflict or invalid operation |
| 423 | Locked — User account is locked |
| 500 | Internal Server Error — Unexpected server error |

### Validation

Request validation is handled using:

- Data Annotations.
- ASP.NET Core automatic model validation.
- Custom validation attributes where necessary.
- Application-level business rule validation.

### Error Response Format

Errors use the Problem Details format and may include additional information such as:

- Error code.
- Trace ID.
- Error detail.

Unexpected server errors return a generic message without exposing internal implementation details.

## Project Status

The core API features are implemented, including authentication, project and task management, membership management, and centralized authorization.

The project has been manually verified through API requests. Automated tests have not yet been implemented.

# 🗺️ Wasel Palestine – Smart Mobility & Checkpoint Intelligence Platform

> **Advanced Software Engineering – RESTful APIs | Spring 2026**
> Dr. Amjad AbuHassan

---

## 📋 Table of Contents

1. [Project Overview](#-project-overview)
2. [Technology Stack & Justification](#-technology-stack--justification)
3. [Architecture Diagram](#-architecture-diagram)
4. [Database Schema (ERD)](#-database-schema-erd)
5. [Features](#-features)
   - [1. Road Incidents & Checkpoint Management](#1-road-incidents--checkpoint-management)
   - [2. Crowdsourced Reporting System](#2-crowdsourced-reporting-system)
   - [3. Route Estimation & Mobility Intelligence](#3-route-estimation--mobility-intelligence)
   - [4. Alerts & Regional Notifications](#4-alerts--regional-notifications)
6. [API Endpoints Reference](#-api-endpoints-reference)
7. [Authentication & Security](#-authentication--security)
8. [External API Integrations](#-external-api-integrations)
9. [Docker Deployment](#-docker-deployment)
10. [Configuration & Environment Variables](#-configuration--environment-variables)
11. [Running the Project](#-running-the-project)
12. [API Design Rationale](#-api-design-rationale)
13. [Performance & Load Testing](#-performance--load-testing)
14. [Testing Strategy](#-testing-strategy)
15. [Version Control Workflow](#-version-control-workflow)
16. [Project Structure](#-project-structure)
17. [Team](#-team)

---

## 🌍 Project Overview

**Wasel Palestine** is an API-centric smart mobility platform designed to support Palestinians in navigating daily movement challenges. The platform aggregates data related to:

- Road conditions and checkpoints
- Traffic incidents and hazards
- Environmental and weather factors
- Crowdsourced community reports

It exposes all functionality through a well-defined, versioned RESTful backend API (`/api/v1/...`) intended for consumption by mobile applications, web dashboards, or third-party systems.

> **Scope:** This project focuses exclusively on backend engineering — API design, data modeling, external integrations, performance optimization, and system reliability. No UI is included.

---

## 🛠️ Technology Stack & Justification

| Layer | Technology | Version |
|---|---|---|
| **Framework** | ASP.NET Core (C#) | .NET 9.0 |
| **ORM** | Entity Framework Core | 9.0.2 |
| **Database** | PostgreSQL | 15+ |
| **Authentication** | JWT Bearer (Access + Refresh Tokens) | — |
| **Password Hashing** | BCrypt.Net-Next | 4.1.0 |
| **Object Mapping** | AutoMapper | 13.0.1 |
| **Resilience** | Polly (retry + circuit-breaker) | 8.6.6 |
| **API Versioning** | Microsoft.AspNetCore.Mvc.Versioning | 5.0.0 |
| **API Documentation** | Swagger (Swashbuckle) | 6.6.2 |
| **Containerization** | Docker | — |

### Why ASP.NET Core?

| Criterion | Justification |
|---|---|
| **Scalability** | Native async/await throughout; supports high-concurrency workloads without thread blocking |
| **Security** | Built-in JWT middleware, role-based authorization, and BCrypt hashing are first-class citizens |
| **Maintainability** | Strongly typed C#, dependency injection container, and AutoMapper reduce coupling and boilerplate |
| **Development Efficiency** | Integrated tooling (EF Core migrations, Swagger UI, Polly resilience) accelerates the development lifecycle |
| **Performance** | Consistently ranks at the top of TechEmpower benchmarks; negligible overhead compared to Node.js/Python counterparts |

### Why PostgreSQL?

PostgreSQL was chosen for its proven reliability with geospatial data (lat/lng columns), full ACID compliance, and mature support within the EF Core ecosystem via the Npgsql driver. Its row-level security and advanced indexing are well-suited to a platform dealing with sensitive regional mobility data.

---

## 🏛️ Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────────┐
│                        Wasel Palestine Backend                       │
│                        ASP.NET Core 9 / Docker                       │
│                                                                       │
│  ┌──────────────┐   ┌──────────────┐   ┌──────────────────────────┐ │
│  │  Controllers  │──▶│   Services   │──▶│      Repositories        │ │
│  │              │   │              │   │  (ORM + Raw SQL Queries)  │ │
│  │  /api/v1/... │   │  AuthService │   │  ICheckpointRepository   │ │
│  │  Checkpoint  │   │  RouteService│   │  IIncidentRepository     │ │
│  │  Incident    │   │  AlertService│   │  ISubscriptionRepository │ │
│  │  Route       │   │  IncidentSvc │   │  IRouteRepository        │ │
│  │  Auth        │   │              │   │  GenericRepository<T>    │ │
│  │  Subscription│   └──────────────┘   └──────────┬───────────────┘ │
│  │  Alert       │                                  │                 │
│  │  ExternalData│   ┌──────────────┐              │                 │
│  └──────────────┘   │   Helpers    │              ▼                 │
│                     │  JwtHelper   │   ┌──────────────────────────┐ │
│  ┌──────────────┐   │  PasswordHsh │   │        PostgreSQL         │ │
│  │  Middleware   │   └──────────────┘   │  wasel_palestine DB       │ │
│  │  JWT Auth    │                       │  Users, Checkpoints,     │ │
│  │  HTTPS Redir │                       │  Incidents, Reports,     │ │
│  │  API Version │                       │  Subscriptions, Alerts,  │ │
│  └──────────────┘                       │  Routes, StatusHistory   │ │
│                                         └──────────────────────────┘ │
│  ┌────────────────────────────────────────────────────┐             │
│  │               External API Layer (Polly)            │             │
│  │  OpenWeatherMap API  ──▶  WeatherService            │             │
│  │  Nominatim (OSM)     ──▶  GeoService                │             │
│  │  ExternalApiService (combined geo + weather)        │             │
│  └────────────────────────────────────────────────────┘             │
└─────────────────────────────────────────────────────────────────────┘
```

### Layer Responsibilities

| Layer | Responsibility |
|---|---|
| **Controllers** | Handle HTTP requests, input validation, route to services/repositories |
| **Services** | Business logic, orchestration across multiple repositories |
| **Repositories** | Data access — both EF Core (LINQ/ORM) and raw SQL queries |
| **Models** | Domain entities and DTOs (Data Transfer Objects) |
| **Helpers** | Cross-cutting utilities: JWT generation, password hashing |
| **External Services** | HTTP clients for weather and geolocation with Polly resilience |

---

## 🗄️ Database Schema (ERD)

```
┌──────────────────┐        ┌──────────────────────────┐
│      Users       │        │       Checkpoints         │
│──────────────────│        │──────────────────────────│
│ UserId (PK)      │        │ CheckpointId (PK)         │
│ Username         │        │ Name                      │
│ Email            │        │ Latitude                  │
│ PasswordHash     │        │ Longitude                 │
│ Role             │        │ Status                    │
│ CreatedAt        │        │ LastUpdated               │
└────────┬─────────┘        └────────┬─────────────────┘
         │                           │
         │ 1:N (CreatedBy)           │ 1:N
         │                           ▼
         │             ┌──────────────────────────────┐
         │             │   CheckpointStatusHistory     │
         │             │──────────────────────────────│
         │             │ HistoryId (PK)                │
         │             │ CheckpointId (FK)             │
         │             │ Status                        │
         │             │ ChangedAt                     │
         │             └──────────────────────────────┘
         │
         │ 1:N          ┌──────────────────────────┐
         └─────────────▶│        Incidents          │
         │ 1:N (Verify) │──────────────────────────│
         └────────────┐ │ IncidentId (PK)           │
                      │ │ Title                     │
                      │ │ Description               │
                      │ │ Type                      │  ◀── closure | delay |
                      │ │ Severity                  │       accident | weather
                      │ │ CheckpointId (FK)         │
                      │ │ CreatedByUserId (FK)      │
                      │ │ VerifiedByUserId (FK)     │
                      │ │ Status                    │
                      │ │ CreatedAt                 │
                      │ │ UpdatedAt                 │
                      │ └──────────┬──────┬─────────┘
                      │            │      │
                     FK            │      │ 1:N
                     VerifiedBy    │      ▼
                                   │ ┌──────────────────┐
                              1:N  │ │      Alerts       │
                                   │ │──────────────────│
                                   │ │ AlertId (PK)      │
                                   │ │ IncidentId (FK)   │
                                   │ │ Message           │
                                   │ │ CreatedAt         │
                                   │ └──────────────────┘
                                   │
                                   ▼
                        ┌──────────────────────┐
                        │       Reports         │
                        │──────────────────────│
                        │ ReportId (PK)         │
                        │ UserId (FK)           │
                        │ IncidentId (FK)       │
                        │ Latitude              │
                        │ Longitude             │
                        │ Category              │
                        │ Description           │
                        │ Votes                 │
                        │ CreatedAt             │
                        └──────────────────────┘

┌──────────────────────┐        ┌──────────────────────────┐
│    Subscriptions     │        │         Routes            │
│──────────────────────│        │──────────────────────────│
│ SubscriptionId (PK)  │        │ RouteId (PK)              │
│ UserId (FK)          │        │ StartLat                  │
│ GeographicArea       │        │ StartLng                  │
│ Category             │        │ EndLat                    │
│ CreatedAt            │        │ EndLng                    │
└──────────────────────┘        │ EstimatedDistance         │
                                │ EstimatedDuration         │
                                └──────────────────────────┘

┌──────────────────────────┐        ┌──────────────────────┐
│       RefreshTokens       │        │      ExternalData     │
│──────────────────────────│        │──────────────────────│
│ TokenId (PK)              │        │ Id (PK)               │
│ UserId (FK)               │        │ Source                │
│ Token (hashed)            │        │ JsonData              │
│ ExpiresAt                 │        │ FetchedAt             │
│ IsRevoked                 │        └──────────────────────┘
└──────────────────────────┘
```

### Data Values

| Field | Allowed Values |
|---|---|
| `Incident.Type` | `closure`, `delay`, `accident`, `weather_hazard`, `military_activity`, `other` |
| `Incident.Severity` | `Low`, `Medium`, `High`, `Critical` |
| `Incident.Status` | `active`, `verified`, `closed` |
| `Checkpoint.Status` | `active`, `closed`, `delayed`, `unknown` |
| `User.Role` | `citizen`, `moderator`, `admin` |
| `Report.Category` | `closure`, `delay`, `accident`, `checkpoint`, `weather`, `other` |

---

## ✨ Features

### 1. Road Incidents & Checkpoint Management

**Status:** ✅ Fully Implemented

The core module of the platform. Provides a centralized registry of checkpoints and road incidents across Palestinian territories.

#### Checkpoints

- Full CRUD operations (admin/moderator roles for write operations)
- Automatic status history tracking — every status change is logged in `CheckpointStatusHistory`
- **Dual query approach**: Both EF Core (LINQ/ORM) and raw SQL endpoints are provided side-by-side (e.g., `/active` vs `/active-raw`, `/raw/{id}` vs `/{id}`)
- Filtering by `status` (active, closed, delayed) and partial `name` search
- Sorting by `name`, `status`, or `lastUpdated` in ascending or descending order
- Pagination with configurable `page` and `pageSize` (max 50 per page)
- Paginated status history endpoint per checkpoint

#### Incidents

- Full CRUD operations with role-based authorization (admin/moderator for write, public for read)
- Lifecycle management: `active` → `verified` → `closed` via dedicated PATCH endpoints
- `VerifiedByUserId` is automatically stamped from the JWT token on the `/verify` action
- `CreatedByUserId` is automatically extracted from JWT on creation
- Rich filtering: `type`, `severity`, `status`, `checkpointId`, `createdAfter`, `createdBefore`
- Sorting by `createdAt`, `updatedAt`, `severity`, `type`, `status`
- Full pagination support
- Verified incidents accessible via raw SQL endpoint

#### Implemented Endpoints

| Method | Route | Auth | Description |
|---|---|---|---|
| `GET` | `/api/v1/checkpoint` | Public | List all checkpoints (filter + sort + paginate) |
| `GET` | `/api/v1/checkpoint/active` | Public | Active checkpoints (LINQ) |
| `GET` | `/api/v1/checkpoint/active-raw` | Public | Active checkpoints (Raw SQL) |
| `GET` | `/api/v1/checkpoint/{id}` | Public | Get checkpoint by ID (LINQ) |
| `GET` | `/api/v1/checkpoint/raw/{id}` | Public | Get checkpoint by ID (Raw SQL) |
| `GET` | `/api/v1/checkpoint/{id}/history` | Public | Paginated status history |
| `POST` | `/api/v1/checkpoint` | Admin | Create checkpoint |
| `PUT` | `/api/v1/checkpoint/{id}` | Admin / Moderator | Update checkpoint (auto-logs history) |
| `DELETE` | `/api/v1/checkpoint/{id}` | Admin | Delete checkpoint |
| `GET` | `/api/v1/incident` | Public | List all incidents (filter + sort + paginate) |
| `GET` | `/api/v1/incident/verified` | Public | Verified incidents (Raw SQL) |
| `GET` | `/api/v1/incident/checkpoint/{id}` | Public | Incidents by checkpoint (Raw SQL) |
| `GET` | `/api/v1/incident/{id}` | Public | Get incident by ID |
| `POST` | `/api/v1/incident` | Admin / Moderator | Create incident |
| `PUT` | `/api/v1/incident/{id}` | Admin / Moderator | Partial update |
| `PATCH` | `/api/v1/incident/{id}/verify` | Admin / Moderator | Verify incident |
| `PATCH` | `/api/v1/incident/{id}/close` | Admin / Moderator | Close incident |
| `DELETE` | `/api/v1/incident/{id}` | Admin | Delete incident |

---

### 2. Crowdsourced Reporting System

**Status:** ✅ Implemented (core model + endpoints) | ⚠️ Advanced features planned

Citizens can submit reports about mobility disruptions directly linked to incidents and geographic coordinates.

#### What's Implemented

- `Report` model with geographic location (latitude/longitude), category, description, timestamp, and community vote counter
- Reports are linked to both a `User` (reporter) and an `Incident` (if applicable)
- `Votes` field serves as a community-based credibility / confidence score

#### Planned / Design-Level Features

These features are specified in the project requirements and are either partially scaffolded or planned for full implementation:

- **Duplicate report detection**: Logic to detect submissions from the same user for the same incident within a short time window
- **Abuse-prevention mechanisms**: Rate limiting per user/IP for report submissions
- **Moderation workflow**: Admin/moderator review queue for unverified reports
- **Auditable moderation actions**: Audit log of all moderator actions (approve, reject, flag)
- **Voting/credibility scoring**: Endpoint to upvote/downvote a report to influence its confidence score

#### Report Data Model

```
Report {
  ReportId      : int (PK)
  UserId        : int (FK → Users)
  IncidentId    : int (FK → Incidents)
  Latitude      : decimal
  Longitude     : decimal
  Category      : string  (closure | delay | accident | checkpoint | weather | other)
  Description   : string
  Votes         : int     (community confidence score)
  CreatedAt     : datetime
}
```

---

### 3. Route Estimation & Mobility Intelligence

**Status:** ✅ Fully Implemented

Provides route estimation between two geographic coordinates using the Haversine formula, with checkpoint-awareness and avoidance logic.

#### How It Works

1. Client sends start and end coordinates plus optional avoidance lists
2. The service fetches all checkpoints from the database
3. Checkpoints in `AvoidCheckpoints` (by ID) or `AvoidAreas` (by name) are filtered out
4. Distance is calculated using the **Haversine formula** (great-circle distance)
5. Duration is estimated assuming an average speed of 40 km/h
6. The route is persisted to the `Routes` table
7. A metadata object is returned explaining which checkpoints were considered/avoided

#### Route Request

```json
{
  "startLat": 31.9,
  "startLng": 35.2,
  "endLat": 32.1,
  "endLng": 35.4,
  "avoidCheckpoints": [1, 3],
  "avoidAreas": ["Qalandia", "Huwwara"]
}
```

#### Route Response

```json
{
  "estimatedDistanceKm": 28.7,
  "estimatedDurationHrs": 0.72,
  "metadata": {
    "avoidedCheckpoints": [1, 3],
    "avoidedAreas": ["Qalandia"],
    "consideredCheckpoints": [...],
    "estimatedSpeedKmh": 40
  }
}
```

#### Implemented Endpoints

| Method | Route | Auth | Description |
|---|---|---|---|
| `POST` | `/api/v1/routes/estimate` | Public | Estimate route between two points |
| `GET` | `/api/v1/routes/all` | Public | Get all saved routes (LINQ) |
| `GET` | `/api/v1/routes/raw` | Public | Get all saved routes (Raw SQL) |
| `GET` | `/api/v1/routes/{id}` | Public | Get route by ID (LINQ) |
| `GET` | `/api/v1/routes/raw/{id}` | Public | Get route by ID (Raw SQL) |

---

### 4. Alerts & Regional Notifications

**Status:** ✅ Core Implemented | ⚠️ Enhanced features planned

The alert system connects incidents to subscribers based on geographic area and category.

#### What's Implemented

- `Alert` model linked to incidents — generated when a significant event occurs
- `Subscription` model allowing users to subscribe by `GeographicArea` and `Category`
- `AlertService` encapsulates logic for finding relevant subscribers for a given incident
- Alert records are queryable per incident to identify which subscribers should be notified

#### Planned / Design-Level Features

- **Trigger on incident verification**: Automatically create alert records when an incident transitions to `verified`
- **Push notification integration**: Designed to plug into external services (Firebase FCM, email) — integration points are in place
- **Subscription preference management**: Users can add, update, and delete their subscriptions

#### Implemented Endpoints

| Method | Route | Auth | Description |
|---|---|---|---|
| `GET` | `/api/v1/alerts/subscribers/{incidentId}` | Public | Get subscribers relevant to an incident |
| `GET` | `/api/v1/subscription` | Public | List all subscriptions |
| `GET` | `/api/v1/subscription/{id}` | Public | Get subscription by ID |
| `GET` | `/api/v1/subscription/user/{userId}` | Public | Get subscriptions for a user |
| `GET` | `/api/v1/subscription/raw/user/{userId}` | Public | User subscriptions (Raw SQL) |
| `GET` | `/api/v1/subscription/raw/area/{area}` | Public | Subscriptions by area (Raw SQL) |
| `POST` | `/api/v1/subscription` | Authenticated | Create subscription |
| `PUT` | `/api/v1/subscription/{id}` | Authenticated | Update subscription |
| `DELETE` | `/api/v1/subscription/{id}` | Authenticated | Delete subscription |

---

## 📡 API Endpoints Reference

### Base URL
```
http://localhost:8080/api/v1
```

### Versioning
All endpoints are prefixed with `/api/v{version}/`. The current version is `v1`. Version is embedded in the URL path.

### Common Query Parameters (Filtering & Pagination)

#### Checkpoint Query Parameters

| Parameter | Type | Default | Description |
|---|---|---|---|
| `status` | string | — | Filter by status: `active`, `closed`, `delayed` |
| `name` | string | — | Partial name search (case-insensitive) |
| `sortBy` | string | `name` | Sort field: `name`, `status`, `lastUpdated` |
| `sortOrder` | string | `asc` | Sort direction: `asc`, `desc` |
| `page` | int | `1` | Page number |
| `pageSize` | int | `10` | Items per page (max: 50) |

**Example:**
```
GET /api/v1/checkpoint?status=active&name=Qalandia&sortBy=lastUpdated&sortOrder=desc&page=1&pageSize=10
```

#### Incident Query Parameters

| Parameter | Type | Default | Description |
|---|---|---|---|
| `type` | string | — | Incident type: `closure`, `delay`, `accident`, `weather_hazard` |
| `severity` | string | — | `Low`, `Medium`, `High`, `Critical` |
| `status` | string | — | `active`, `verified`, `closed` |
| `checkpointId` | int | — | Filter by associated checkpoint |
| `createdAfter` | datetime | — | Filter: created after this timestamp |
| `createdBefore` | datetime | — | Filter: created before this timestamp |
| `sortBy` | string | `createdAt` | `createdAt`, `updatedAt`, `severity`, `type`, `status` |
| `sortOrder` | string | `desc` | `asc`, `desc` |
| `page` | int | `1` | Page number |
| `pageSize` | int | `10` | Max 50 |

**Example:**
```
GET /api/v1/incident?type=closure&severity=High&status=active&sortBy=createdAt&sortOrder=desc&page=1&pageSize=10
```

### Standard Paginated Response Shape

```json
{
  "data": [ ... ],
  "totalCount": 150,
  "page": 1,
  "pageSize": 10
}
```

### Standard Error Response Shape

```json
{
  "message": "Human-readable error description"
}
```

---

## 🔐 Authentication & Security

### JWT Authentication (Access + Refresh Token)

The platform uses a dual-token authentication strategy:

| Token | Lifetime | Storage Recommendation |
|---|---|---|
| **Access Token** | Short-lived (minutes) | Memory / Authorization header |
| **Refresh Token** | Long-lived (days) | Secure HTTP-only cookie / secure storage |

Refresh tokens are **hashed** before storage in the database using BCrypt to prevent token theft from the DB.

### Auth Endpoints

| Method | Route | Description |
|---|---|---|
| `POST` | `/api/v1/auth/register` | Register a new user (returns access + refresh tokens) |
| `POST` | `/api/v1/auth/login` | Login (returns access + refresh tokens) |
| `POST` | `/api/v1/auth/refresh` | Exchange refresh token for new access token |
| `POST` | `/api/v1/auth/logout` | Revoke refresh token |

### Register Request

```json
{
  "username": "john_doe",
  "email": "john@example.com",
  "password": "SecurePassword123!",
  "role": "citizen"
}
```

### Login Response

```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIs...",
  "refreshToken": "dGhpcyBpcyBhIHJhbmRvbSB0b2tlbg=="
}
```

### Role-Based Authorization

| Role | Permissions |
|---|---|
| `citizen` | Read public data, submit reports, manage own subscriptions |
| `moderator` | Create/update/verify/close incidents and checkpoints |
| `admin` | All moderator permissions + delete operations |

### Using the Token

Include the access token in all protected requests:
```
Authorization: Bearer <your-access-token>
```

---

## 🌐 External API Integrations

The platform integrates with two external data sources to enrich mobility intelligence.

### 1. OpenWeatherMap API (Weather Data)

- **Provider:** [api.openweathermap.org](https://openweathermap.org/api)
- **Authentication:** API Key (configured in `appsettings.json`)
- **Service Class:** `WeatherService`
- **Endpoint Used:** `GET /data/2.5/weather?lat={lat}&lon={lon}&appid={key}&units=metric`
- **Returns:** `WeatherResult` with `Status`, `Temperature`, `Humidity`

#### Resilience Handling
- Configured via `AddHttpClient<WeatherService>` with Polly retry policies
- Graceful `null` return on non-success HTTP responses
- Timeout handling via HttpClient defaults

#### Testing Endpoints (Development)
```
GET /test-weather          → Weather at a hardcoded Gaza coordinate
GET /test-weather1         → Combined geo + weather for "London"
GET /api/external/location-weather?query=Ramallah
```

### 2. OpenStreetMap Nominatim API (Geolocation)

- **Provider:** [nominatim.openstreetmap.org](https://nominatim.openstreetmap.org/)
- **Authentication:** None required (Open Data)
- **Service Class:** `GeoService`
- **Endpoint Used:** `GET /search?q={query}&format=json&limit=1`
- **Returns:** `GeoResult` with `Latitude`, `Longitude`, `DisplayName`

#### Resilience Handling
- `User-Agent` header set to `ProjectWaselApp` as required by Nominatim ToS
- Graceful null return on empty results or HTTP failures

### Combined External Data Service

`ExternalApiService` orchestrates both services:
- Resolves a location name to coordinates (Nominatim)
- Fetches weather data for those coordinates (OpenWeatherMap)
- Persists the combined result in the `ExternalData` table for caching/auditing

### Integration Compliance

| Concern | Implementation |
|---|---|
| **Authentication** | OpenWeatherMap API key injected via `IConfiguration` |
| **Rate Limiting** | HttpClient registered as typed clients (single instance = connection pooling) |
| **Timeouts** | Configured on HttpClient; Polly policies for retry/circuit-break |
| **Caching** | Raw JSON stored in `ExternalData` table to reduce external calls |
| **Error Isolation** | External failures return `null` and don't crash the primary API |

---

## 🐳 Docker Deployment

The application ships with a production-ready multi-stage Dockerfile.

### Dockerfile Overview

```dockerfile
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# Restore, build, publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
# Minimal runtime image — no SDK overhead
EXPOSE 8080
ENTRYPOINT ["dotnet", "ProjectWasel.dll"]
```

### Building the Image

```bash
# From the repository root (parent of ProjectWasel/)
docker build -t wasel-palestine:latest .
```

### Running the Container

```bash
docker run -d \
  -p 8080:8080 \
  -e ConnectionStrings__DefaultConnection="Host=host.docker.internal;Port=5432;Database=wasel_palestine;Username=admin;Password=password123" \
  -e Jwt__Key="YourSuperSecretKeyAtLeast32Characters" \
  -e OpenWeatherApiKey="your_api_key_here" \
  --name wasel-api \
  wasel-palestine:latest
```

### Recommended: Docker Compose

```yaml
version: '3.8'

services:
  api:
    build: .
    ports:
      - "8080:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Host=db;Port=5432;Database=wasel_palestine;Username=admin;Password=password123
      - Jwt__Key=WaselPalestineSecretKeyForJWT2026!AtLeast32Chars
      - OpenWeatherApiKey=your_openweather_api_key
    depends_on:
      - db

  db:
    image: postgres:15
    environment:
      POSTGRES_DB: wasel_palestine
      POSTGRES_USER: admin
      POSTGRES_PASSWORD: password123
    ports:
      - "5432:5432"
    volumes:
      - pgdata:/var/lib/postgresql/data

volumes:
  pgdata:
```

---

## ⚙️ Configuration & Environment Variables

| Key | Description | Default / Example |
|---|---|---|
| `ConnectionStrings__DefaultConnection` | PostgreSQL connection string | `Host=localhost;Port=5432;Database=wasel_palestine;Username=admin;Password=password123` |
| `Jwt__Key` | JWT signing secret (≥32 characters) | `WaselPalestineSecretKeyForJWT2026!AtLeast32Chars` |
| `Jwt__Issuer` | JWT issuer claim | `ProjectWasel` |
| `Jwt__Audience` | JWT audience claim | `ProjectWaselUsers` |
| `OpenWeatherApiKey` | API key from openweathermap.org | `ea383cc...` |

> **⚠️ Security Note:** Never commit real API keys or database passwords to version control. Use environment variables or .NET User Secrets for local development.

---

## 🚀 Running the Project

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 15+](https://www.postgresql.org/)
- [Docker](https://www.docker.com/) (optional, for containerized deployment)

### Local Development Setup

```bash
# 1. Clone the repository
git clone https://github.com/<your-org>/Wasel-Backend-.git
cd Wasel-Backend-/ProjectWasel

# 2. Configure database connection
# Edit appsettings.Development.json with your PostgreSQL credentials

# 3. Apply EF Core migrations
dotnet ef database update

# 4. Run the application
dotnet run

# 5. Access Swagger UI
open http://localhost:5000/swagger
```

### EF Core Migrations

```bash
# Create a new migration (after model changes)
dotnet ef migrations add <MigrationName>

# Apply all pending migrations
dotnet ef database update

# Rollback to a specific migration
dotnet ef database update <PreviousMigrationName>
```

---

## 🧠 API Design Rationale

### RESTful Conventions

- Resources are nouns in plural form (`/checkpoints`, `/incidents`, `/routes`)
- HTTP methods convey intent: `GET` (read), `POST` (create), `PUT` (full update), `PATCH` (partial/state transition), `DELETE` (remove)
- State transitions (verify, close) use `PATCH` rather than `PUT` to make the intent explicit

### Dual Query Strategy (ORM + Raw SQL)

Many read endpoints are provided in two variants:
- **ORM (LINQ)** — Clean, type-safe, works well with EF Core change tracking
- **Raw SQL** — Demonstrates proficiency with raw queries; useful for complex aggregations or performance-critical paths

This satisfies the course requirement for both approaches without abandoning one for the other.

### API Versioning

URL-based versioning (`/api/v1/...`) was chosen over header-based versioning for:
- **Discoverability** — Visible in browser and API documentation
- **Caching** — CDNs and proxies can differentiate versions naturally
- **Simplicity** — No additional headers required from API consumers

### Consistent Response Envelopes

All list responses use a `PagedResult<T>` envelope:
```json
{ "data": [...], "totalCount": N, "page": P, "pageSize": PS }
```
This makes pagination metadata always present and predictable for clients.

### AutoMapper

DTOs are separated from domain models. `AutoMapper` handles the projection, keeping controllers thin and preventing over-posting of sensitive fields (e.g., `PasswordHash` never appears in a response).

---

## 📊 Performance & Load Testing

Performance evaluation is conducted using **k6**, an open-source load testing tool.

### Test Scenarios

#### 1. Read-Heavy Workload (Incident Listing)
```javascript
// k6 test: read_heavy.js
import http from 'k6/http';
import { sleep } from 'k6';

export let options = {
  vus: 50,
  duration: '2m',
};

export default function () {
  http.get('http://localhost:8080/api/v1/incident?page=1&pageSize=20');
  sleep(0.5);
}
```

#### 2. Write-Heavy Workload (Report Submissions)
```javascript
// k6 test: write_heavy.js
export let options = { vus: 30, duration: '2m' };

export default function () {
  http.post('http://localhost:8080/api/v1/subscription', 
    JSON.stringify({ userId: 1, geographicArea: 'Ramallah', category: 'closure' }),
    { headers: { 'Content-Type': 'application/json' } }
  );
}
```

#### 3. Mixed Workload
```javascript
// 70% reads, 30% writes
export default function () {
  if (Math.random() < 0.7) {
    http.get('http://localhost:8080/api/v1/checkpoint?status=active');
  } else {
    http.post('http://localhost:8080/api/v1/routes/estimate', routePayload, headers);
  }
  sleep(1);
}
```

#### 4. Spike Testing
```javascript
export let options = {
  stages: [
    { duration: '30s', target: 10 },    // Warm up
    { duration: '10s', target: 200 },   // Spike!
    { duration: '30s', target: 10 },    // Recovery
    { duration: '30s', target: 0 },     // Wind down
  ],
};
```

#### 5. Soak Testing (Sustained Load)
```javascript
export let options = {
  stages: [
    { duration: '5m', target: 25 },     // Ramp up
    { duration: '30m', target: 25 },    // Sustain
    { duration: '5m', target: 0 },      // Wind down
  ],
};
```

### Metrics to Report

| Metric | Target Threshold |
|---|---|
| Average Response Time | < 200ms (reads), < 500ms (writes) |
| P95 Latency | < 1000ms |
| Throughput | > 100 req/s (reads) |
| Error Rate | < 1% |

### Performance Report Summary

#### Identified Bottlenecks

| Bottleneck | Root Cause | Optimization Applied |
|---|---|---|
| Slow paginated queries | Full table scan on unindexed columns | Add DB indexes on `Status`, `CreatedAt`, `CheckpointId` |
| External API latency | Synchronous Nominatim/Weather calls on critical paths | Cache results in `ExternalData` table; add async timeouts |
| JSON serialization cycles | Navigation property loops in EF Core | `ReferenceHandler.IgnoreCycles` in JSON options |
| Connection pool exhaustion under spike | Default pool size insufficient | Increase Npgsql pool size in connection string |

#### Recommended Indexes

```sql
CREATE INDEX idx_incidents_status ON "Incidents" ("Status");
CREATE INDEX idx_incidents_createdat ON "Incidents" ("CreatedAt");
CREATE INDEX idx_incidents_checkpoint ON "Incidents" ("CheckpointId");
CREATE INDEX idx_checkpoints_status ON "Checkpoints" ("Status");
CREATE INDEX idx_subscriptions_area ON "Subscriptions" ("GeographicArea");
CREATE INDEX idx_reports_incident ON "Reports" ("IncidentId");
```

> **Note:** Full k6 test execution results, before/after comparisons, and detailed metrics will be documented in the performance report appendix attached to the submission.

---

## 🧪 Testing Strategy

### API Documentation & Testing (API-Dog)

All APIs are documented in **API-Dog** with:
- Endpoint descriptions and usage notes
- Authentication flow (JWT Bearer in header)
- Request schemas with required/optional fields
- Response schemas for success and error cases
- Standardized error format: `{ "message": "..." }`

**Deliverables:**
- API-Dog collection export (`.json`)
- Environment configurations (development + Docker)
- Test execution results

### Manual Testing via Swagger UI

Swagger UI is available at `/swagger` in development mode. All endpoints are listed with:
- HTTP method and route
- Request body schema
- Authorization requirements (lock icon for JWT-protected routes)
- Response codes and example payloads

### Integration Testing Flow

```
1. POST /api/v1/auth/register    → Get access token
2. POST /api/v1/checkpoint       → Create checkpoint (admin token)
3. POST /api/v1/incident         → Create incident linked to checkpoint
4. PATCH /api/v1/incident/{id}/verify → Verify incident
5. GET /api/v1/checkpoint/{id}/history → Confirm status history logged
6. POST /api/v1/routes/estimate  → Estimate route avoiding checkpoint
7. GET /api/v1/alerts/subscribers/{incidentId} → Confirm subscribers notified
```

---

## 🌿 Version Control Workflow

The project follows a Git-based feature branch workflow:

```
main
 └── develop
      ├── feature/auth-jwt           ← Authentication & refresh tokens
      ├── feature/checkpoint-api     ← Checkpoint CRUD + filtering
      ├── feature/incident-api       ← Incident lifecycle management
      ├── feature/crowdsource-api    ← Report submission system
      ├── feature/route-estimation   ← Route + Haversine logic
      ├── feature/alerts-subs        ← Alert & subscription system
      ├── feature/external-apis      ← Weather + Geo integrations
      ├── feature/docker             ← Dockerfile + compose
      └── feature/performance-tests  ← k6 test scripts
```

### Branch Rules

- **Feature branches** are created from `develop`, never from `main`
- **Pull Requests** are required for all merges into `develop` and `main`
- **Meaningful commit messages** following the convention:
  ```
  feat: add paginated incident listing with filtering
  fix: resolve check for null verifiedByUserId in JWT claim
  chore: update Dockerfile to .NET 9 base image
  docs: add API endpoint table to README
  ```
- Every team member's contributions are traceable through commit history

---

## 📁 Project Structure

```
ProjectWasel/
├── Controllers/
│   ├── AuthController.cs          # Register, Login, Refresh, Logout
│   ├── CheckpointController.cs   # Checkpoint CRUD + history + filtering
│   ├── IncidentController.cs     # Incident CRUD + verify/close lifecycle
│   ├── RouteController.cs        # Route estimation + history
│   ├── AlertController.cs        # Alert subscriber lookup
│   ├── SubscriptionController.cs # Subscription CRUD
│   ├── ExternalDataController.cs # Location + weather data (external)
│   └── UsersController.cs        # User management
│
├── Models/
│   ├── User.cs
│   ├── Checkpoint.cs
│   ├── CheckpointStatusHistory.cs
│   ├── Incident.cs
│   ├── Report.cs
│   ├── Route.cs
│   ├── Alert.cs
│   ├── Subscription.cs
│   ├── RefreshToken.cs
│   ├── ExternalData.cs
│   ├── GeoResult.cs
│   ├── WeatherResult.cs
│   └── ModelsDTO/
│       ├── AuthDTO.cs
│       ├── CheckpointDTO.cs
│       ├── CheckpointUpdateDTO.cs
│       ├── CheckpointStatusHistoryDTO.cs
│       ├── CheckpointQueryParams.cs
│       ├── IncidentDTO.cs
│       ├── IncidentResponseDTO.cs
│       ├── IncidentUpdateDTO.cs
│       ├── IncidentQueryParams.cs
│       ├── PagedResult.cs
│       ├── ReportDTO.cs
│       ├── RouteRequestDTO.cs
│       ├── RouteResponseDTO.cs
│       ├── SubscriptionDTO.cs
│       ├── AlertDTO.cs
│       ├── AlertSubscribersDTO.cs
│       └── UserDTO.cs
│
├── Repositres/                    # Note: folder name in project
│   ├── IRepository.cs             # Generic CRUD interface
│   ├── GenericRepository.cs       # Generic EF Core implementation
│   ├── ICheckpointRepository.cs   # Checkpoint-specific + raw SQL
│   ├── IIncidentRepository.cs     # Incident-specific + raw SQL
│   ├── ISubscriptionRepository.cs # Subscription queries + raw SQL
│   ├── IRouteRepository.cs        # Route queries + raw SQL
│   └── AuthRepository.cs          # Auth-specific queries
│
├── Services/
│   ├── AuthService.cs             # Token generation & refresh logic
│   ├── IncidentService.cs         # Incident business logic
│   ├── RouteService.cs            # Haversine estimation + checkpoint avoidance
│   ├── AlertService.cs            # Subscriber lookup for incidents
│   ├── WeatherService.cs          # OpenWeatherMap API client
│   ├── GeoService.cs              # Nominatim (OSM) API client
│   └── ExternalApiService.cs      # Combined geo + weather orchestration
│
├── Helper/
│   ├── JwtHelper.cs               # Access + refresh token generation
│   └── PasswordHasher.cs          # BCrypt wrapper
│
├── Profiles/
│   └── (AutoMapper mapping profiles)
│
├── Data/
│   └── WaselContext.cs            # EF Core DbContext
│
├── Migrations/                    # EF Core migration history
├── Properties/
│   └── launchSettings.json
│
├── Dockerfile                     # Multi-stage production build
├── appsettings.json               # Base configuration
├── appsettings.Development.json   # Development overrides
├── Program.cs                     # DI registration + middleware pipeline
└── ProjectWasel.csproj            # Project file + NuGet dependencies
```

---

## 👥 Team

> Course: Advanced Software Engineering — Spring 2026
> Instructor: Dr. Amjad AbuHassan
> Deadline: April 17, 2026

| Member | Role |
|---|---|
| Ahmad Yaser | Backend Developer |
| *(add team members)* | *(add roles)* |

---

## 📄 License

This project is developed for academic purposes as part of the Advanced Software Engineering course at [University Name].

---

*Built with ❤️ for Palestine 🇵🇸*

# Wasel API — Simple Project Overview

## About

This is a road checkpoint and incident management system.  
The API helps users track checkpoints, report incidents, and get alerts about road conditions.

There are 3 types of users:

- **Citizen** → can view data and submit reports  
- **Moderator** → can verify incidents and moderate reports  
- **Admin** → full control over the system  

---

## What we did

- Created models:  
  `User, Checkpoint, Incident, Report, ReportVote, ReportAuditLog, Subscription, Route, RefreshToken, ExternalData`

- Built DbContext with all relationships and constraints  

- Added seed data for testing (users, checkpoints, incidents, reports…)  

- Created DTOs for all entities to handle requests and responses  

- Implemented repositories:
  - Generic repository for CRUD  
  - Specific repositories (Checkpoint, Incident, Report, Subscription, Auth)  

- Added service layer:
  - `AuthService, AlertService, IncidentService, ReportService, RouteService, ExternalApiService`  

- Added helpers:
  - Jwt → generate access & refresh tokens  
  - PasswordHasher → hash passwords using BCrypt  

- Built controllers:
  - AuthController → login, register, refresh  
  - CheckpointController → manage checkpoints  
  - IncidentController → manage incidents  
  - ReportsController → manage reports & votes  
  - RouteController → route estimation  
  - SubscriptionController → user subscriptions  
  - AlertController → get users for alerts  
  - ExternalDataController → geo + weather data  

- Implemented:
  - JWT authentication  
  - Role-based authorization  
  - Pagination, filtering, sorting  
  - Raw SQL + LINQ queries  
  - Caching using MemoryCache  
  - Docker support  

---

## Tables

- **Users** → id, name, email, password, role, lockout info  
- **Checkpoints** → id, name, latitude, longitude, status  
- **CheckpointStatusHistories** → status changes history  
- **Incidents** → id, title, description, type, severity, status, checkpointId, userId  
- **Reports** → id, content, location, status, incidentId, userId  
- **ReportVotes** → id, voteType, reportId, userId  
- **ReportAuditLogs** → moderation logs  
- **Subscriptions** → user subscriptions (area + category)  
- **Routes** → saved routes with distance and duration  
- **ExternalData** → cached geo + weather data  
- **RefreshTokens** → token, expiry, user  

---

## DTOs

### Auth
- RegisterDto  
- LoginDto  
- AuthResponseDto  
- RefreshTokenDto  

### Checkpoints
- CheckpointDTO  
- CheckpointUpdateDTO  
- CheckpointQueryParams  

### Incidents
- IncidentCreateDTO  
- IncidentUpdateDTO  
- IncidentResponseDTO  

### Reports
- ReportDTO  

### Routes
- RouteRequestDTO  
- RouteResponseDTO  

### Subscriptions
- SubscriptionDTO  

### Users
- UserDTO  

---

## Authentication

- Uses JWT tokens  
- Access token → valid for **3 hours**  
- Refresh token → valid for **7 days**  

### Flow:
1. User logs in or registers  
2. Gets access token + refresh token  
3. Uses access token in requests  
4. Uses refresh token when access token expires  

---

## Helpers

### Jwt
- GenerateAccessToken(user)  
- GenerateRefreshToken()  

### PasswordHasher
- Hash(password)  
- Verify(password, hash)  

---

## Repositories

### GenericRepository
Basic CRUD:
- GetAll  
- GetById  
- Add  
- Update  
- Delete  

### AuthRepository
- Register  
- Login  
- SaveRefreshToken  
- GetUserByRefreshToken  
- RevokeRefreshToken  

### Other Repositories
- CheckpointRepository  
- IncidentRepository  
- ReportRepository  
- SubscriptionRepository  
- RouteRepository  

---

## Controllers

### AuthController
- register  
- login  
- refresh  
- logout  

### CheckpointController
- CRUD for checkpoints  
- status history  

### IncidentController
- create, update, verify, close  

### ReportsController
- create report  
- vote  
- moderate  
- audit logs  

### RouteController
- estimate routes  
- get routes  

### SubscriptionController
- manage subscriptions  

### AlertController
- get users to notify  

### ExternalDataController
- get geo + weather data  

---

## API Endpoints (Examples)

### Auth
- POST `/api/v1/auth/register`  
- POST `/api/v1/auth/login`  

### Checkpoint
- GET `/api/v1/checkpoint`  
- POST `/api/v1/checkpoint`  

### Incident
- GET `/api/v1/incident`  
- POST `/api/v1/incident`  

### Reports
- POST `/api/v1/reports`  
- POST `/api/v1/reports/{id}/vote`  

### Routes
- POST `/api/v1/routes/estimate`  

---

## Features

- Role-based system (Admin / Moderator / Citizen)  
- Incident verification workflow  
- Report voting system  
- Audit logs for moderation  
- Route estimation using GPS  
- External API integration (location + weather)  
- Caching to improve performance  
- Docker support  

---

## Technology Stack

- ASP.NET Core 8  
- C#  
- Entity Framework Core  
- SQL Server  
- JWT Authentication  
- AutoMapper  
- Swagger  
- Docker  

---

## Diagrams

### ERD (Database Relationships)

```text
User
 ├──< RefreshToken
 ├──< Incident (createdBy)
 ├──< Report
 ├──< ReportVote
 ├──< ReportAuditLog (moderator)
 └──< Subscription

Checkpoint
 ├──< Incident
 └──< CheckpointStatusHistory

Incident
 ├── belongs to Checkpoint
 ├── belongs to User
 └──< Report

Report
 ├── belongs to User
 ├── belongs to Incident
 └──< ReportVote

ReportVote
 └── belongs to User + Report

ReportAuditLog
 └── belongs to Report + User

Subscription
 └── belongs to User

Route (independent)

ExternalData (cached data)
```

---

### Architecture

```text
Client (Frontend / Postman)
        |
        v
   Controllers
        |
        v
     Services
        |
  ---------------------
  |         |         |
  v         v         v
Repositories Helpers External APIs
     |                   |
     v                   v
  Database        (Geo + Weather)
```

---

### Request Flow


Client → Controller → Service → Repository → Database

---

## Notes

- API is versioned (v1)  
- Uses Swagger for testing  
- Supports pagination and filtering  
- External APIs are cached for 15 minutes

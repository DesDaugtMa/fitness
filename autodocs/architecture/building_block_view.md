---
title: "Building Block View"
date: 2026-03-21
type: architecture
view_type: building_block
status: accepted
tags: [architecture, building-block, c4, components, isaqb, view/building-block]
related_docs:
  - "[[../context_view]]"
  - "[[../runtime_view]]"
  - "[[../adrs/adr-001-use-entity-framework]]"
  - "[[../features/index]]"
  - "[[../tests/index]]"
source_files:
  - Source/Fitness/Controllers/
  - Source/Fitness/Models/
  - Source/Fitness/Config/
  - Source/Fitness/Program.cs
---

# Building Block View (C4 L2/L3)

## Einführung

Die Fitness WebApp folgt einer **Layered (3-Tier) Monolithic Architecture**:
- **Presentation Layer** (Views/Controllers)
- **Application/Domain Layer** (Models, Business Logic)
- **Persistence Layer** (Entity Framework, SQL Server)

Externe Dependencies: **Google OAuth2**, **AppSettings**

## System-Ebene Architektur (C4 L2)

```mermaid
C4Container
    title Fitness App Container Diagram (C4 L2)
    
    Person(user, "Fitnessbenutzer", "Nutzer in Browser")
    
    Container_Boundary(webapp, "Fitness WebApp") {
        Container(mvc, "ASP.NET Core MVC<br/>Presentation", "C#/.NET 10", "Rendering von Views, Request-Routing")
        Container(domain, "Domain Models & Services<br/>Application Logic", "C#", "User, Exercise, Workout Business Logic")
        Container(datalayer, "Entity Framework Core<br/>Data Access Layer", "C#/ORM", "LINQ Queries, Migrations")
        Rel(mvc, domain, "Nutzt")
        Rel(domain, datalayer, "Nutzt")
    }
    
    System_Ext(google, "Google OAuth2", "OAuth2-Provider")
    System_Ext(sqlserver, "SQL Server Datenbank", "Persistenz")
    System_Ext(appsettings, "Configuration<br/>(appsettings.json)", "Config-Management")
    
    Rel(user, mvc, "HTTPS Requests (Browser)")
    Rel(mvc, google, "OAuth2 Login-Flow")
    Rel(datalayer, sqlserver, "Entity Framework Queries")
    Rel(mvc, appsettings, "Liest Config (DB, OAuth Keys)")
```

## Module/Komponenten (C4 L3)

### 1. Presentation Layer (Controllers & Views)

| Komponente | Verantwortung | Quellpfad | Abhängigkeiten |
|---|---|---|---|
| **AccountController** | User Auth, Registration, Login, Logout, OAuth | `Source/Fitness/Controllers/AccountController.cs` | FitnessDbContext, IPasswordHasher |
| **ExercisesController** | Exercise CRUD | `Source/Fitness/Controllers/ExercisesController.cs` | FitnessDbContext |
| **HomeController** | Dashboard, Error Handling | `Source/Fitness/Controllers/HomeController.cs` | — |
| **AdminController** | Admin Functions | `Source/Fitness/Controllers/AdminController.cs` | FitnessDbContext |
| **MuscleGroupsController** | Muscle Group Management | `Source/Fitness/Controllers/MuscleGroupsController.cs` | FitnessDbContext |
| **ImagesController** | Image Upload/Management | `Source/Fitness/Controllers/ImagesController.cs` | FitnessDbContext, File System |
| **RegistrationTokensController** | Registration Token CRUD | `Source/Fitness/Controllers/RegistrationTokensController.cs` | FitnessDbContext |

**Views Location:** `Source/Fitness/Views/` (ASP.NET Razor Templates)

### 2. Domain Models (Application/Domain Layer)

| Modell | Verantwortung | Quellpfad | Persistiert |
|---|---|---|---|
| **User** | Benutzerkonto, Credentials | `Source/Fitness.DataAccess/Models/User.cs` | ✅ SQL Server |
| **Exercise** | Trainingsübung | `Source/Fitness.DataAccess/Models/Exercise.cs` | ✅ SQL Server |
| **Workout** | Trainingsplan/-session | `Source/Fitness.DataAccess/Models/Workout.cs` | ✅ SQL Server |
| **MuscleGroup** | Muskelgruppe (für Kategorisierung) | `Source/Fitness.DataAccess/Models/MuscleGroup.cs` | ✅ SQL Server |
| **Image** | Übungs-Bild | `Source/Fitness.DataAccess/Models/Image.cs` | ✅ SQL Server |
| **RegistrationToken** | Einladungs-Token | `Source/Fitness.DataAccess/Models/RegistrationToken.cs` | ✅ SQL Server |

**ViewModel Hierarchy:** `Source/Fitness/Models/ViewModels/` (für Server-Side Rendering)

### 3. Data Access Layer (Entity Framework Core)

| Komponente | Verantwortung | Quellpfad |
|---|---|---|
| **FitnessDbContext** | DbSet-Definitionen, Migrations | `Source/Fitness.DataAccess/FitnessDbContext.cs` |
| **Entity Mappings** | Fluent API Konfigurationen | `Source/Fitness.DataAccess/FitnessDbContext.cs` (OnModelCreating) |
| **EF Migrations** | Schema-Versioning | `Source/Fitness.DataAccess/Migrations/` |

### 4. Configuration & Infrastructure

| Komponente | Verantwortung | Quellpfad |
|---|---|---|
| **AppSettings** | Config-POCO mit Connection String, OAuth Keys | `Source/Fitness/Config/AppSettings.cs` |
| **appsettings.json** | Umgebungs-spezifische Settings | `Source/Fitness/Config/appsettings*.json` |
| **Program.cs** | DI-Container Setup, Middleware Configuration | `Source/Fitness/Program.cs` |

## Komponenten-Abhängigkeiten

```mermaid
graph TB
    User["👤 Browser User"]
    Controllers["Controllers Layer<br/>(Account, Exercises, Home, Admin, ...)"]
    Models["Domain Models<br/>(User, Exercise, Workout, ...)"]
    EF["Entity Framework Core"]
    DB["SQL Server"]
    OAuth["Google OAuth2"]
    Config["AppSettings / Config"]
    
    User -->|HTTP Request| Controllers
    Controllers -->|Uses| Models
    Controllers -->|Uses| EF
    Models -->|Mapped in| EF
    EF -->|SQL Queries| DB
    Controllers -->|OAuth2 Flow| OAuth
    Controllers -->|Reads| Config
    EF -->|Reads| Config
```

## Cross-Cutting Concerns

| Concern | Implementation | Location |
|---|---|---|
| **Authentication** | Cookie + Google OAuth2 | `Program.cs` (ConfigureAuthentication) |
| **Authorization** | `[Authorize]` Attribute auf Controller-Actions | `Controllers/` |
| **Anti-CSRF** | `[ValidateAntiForgeryToken]` auf POST-Actions | `Controllers/` |
| **Error Handling** | Global exception/error pages | `Controllers/HomeController.cs` (Error Action) |
| **Configuration Management** | `AppSettings` POCO + Environment-specific JSON | `Config/`, `Program.cs` |

## External Component: Google OAuth2 Integration

```mermaid
sequenceDiagram
    participant Browser
    participant WebApp
    participant GoogleAuth as "Google OAuth2"
    
    Browser->>WebApp: GET /Account/GoogleLogin
    WebApp->>GoogleAuth: Redirect to Google Login (Authorization Request)
    GoogleAuth-->>Browser: Google Login UI
    Browser->>GoogleAuth: User Authenticates
    GoogleAuth-->>WebApp: Authorization Code Callback
    WebApp->>GoogleAuth: Exchange Code for Token (Backend)
    GoogleAuth-->>WebApp: ID Token + AccessToken
    WebApp->>WebApp: Create/Update User, Set Cookie
    WebApp-->>Browser: Redirect to Dashboard (Authenticated)
```

## Quality Concerns pro Baustein

| Baustein | Performance | Security | Maintainability | Notes |
|---|---|---|---|---|
| **Controllers** | Synchronous Request/Response | CSRF, Authorization | MVC separation of concerns | Rate Limiting missing (GAP-001) |
| **Models** | Memory efficient | Property Validation | Clear POCO design | Consider Fluent Validation |
| **EF Core** | Connection Pooling enabled | Parameterized SQL (automatic) | ORM abstracts DB details | Migration ordering critical |
| **SQL Server** | Indexes needed on FK/PK | At-rest encryption not enforced (GAP-003) | Schema versioning via migrations | TDS protocol unencrypted (consider VPN) |
| **OAuth2 Integration** | Depends on Google SLA | Token storage in cookie secure | Minimal retry logic (GAP-002) | No circuit breaker pattern |

## Architecture Decision Records (ADRs)

Folgende ADRs beeinflussen diese Sicht:
- **ADR-001**: Entity Framework Core für ORM (vs. Dapper, NHibernate)

## Mapping: Code → Building Blocks

(Siehe auch `[[../architecture_mapping]]` für vollständiges Mapping)

| Code-Bereich | Baustein | Layer | Status |
|---|---|---|---|
| `Source/Fitness/Controllers/**` | Controllers | Presentation | ✅ Dokumentiert |
| `Source/Fitness/Models/**` | Domain Models | Application | ✅ Dokumentiert |
| `Source/Fitness.DataAccess/**` | EF Core + Models | Persistence | ✅ Dokumentiert |
| `Source/Fitness/Config/**` | Configuration | Infrastructure | ✅ Dokumentiert |
| `Source/Fitness/Program.cs` | DI & Middleware Setup | Infrastructure | ✅ Dokumentiert |
| `Source/Fitness/Views/**` | View Templates | Presentation | ⚠️ Minimal Docs |

---

## Navigation

[[index]] — Architektur-Übersicht
[[context_view]] — System Context
[[runtime_view]] — Runtime Behavior

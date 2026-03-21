---
title: "Architecture Mapping"
date: 2026-03-21
type: architecture
view_type: mapping
status: accepted
tags: [architecture, mapping, code-to-docs, isaqb]
related_docs:
  - "[[../building_block_view]]"
  - "[[../context_view]]"
  - "[[../features/index]]"
  - "[[../tests/index]]"
  - "[[../adrs/index]]"
source_files:
  - Source/Fitness/Controllers/
  - Source/Fitness/Models/
  - Source/Fitness.DataAccess/
---

# Architecture Mapping (Code ↔ Docs)

## Einführung

Diese Sicht mappt **Code-Dateien** auf **Architektur-Bausteine**. Sie verbindet:
- **Code-Pfade** → **Architektur-Bausteine** (who is responsible)
- **Architektur-Bausteine** → **Features** (what business value)
- **Features** → **Tests** (how it's validated)
- **Bausteine** → **ADRs** (why this decision)

Dieses Mapping ist **maschinenlesbar** und wird von zukünftigen Agents für Code-Sync verwendet.

---

## 📍 Layer 1: Top-Level Mapping

| Code Root | Baustein | Layer | Architektur-Doc | Status |
|---|---|---|---|---|
| `Source/Fitness/Controllers/` | **MVC Controllers** | Presentation | [[../building_block_view#1-presentation-layer-controllers--views]] | ✅ Mapped |
| `Source/Fitness/Models/` | **Domain Models + ViewModels** | Application/Presentation | [[../building_block_view#2-domain-models-application-domain-layer]] | ✅ Mapped |
| `Source/Fitness.DataAccess/` | **Entity Framework Core** | Persistence | [[../building_block_view#3-data-access-layer-entity-framework-core]] | ✅ Mapped |
| `Source/Fitness/Config/` | **Configuration** | Infrastructure | [[../building_block_view#4-configuration--infrastructure]] | ✅ Mapped |
| `Source/Fitness/Views/` | **Razor Templates** | Presentation | [[../building_block_view#1-presentation-layer-controllers--views]] | ⚠️ Minimal |
| `Source/Fitness/Program.cs` | **DI Container Setup** | Infrastructure | [[../building_block_view#4-configuration--infrastructure]] | ✅ Mapped |

---

## 📍 Layer 2: Controller Mapping (Detailed)

### Presentation Layer: Controllers

| Controller | Responsibility | Code Path | Features | Tests | ADRs |
|---|---|---|---|---|---|
| **AccountController** | User Auth (Register, Login, Logout, OAuth) | `Controllers/AccountController.cs` | [[../../features/2026-03-21-user-management]] | [[../../tests/test-auth]] | — |
| **ExercisesController** | Exercise CRUD, Exercise List | `Controllers/ExercisesController.cs` | [[../../features/2026-03-21-workout-tracking]] | [[../../tests/test-exercises]] | [[../../adrs/adr-001-use-entity-framework]] |
| **HomeController** | Dashboard, Error Page, Privacy | `Controllers/HomeController.cs` | — | [[../../tests/test-home]] | — |
| **AdminController** | Admin Functions (if any) | `Controllers/AdminController.cs` | — | — | — |
| **MuscleGroupsController** | Muscle Group CRUD | `Controllers/MuscleGroupsController.cs` | — | — | — |
| **ImagesController** | Image Upload/Management | `Controllers/ImagesController.cs` | — | — | — |
| **RegistrationTokensController** | Token-based Registration | `Controllers/RegistrationTokensController.cs` | [[../../features/2026-03-21-user-management]] | — | — |

---

## 📍 Layer 3: Domain Model Mapping

### Core Domain Entities

| Model | Responsibility | Code Path | DB Table | Features | Validations |
|---|---|---|---|---|---|
| **User** | User account, credentials, profile | `DataAccess/Models/User.cs` | `Users` | [[../../features/2026-03-21-user-management]] | Email unique, Password hashed |
| **Exercise** | Fitness exercise definition | `DataAccess/Models/Exercise.cs` | `Exercises` | [[../../features/2026-03-21-workout-tracking]] | Name required, FK to MuscleGroup |
| **Workout** | Training session / plan | `DataAccess/Models/Workout.cs` | `Workouts` | [[../../features/2026-03-21-workout-tracking]] | FK to User, date required |
| **MuscleGroup** | Muscle group category | `DataAccess/Models/MuscleGroup.cs` | `MuscleGroups` | — | Name required |
| **Image** | Exercise image / demonstration | `DataAccess/Models/Image.cs` | `Images` | — | FK to Exercise, blob storage |
| **RegistrationToken** | Invitation token for signup | `DataAccess/Models/RegistrationToken.cs` | `RegistrationTokens` | [[../../features/2026-03-21-user-management]] | Token unique, expires |

### ViewModels (Presentation Models)

| ViewModel | Used By | Code Path | Purpose |
|---|---|---|---|
| **LoginViewModel** | AccountController.Login | `Models/ViewModels/LoginViewModel.cs` | Form binding for login |
| **RegisterViewModel** | AccountController.Register | `Models/ViewModels/RegisterViewModel.cs` | Form binding for registration |
| **ExerciseCreateViewModel** | ExercisesController.Create | `Models/ViewModels/Exercises/Create.cs` | Form + dropdowns for create |
| **AdminIndexViewModel** | AdminController.Index | `Models/ViewModels/AdminIndexViewModel.cs` | Dashboard data |
| **ImageCreateViewModel** | ImagesController.Create | `Models/ViewModels/Image/Create.cs` | File upload form |
| **RegistrationTokenCreateViewModel** | RegistrationTokensController.Create | `Models/ViewModels/RegistrationTokens/Create.cs` | Token creation form |

---

## 📍 Layer 4: Data Access Mapping

### Entity Framework Configuration

| Component | Code Path | Responsibility |
|---|---|---|
| **FitnessDbContext** | `DataAccess/FitnessDbContext.cs` | DbSet definitions, fluent API config |
| **Migrations** | `DataAccess/Migrations/` | Schema versioning, rollback capability |
| **DbContextOptions** | `Program.cs` (DI setup) | Connection string, lazy-loading config |

### Database Schema

```
┌─────────────────────────────────────────────────────┐
│ Fitness Database (SQL Server)                       │
├─────────────────────────────────────────────────────┤
│ Users                          (PK: Id)             │
│  ├─ Id (int)                                        │
│  ├─ Email (string, unique)                          │
│  ├─ PasswordHash (string)       [Encrypted]        │
│  ├─ DisplayName (string)                           │
│  ├─ Created (datetime)                             │
│  └─ LastLogin (datetime)                           │
├─────────────────────────────────────────────────────┤
│ Exercises (PK: Id, FK: UserId)                      │
│  ├─ Id (int)                                        │
│  ├─ UserId (int, FK → Users)                        │
│  ├─ Name (string)                                   │
│  ├─ MuscleGroupId (int, FK → MuscleGroups)         │
│  ├─ Reps (int)                                      │
│  ├─ Sets (int)                                      │
│  ├─ Created (datetime)                              │
│  └─ Updated (datetime)                              │
├─────────────────────────────────────────────────────┤
│ Workouts (PK: Id, FK: UserId)                       │
│  ├─ Id (int)                                        │
│  ├─ UserId (int, FK → Users)                        │
│  ├─ Name (string)                                   │
│  ├─ WorkoutDate (datetime)                          │
│  ├─ Duration (int, minutes)                         │
│  └─ Created (datetime)                              │
├─────────────────────────────────────────────────────┤
│ MuscleGroups (PK: Id)                               │
│  ├─ Id (int)                                        │
│  └─ Name (string, unique)                           │
├─────────────────────────────────────────────────────┤
│ Images (PK: Id, FK: ExerciseId)                     │
│  ├─ Id (int)                                        │
│  ├─ ExerciseId (int, FK → Exercises)               │
│  ├─ Url (string, blob storage path)                 │
│  └─ Created (datetime)                              │
├─────────────────────────────────────────────────────┤
│ RegistrationTokens (PK: Id)                         │
│  ├─ Id (int)                                        │
│  ├─ Token (string, unique)                          │
│  ├─ ExpiresAt (datetime)                            │
│  ├─ UsedAt (datetime, nullable)                     │
│  └─ Created (datetime)                              │
└─────────────────────────────────────────────────────┘
```

---

## 🔗 Feature → Test Mapping (Bidirectional)

| Feature | Test Suite | Code Path | Status |
|---|---|---|---|
| **User Registration** | `test-auth` | `Tests/AuthTests.cs` (missing) | ⚠️ Tests Missing |
| **User Login** | `test-auth` | `Tests/AuthTests.cs` (missing) | ⚠️ Tests Missing |
| **Google OAuth2** | `test-oauth` | `Tests/OAuthTests.cs` (missing) | ⚠️ Tests Missing |
| **Exercise CRUD** | `test-exercises` | `Tests/ExerciseTests.cs` (missing) | ⚠️ Tests Missing |
| **Workout Management** | `test-workouts` | `Tests/WorkoutTests.cs` (missing) | ⚠️ Tests Missing |
| **Password Hashing** | `test-security` | `Tests/SecurityTests.cs` (missing) | ⚠️ Tests Missing |

---

## Baustein → ADR Mapping

| Baustein | ADR | Decision | Rationale |
|---|---|---|---|
| **EF Core** | [[../../adrs/adr-001-use-entity-framework]] | Use EF Core ORM | Productivity vs Dapper speed trade-off |
| **Documentation System** | [[../../adrs/adr-001-documentation-system]] | Obsidian-compatible markdown | Agent-friendly, Obsidian-native |
| **Authentication** | (None yet) | Cookie + Google OAuth2 | Dual-mode: local + social |
| **Database** | (None yet) | SQL Server + TDS | Enterprise standard, but consider PostgreSQL |
| **UI Rendering** | (None yet) | Server-side Razor | Monolith pattern, no SPA (yet) |

---

## 🚀 Feature Roadmap & Code Status

| Feature | Status | Epic | Code | Tests | ADR |
|---|---|---|---|---|---|
| [[../../features/2026-03-21-user-management]] | ✅ In Scope | User Management | [[../building_block_view#1-presentation-layer-controllers--views]] (AccountController) | ⚠️ Missing | — |
| [[../../features/2026-03-21-workout-tracking]] | ✅ In Scope | Workout Tracking | [[../building_block_view#1-presentation-layer-controllers--views]] (ExercisesController) | ⚠️ Missing | ADR-001 |
| Data Export (CSV) | 📋 Planned | Analytics | — | — | — |
| Social Sharing (Workouts) | 📋 Planned | Community | — | — | — |
| Mobile App (React Native) | 🔮 Future | Mobile | — | — | — |

---

## Query & Cross-Cutting Map

### Authentication Flow

```
Browser (Login Form)
  ↓ POST /Account/Login
AccountController.Login()
  ↓ Validates Email + Password
IPasswordHasher.VerifyHashedPassword()
  ↓ Matches against User.PasswordHash
FitnessDbContext.Users.FirstOrDefaultAsync()
  ↓ Queries SQL Server
[SQL] SELECT * FROM Users WHERE Email = @email
  ↓ Returns User entity
AuthenticationTicket + Cookie
  ↓ Set-Cookie Header
Browser (Session cookie stored)
```

### Exercise Creation Flow

```
Browser (Exercise Form)
  ↓ POST /Exercises/Create
ExercisesController.Create(ExerciseCreateViewModel)
  ↓ Validates ModelState
Exercise entity
  ↓ Maps from ViewModel
FitnessDbContext.Exercises.AddAsync()
  ↓ Queues INSERT
FitnessDbContext.SaveChangesAsync()
  ↓ Sends to SQL Server
[SQL] INSERT INTO Exercises (UserId, Name, ...) VALUES (...)
  ↓ Returns new Exercise.Id
Browser (Redirect to /Exercises/Details/{id})
```

---

## Code Quality Metrics

| Concern | Current Status | Measurement | Target |
|---|---|---|---|
| **Documentation Coverage** | ⚠️ 50% | Analyzed [[../_meta]] | 80%+ |
| **Test Coverage** | ❌ Unknown | No CI/CD visible | 80%+ |
| **Code Duplication** | ⚠️ Likely | Manual inspection needed | < 5% |
| **Cyclomatic Complexity** | ⚠️ Unknown | Need analysis | Avg < 5 |
| **Method Length** | ⚠️ Unknown | Code review | < 100 LoC max |

---

## Synchronization Rules (for Agents)

**How the Updater Agent (100) should use this mapping:**

1. **On Code Change:** Find code file → lookup in mapping → update related docs
   - If `Controllers/ExercisesController.cs` changes → update `[[../../features/2026-03-21-workout-tracking]]`
   - If `Models/User.cs` changes → update `[[../building_block_view]]` (User model section)

2. **On Missing Test:** Find feature → lookup expected test → create TODO
   - Feature `2026-03-21-workout-tracking` → Test mapping shows `test-workouts` missing → Create [[../../todo/todo-create-workout-tests]]

3. **On Missing ADR:** Find decision → lookup ADR mapping → suggest new ADR
   - No ADR for "Authentication strategy" → Create [[../../adrs/adr-002-authentication-strategy]]

---

## Related Documentation

- [[../building_block_view]] — Detailed component descriptions
- [[../../features/index]] — Feature documentation
- [[../../tests/index]] — Test documentation
- [[../../adrs/index]] — Architecture Decision Records
- [[../../todo/index]] — TODO backlog (derived from this mapping)

---

## Navigation

[[index]] — Architektur-Übersicht
[[building_block_view]] — Detailed Building Blocks

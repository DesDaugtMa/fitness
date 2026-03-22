---
type: feature
created: 2026-03-21
updated: 2026-03-22
status: released
area: auth
tags: [feature, auth, security, user-management, cmp/user-service, dom/identity, status/active]
components: [User, Role, RegistrationToken, Friendship]
related_adrs: [adr-001-use-entity-framework]
related_tests: []
related_todos: [todo-create-user-management-tests]
commits: []
---

# User Management System

## Overview

Complete user authentication and management system with support for local accounts and OAuth2 (Google).

- User registration with email validation and optional registration tokens
- Login with password hashing (bcrypt via Identity.PasswordHasher)
- OAuth2 integration (Google authentication)
- Role-based access control (RBAC)
- User profiles with optional profile images
- Friendship/social features between users
- Password reset and recovery flow

## Architecture

### Domain Models

**User**
- Email (unique)
- PasswordHash (nullable for OAuth-only accounts)
- DisplayName
- RoleId (foreign key to Role)
- ProfileImageId (nullable, foreign key to Image)
- AuthProvider and ProviderId (for OAuth2)
- ShareExercisesEnabled (boolean)
- CreatedAt timestamp
- Navigation: FriendshipsRequested, FriendshipsReceived

**Role**
- Name (unique)
- Permissions (implicit through controller authorization)

**RegistrationToken**
- Token (primary key, composite with issued date)
- ExpiresAt
- UsedAt (nullable, indicates if token was used)
- IsActive (business logic)

**Friendship**
- RequesterId and ResponderId
- Status enum (pending, accepted, rejected)
- CreatedAt timestamp

### API Controllers

**AccountController** (`Controllers/AccountController.cs`)
- GET `/Account/Register/{token}` – Registration form with token validation
- POST `/Account/Register` – Create new user account
- GET `/Account/Login` – Login form
- POST `/Account/Login` – Authenticate user
- GET `/Account/Logout` – Sign out user
- Google OAuth2 callback handling
- Access denied redirect

### Key Features

1. **Secure Registration**
   - Registration tokens required (invite-only system)
   - Token expiration validation
   - Email uniqueness enforced at DB level

2. **Password Security**
   - Hashed passwords via `IPasswordHasher<User>`
   - Never stored in plaintext
   - Optional for OAuth-only accounts

3. **OAuth2 Integration**
   - Google authentication via `AddGoogle()`
   - Claims-based identity
   - ProviderId uniqueness constraint

4. **Access Control**
   - Cookie-based authentication
   - Default redirect to login page (/Account/Login)
   - Access denied page at /Account/AccessDenied
   - 30-day sliding expiration

## Implementation Notes

- Uses ASP.NET Core Identity PasswordHasher (production-ready)
- ForeignKey constraints prevent cascade delete cycles on Friendship
- Email and ProviderId indexed unique for performance
- All timestamps use UTC (DateTime.UtcNow)

## Testing

- [ ] Unit tests for User model validation
- [ ] Integration tests for registration flow
- [ ] OAuth2 token validation tests
- [ ] Password hashing verification tests
- [ ] Friendship acceptance/rejection tests
- [ ] E2E: Complete registration flow

## Related

- [[../adrs/adr-001-use-entity-framework]] – ORM decision
- [[../tests/test-user-management]] – Test documentation (pending)
- [[../todo/todo-create-user-management-tests]] – Test implementation backlog

[[../features/index]]

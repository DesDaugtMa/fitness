---
type: feature
created: 2026-03-21
status: released
area: admin
tags: [feature, admin, security, role-based-access, cmp/admin-service, dom/governance, status/active]
components: [RegistrationToken, AdminIndexViewModel]
related_adrs: [adr-001-use-entity-framework]
related_tests: []
related_todos: [todo-create-admin-tests]
commits: []
---

# Admin Features & Management

## Overview

Administrative dashboard and management functions for invite-based user registration system.

- Registration token lifecycle management (create, expire, revoke)
- Approve/monitor new user signups
- Display active and used registration tokens
- Generate shareable invitation links
- Role-based access control (Admin role required)

## Architecture

### Domain Models

**RegistrationToken**
- Token (primary key, unique)
- ExpiresAt (expiration timestamp)
- IsActive (boolean, business logic)
- UsedAt (nullable, tracks when token was used)
- UsedByUserId (optional, foreign key to User who registered)
- CreatedByUserId (optional, foreign key to User who created token)
- Description (optional, for reference/notes)
- CreatedAt timestamp

### API Controllers

**AdminController** (`Controllers/AdminController.cs`)
- Requires Admin role authorization
- GET `/Admin` or `/Admin/Index` – Registration token management dashboard
  - Lists all registration tokens
  - Shows token status (active, expired, used)
  - Displays shareable invitation links for active tokens
  - Show which user used each token

### Key Features

1. **Invite-Only Registration**
   - Users cannot self-register without a valid token
   - Tokens have explicit expiration dates
   - Admins control who can join via token creation

2. **Token Management**
   - View all tokens and their status
   - Active invi links are generated with proper base URL
   - Expired tokens show null link (cannot be used)
   - Track which user redeemed each token

3. **Access Control**
   - AdminController requires `[Authorize(Roles = "Admin")]`
   - Only admin users can view token dashboard
   - Prevents unauthorized access to user registration

## Implementation Notes

- Tokens include base URL construction for shareable links
- IsActive computed from ExpiresAt and UsedAt state (not explicit flag)
- Many-to-one relationship: RegistrationToken → User (CreatedBy and UsedBy)
- Timestamps use UTC (DateTime.UtcNow)
- AdminIndexViewModel transforms token data for UI presentation

## Testing

- [ ] Authorization tests (non-admin users blocked)
- [ ] Token generation and validation
- [ ] Expiration date enforcement
- [ ] Link generation with correct base URLs
- [ ] Token status transitions (active → used, active → expired)
- [ ] E2E: Admin creates token → user registers with token → appears in admin UI

## Related

- [[../features/2026-03-21-user-management]] – Registration flow that uses tokens
- [[../adrs/adr-001-use-entity-framework]] – ORM decision
- [[../tests/test-admin]] – Test documentation (pending)
- [[../todo/todo-create-admin-tests]] – Test implementation backlog

[[../features/index]]

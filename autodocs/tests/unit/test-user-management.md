---
type: test
created: 2026-03-22
area: auth
tags: [test, unit, integration, auth, cmp/user-service, status/todo]
related_feature: [[../features/2026-03-21-user-management]]
test_level: [unit, integration]
coverage_status: 0%
---

# User Management Tests

## Overview

Tests for user authentication, registration, and profile management.

## Test Areas

### Unit Tests (Pending)
- [x] User model creation
- [ ] Password hashing validation
- [ ] Email validation and uniqueness
- [ ] RegistrationToken expiration logic
- [ ] Role assignment

### Integration Tests (Pending)
- [ ] AccountController.Register POST flow
  - Valid registration with token
  - Invalid token (expired)
  - Missing required fields
  - Email conflict detection
- [ ] AccountController.Login POST flow
  - Valid credentials
  - Invalid credentials
  - Non-existent user
  - Failed login tracking (rate limiting - future)
- [ ] OAuth2 Google authentication
  - Token validation
  - Existing user linking
  - New account creation

### E2E Tests (Pending)
- [ ] Complete registration flow
  - Admin creates token
  - User registers with token
  - User logs in
  - User accesses authorized pages
- [ ] Google OAuth flow
  - Redirect to Google
  - OAuth callback
  - Auto account creation
  - Subsequent login

## Related

- [[../features/2026-03-21-user-management]] – User management implementation
- [[../todo/todo-create-user-management-tests]] – Test implementation backlog

[[../tests/index]]

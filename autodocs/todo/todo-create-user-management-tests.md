---
type: todo
created: 2026-03-22
updated: 2026-03-22
prio: high
status: open
area: testing
tags: [todo, test, user-management, cmp/user-service, prio/high, status/todo]
related_feature: [[../features/2026-03-21-user-management]]
related_test: [[../tests/unit/test-user-management]]
---

# Create User Management Tests

## Description

Implement unit and integration tests for the User Management System.

## Subtasks

### Unit Tests
- [ ] User model validation (Email required, DisplayName required)
- [ ] RegistrationToken expiration logic
- [ ] Password hashing verification
- [ ] Role assignment validation
- [ ] Friendship request/accept flow

### Integration Tests  
- [ ] AccountController.Register – valid registration
- [ ] AccountController.Register – token validation (expired, invalid)
- [ ] AccountController.Register – email conflict detection
- [ ] AccountController.Login – valid credentials
- [ ] AccountController.Login – invalid credentials
- [ ] Google OAuth2 callback handling
- [ ] Account linking for OAuth2

### E2E Tests
- [ ] Complete registration flow (admin token → register → login)
- [ ] Google OAuth flow (redirect → callback → account creation)
- [ ] User profile update

## Acceptance Criteria

- [ ] ≥80% code coverage for User/Role/RegistrationToken models
- [ ] Integration tests for all AccountController endpoints
- [ ] OAuth2 flow validated end-to-end
- [ ] All tests passing and integrated into CI/CD

## Related

- [[../features/2026-03-21-user-management]] – Implementation
- [[../tests/unit/test-user-management]] – Test documentation

[[../todo/index]]

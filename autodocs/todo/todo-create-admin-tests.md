---
type: todo
created: 2026-03-22
updated: 2026-03-22
prio: high
status: open
area: testing
tags: [todo, test, admin, cmp/admin-service, prio/high, status/todo]
related_feature: [[../features/2026-03-21-admin-features]]
related_test: [[../tests/unit/test-admin]]
---

# Create Admin Tests

## Description

Implement authorization and integration tests for Admin features and registration token management.

## Subtasks

### Unit Tests
- [ ] RegistrationToken.IsActive computation
- [ ] Token expiration date logic
- [ ] Token used/unused state transitions

### Integration Tests
- [ ] AdminController access control – admin user can view
- [ ] AdminController access control – non-admin user blocked (403)
- [ ] AdminController access control – anonymous user redirects to login
- [ ] Token status display (active, expired, used)
- [ ] Invitation link generation with base URL
- [ ] Token listing/sorting by CreatedAt

### E2E Tests
- [ ] Admin generates token → receives invitation link
- [ ] User clicks link → registration form shows
- [ ] User registers → token marked as UsedAt
- [ ] Admin dashboard shows token as used with user info

## Acceptance Criteria

- [ ] ≥80% coverage for AdminController and RegistrationToken  
- [ ] Authorization tests for all access levels (admin, user, anonymous)
- [ ] Token lifecycle validated (create → display → use → mark used)
- [ ] All tests passing and integrated into CI/CD

## Related

- [[../features/2026-03-21-admin-features]] – Implementation
- [[../tests/unit/test-admin]] – Test documentation

[[../todo/index]]

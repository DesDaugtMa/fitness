---
type: test
created: 2026-03-22
area: admin
tags: [test, unit, integration, admin, cmp/admin-service, status/todo]
related_feature: [[../features/2026-03-21-admin-features]]
test_level: [unit, integration]
coverage_status: 0%
---

# Admin Features Tests

## Overview

Tests for administrative functionality and registration token management.

## Test Areas

### Unit Tests (Pending)
- [ ] RegistrationToken validation
  - Token generation
  - Expiration date logic
  - IsActive computation (not expired AND not used)

### Integration Tests (Pending)
- [ ] AdminController access control
  - Admin user can view dashboard
  - Non-admin users blocked (403 Forbidden)
  - Unauthenticated users redirect to login
- [ ] Token management
  - Create new token
  - List all tokens with status
  - Display invitation links (only for active tokens)
  - Mark token as used
  - Expire token
- [ ] AdminIndexViewModel
  - Token status mapping
  - Link URL generation with base URL
  - Sorting by creation date (descending)

### E2E Tests (Pending)
- [ ] Admin creates token → user sees link → registers via link
  - Admin generates token with expiration
  - Invitation link is valid
  - Clicking link redirects to registration form
  - User completes registration
  - Token marked as UsedAt in admin dashboard

## Related

- [[../features/2026-03-21-admin-features]] – Admin feature implementation
- [[../todo/todo-create-admin-tests]] – Test implementation backlog

[[../tests/index]]

---
type: todo
created: 2026-03-21
status: open
priority: medium
category: documentation
area: testing
tags: [todo, test-documentation, blocked, cmp/tests]
estimated_effort: 16
impact: 3
urgency: 2
effort: 2
priority_score: 8
related_adrs: []
resolved_by: []
source_documents:
  - "[[../auditor_non_compliance#nc-maj-001-missing-test-documentation-files]]"
  - "[[../tests/_meta.md]]"
  - "[[../templates/test-template.md]]"
---

# TODO: Create Test Documentation Stubs

## Problem/Idee

**Ausgangsituation:** `/tests/unit/` und `/tests/e2e/` haben keine Test-Dokumentation (0 Test-Docs)  
**Standard (per [[../tests/_meta.md]]):** Jeder Test braucht ein entsprechendes Doc (z.B. `test-auth-flow.md`)  
**Ziel:** Erstelle 10-15 Test-Dokumentation Dateien  
**Gap:** 0% Test-Docs → 100%  
**Blocker:** Abhängig von [[../todo/todo-create-unit-test-infrastructure]] (Tests müssen zuerst existieren)

**Szenario:** Developer schreibt 20 User-Auth Tests. Wie dokumentiert man diese? In welchen Doc. Wo verlinken?

---

## Business-Value

- **Traceability:** Test ↔ Feature Mapping (bidirektional)
- **Onboarding:** Neue Devs verstehen Test-Struktur
- **Coverage Audit:** Können sehen welche Features getestet sind

---

## Schritt-für-Schritt-Anleitung

**⚠️ BLOCKER:** Diese TODO hängt ab von [[../todo/todo-create-unit-test-infrastructure]]. Erst DANACH beginnen.

### Schritt 1: Create Test Documentation Structure

Nachdem Unit-Tests existieren (nach Todo-1), erstelle folgende Dokumente:

**Datei-Layout:**
```
autodocs/tests/unit/
├── _meta.md (exists already)
├── index.md (exists already)
├── test-user-auth-controller.md      (← NEW)
├── test-exercise-crud-operations.md  (← NEW)
├── test-user-service-password.md     (← NEW)
├── test-oauth2-integration.md        (← NEW)
├── test-user-validation-rules.md     (← NEW)
└── test-performance-stress.md        (← NEW)

autodocs/tests/e2e/
├── _meta.md (exists already)
├── index.md (exists already)
├── test-login-flow-e2e.md            (← NEW)
├── test-registration-flow-e2e.md     (← NEW)
└── test-workout-tracking-e2e.md      (← NEW)
```

### Schritt 2: Create Test Doc from Template

**Template verwendend:** `[[../templates/test-template.md]]`

**Example: test-user-auth-controller.md**

```yaml
---
type: test
created: 2026-03-21
updated: 2026-03-21
category: unit
area: authentication
status: documented
tags: [test, unit, auth, controller]
feature_mappings:
  - "[[../../features/2026-03-21-user-management]]"
test_file: "Fitness.Tests/Controllers/UserControllerTests.cs"
test_methods:
  - "Login_WithValidCredentials_ReturnsOkResult"
  - "Login_WithInvalidPassword_ReturnsBadRequest"
  - "Login_WithNonExistentUser_ReturnsNotFound"
  - "Register_WithValidData_CreatesUser"
coverage: 85%
---

# Test: User Authentication Controller

## Overview
Unit tests for `UserController.Login()` and `UserController.Register()` endpoints.

## Test File Location
- **Path:** `Fitness.Tests/Controllers/UserControllerTests.cs`
- **Framework:** xUnit
- **Mocking:** Moq
- **Assertions:** FluentAssertions

## Test Cases

| Test Method | Description | Pass? |
|---|---|---|
| `Login_WithValidCredentials_ReturnsOkResult` | Valid email + password | ✅ |
| `Login_WithInvalidPassword_ReturnsBadRequest` | Wrong password | ✅ |
| `Login_WithNonExistentUser_ReturnsNotFound` | User not found | ✅ |
| `Register_WithValidData_CreatesUser` | Valid registration flow | ✅ |
| `Register_WithInvalidEmail_ReturnsBadRequest` | Email validation | ✅ |

## Coverage

- **Method Coverage:** 85% (5 of 6 methods in `UserController`)
- **Line Coverage:** 80%
- **Branch Coverage:** 75%

## Related Feature Documentation

- [[../../features/2026-03-21-user-management]] (Feature being tested)

## Related Quality Scenarios

- [[../../quality_scenarios#qs-001-user-login-with-valid-credentials]]
- [[../../quality_scenarios#qs-002-user-login-with-invalid-password]]
- [[../../quality_scenarios#qs-003-user-registration]]

## Test Execution

```powershell
# Run this specific test:
dotnet test --filter "UserControllerTests"

# Run with coverage report:
dotnet test /p:CollectCoverage=true /p:CoverageFormat=opencover
```

---

[[index]]
```

### Schritt 3: Bidirectional Linking

**In Feature Doc:** `2026-03-21-user-management.md`

Füge hinzu:
```markdown
## Test Documentation

Related Test Docs:
- [[../../tests/unit/test-user-auth-controller.md]]
- [[../../tests/e2e/test-login-flow-e2e.md]]
```

**In Test Doc:** Links zurück zur Feature
```markdown
## Related Feature Documentation

- [[../../features/2026-03-21-user-management]]
```

### Schritt 4: Create E2E Test Docs

**Beispiel: test-login-flow-e2e.md**

```markdown
---
type: test
created: 2026-03-21
category: e2e
test_file: "Fitness.Tests/Integration/UserAuthenticationE2ETests.cs"
feature_mappings:
  - "[[../../features/2026-03-21-user-management]]"
---

# E2E Test: Login Flow

## Scenario: Complete User Login End-to-End

1. User navigates to `/login`
2. User enters email + password
3. User clicks "Login" button
4. System validates credentials against database
5. System authenticates with OAuth (if configured)
6. System issues JWT token
7. User redirected to `/dashboard`

## Test Cases

- ✅ Happy path: Valid login
- ✅ Invalid password: Error message
- ✅ Account locked: Show lockout message
- ✅ OAuth fallback: If Google down, use local auth

---
```

---

## Acceptance Criteria

- [ ] **Blocker cleared:** [[../todo/todo-create-unit-test-infrastructure]] marked COMPLETED
- [ ] **Unit test docs created:** 6-8 docs in `/tests/unit/`
- [ ] **E2E test docs created:** 3-4 docs in `/tests/e2e/`
- [ ] **Bidirectional links:** All feature ↔ test links exist
- [ ] **Coverage reported:** Each test doc shows % coverage
- [ ] **No broken links:** All wikilinks validate

---

## Effort Estimation

| Phase | Time |
|-------|------|
| Create 6-8 unit test docs | 8h |
| Create 3-4 e2e test docs | 4h |
| Bidirectional linking | 2h |
| Link validation | 1h |
| Index updates | 1h |
| **TOTAL** | **16 hours** |

---

## Dependencies

- **Blocked By:** [[../todo/todo-create-unit-test-infrastructure]] (tests must exist first)

---

## Priority Calculation

- **Impact:** 3/5 — Improves test traceability
- **Urgency:** 2/5 — Can wait for tests to be written
- **Effort:** 2/5 — ~16 hours
- **Priority Score:** (3×2) + (2×1.5) - (2×0.5) = **8 → MEDIUM**

---

## Status: BLOCKED

**⚠️ Note:** This TODO is blocked until [[../todo/todo-create-unit-test-infrastructure]] is completed (OPEN status).

Once tests are written, immediately create test documentation.

---

[[index]]

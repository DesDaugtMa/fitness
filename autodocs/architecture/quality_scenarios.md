---
title: "Quality Scenarios"
date: 2026-03-21
type: architecture
view_type: quality
status: accepted
tags: [architecture, quality, scenarios, isaqb, quality/scenarios]
related_docs:
  - "[[../quality_goals]]"
  - "[[../runtime_view]]"
  - "[[../adrs/index]]"
  - "[[../tests/index]]"
source_files:
  - Source/Fitness/Controllers/AccountController.cs
  - Source/Fitness/Controllers/ExercisesController.cs
---

# Quality Scenarios

## Einführung

Quality Scenarios konkretisieren die Quality Goals in **testbare, messbare Szenarien**. Format: **Stimulus → Kontext → Antwort → Maßeinheit**

---

## 🔐 Security & Authentication

### QS-001: Secure Password Hashing

**Quality Goal:** [[../quality_goals#1-security]]

**Stimulus:**  
Neuer Benutzer registriert sich mit Passwort "MyPassword123".

**Kontext:**
- Normaler Anmeldefall
- Passwort hat 8+ Zeichen, erfüllt Komplexitätsanforderungen
- Database ist verfügbar

**Antwort / Akzeptanzkriterien:**
1. Server hasht Passwort mit IPasswordHasher<User> (PBKDF2, 600.000 Iterationen)
2. Plaintext-Passwort wird **nie** geloggt oder in Datenbank gespeichert
3. Bei Login wird eingegebenes Passwort gegen Hash verglichen (VerifyHashedPassword)
4. Hash-Vergleich schlägt fehl → "Invalid credentials" (generische Meldung, kein Username Enumeration)

**Maßeinheit:**
- ✅ Getestete Assertions: Hash != Plaintext, VerifyHashedPassword(input) == true, false auf falsches Passwort

**Current Implementation:** ✅ Implemented in `AccountController.Register()`

**Status:** ✅ Accepted

---

### QS-002: OAuth2 Token Validation

**Quality Goal:** [[../quality_goals#1-security]]

**Stimulus:**  
Benutzer klickt "Login with Google" und durchläuft OAuth2 Flow.

**Kontext:**
- Normales Netzwerk
- Google OAuth2 ist erreichbar
- App hat gültige Client ID/Secret in AppSettings

**Antwort:**
1. App speichert OAuth2 `state` parameter in Session vor Redirect
2. GoogleResponse-Callback prüft: `request.state` == `session.state`
3. State Mismatch → 400 Bad Request (CSRF-Schutz)
4. Code wird gegen Access/ID Token ausgetauscht (Backend, nicht Frontend)
5. ID Token wird dekodiert und Signature validiert
6. Claims (email, name) werden extrahiert und gegen Database user geprüft

**Maßeinheit:**
- ✅ State parameter validiert (unit test)
- ✅ Token signature validiert (library validation)
- ✅ Session user matching (integration test)

**Current Implementation:** ⚠️ Partial (Code exists, but Retry policy missing on token exchange)

**Status:** ⚠️ Partial (see [[../architecture_risks#risk-002]])

---

### QS-003: Anti-CSRF Protection on Forms

**Quality Goal:** [[../quality_goals#1-security]]

**Stimulus:**  
Benutzer wird auf Phishing-Site weitergeleitet, die versucht, einen Workout zu löschen.

**Kontext:**
- Benutzer hat aktive Fitness WebApp Session
- Phishing-Site sendet Cross-Site Request (POST /Exercises/Delete/5)

**Antwort:**
1. WebApp generiert CSRF-Token in jeder GET-Response (Form-Hidden-Field)
2. POST-Endpoint hat `[ValidateAntiForgeryToken]` Attribute
3. CSRF-Token muss im Request-Body / Request-Header sein
4. Token mismatch → 400 Bad Request
5. Phishing-Request wird blockiert (Browser schickt kein CSRF-Token)

**Maßeinheit:**
- ✅ ASP.NET Core default configuration (`builder.Services.AddAntiforgery()`)
- ✅ [ValidateAntiForgeryToken] auf allen POST/DELETE/PUT actions

**Current Implementation:** ✅ Implemented via ASP.NET Core defaults

**Status:** ✅ Accepted

---

## ⚡ Performance & Response Time

### QS-005: P95 Login Response Time

**Quality Goal:** [[../quality_goals#3-performance]]

**Stimulus:**  
100 gleichzeitige Benutzer senden Login-Requests (POST /Account/Login).

**Kontext:**
- Normaler Arbeitstag (9-17 Uhr)
- Keine DB-Ausfallzeiten
- Standard Workout-Last (< 10MB Datenbank)

**Antwort:**
1. Server akzeptiert alle Requests
2. 95% der Responses sind < 500ms Response Time
3. 99% der Responses sind < 1000ms
4. Keine HTTP 500 Fehler (ausreißer sind HTTP 200)
5. Keine Timeouts

**Maßeinheit:**
- **P95 latency:** < 500ms
- **P99 latency:** < 1000ms
- **Error rate:** < 0.1%
- **Durchsatz:** > 200 login/sec (bei 100 concurrent users)

**Current Implementation:** ⚠️ Unknown (No load testing harness)

**Status:** ⚠️ Needs Validation (Create [[../todo/todo-load-testing]])

**Test Command:**
```bash
# Apache Bench (simple load test)
ab -n 1000 -c 100 https://fitness.app/Account/Login
```

---

### QS-006: N+1 Query Prevention (Exercises List)

**Quality Goal:** [[../quality_goals#3-performance]]

**Stimulus:**  
Benutzer lädt Dashboard mit Liste von 50 Exercises.

**Kontext:**
- Benutzer hat 50 Exercises in DB
- Jede Exercise hat MuscleGroup FK
- No caching

**Antwort:** Database Query Count:
1. **❌ Problematisch:** 51 queries (1 für List, +50 für jede Exercise's MuscleGroup) = O(n)
2. **✅ Ziel:** 2 queries (1 für List mit JOIN, 1 für MuscleGroups Lookup) = O(1)

**Maßeinheit:**
- **Query count:** ≤ 2 (nicht 51)
- **Query time:** < 100ms total
- **Memory used:** EF DbSet<Exercise> ≤ 10MB for 1000 exercises

**Implementation Guidance:**
```csharp
// ❌ BAD (N+1)
var exercises = await context.Exercises.ToListAsync();

// ✅ GOOD (Eager Loading)
var exercises = await context.Exercises
    .Include(e => e.MuscleGroup)
    .ToListAsync();
```

**Current Implementation:** ⚠️ Likely N+1 (need code review)

**Status:** ⚠️ Code Review Required

---

## 🔄 Reliability & Availability

### QS-004: Database Failover (High Availability)

**Quality Goal:** [[../quality_goals#2-availability]]

**Stimulus:**  
Primary SQL Server crashes (power failure, network partition).

**Kontext:**
- Production environment
- 100 active users
- Database was reachable 1 second ago

**Antwort (Expected):**
1. Connection timeout: ~5-10 seconds (TCP timeout)
2. Application detects connection loss
3. Option A (Current): Error page shown to users (RTO ~30s if auto-restart)
4. Option B (Recommended): Automatic failover to secondary replica (RTO < 10s)

**Maßeinheit (SLA):**
- **RTO (Recovery Time Objective):** < 5 minutes
- **RPO (Recovery Point Objective):** < 1 minute of data loss
- **Availability:** 99.5% uptime (allowed downtime: 3.6 hours/month)

**Current Implementation:** ❌ No HA (Single instance)

**Status:** ⚠️ Open (see [[../architecture_risks#risk-004]])

**Mitigation:**
- [[../todo/todo-sql-server-availability-groups]]

---

## 🧪 Testability

### QS-008: Unit Test Coverage

**Quality Goal:** [[../quality_goals#5-testability]]

**Stimulus:**  
Developer adds new Exercise validation rule.

**Kontext:**
- Fresh code checkout from main branch
- Test framework configured (xUnit/NUnit)
- Database not required for unit tests

**Antwort:**
1. New unit test can be written without mocking entire data store
2. Test run completes in < 5 seconds
3. Test isolation: each test is independent (no setUp/tearDown issues)
4. Mock IPasswordHasher, IRepository, IAuthContext as needed

**Maßeinheit:**
- **Test count:** ≥ 1 per public method (Controllers, Services)
- **Coverage:** ≥ 80% lines of code (excluding Views, aspx)
- **Execution time:** < 10 seconds for full suite
- **CI/CD:** All tests run on every commit

**Current Implementation:** ⚠️ No visible test project

**Status:** ⚠️ Missing Tests (Create [[../todo/todo-unit-test-setup]])

---

### QS-009: Integration Test Setup

**Quality Goal:** [[../quality_goals#5-testability]]

**Stimulus:**  
Developer writes integration test: "User registers, then logs in".

**Kontext:**
- Test database (SQLite or isolated SQL Server instance)
- Full HTTP request stack
- Real FitnessDbContext

**Antwort:**
1. Test database is seeded with test data
2. HTTP client makes request to test server
3. Database state is validated post-request
4. Test database is cleaned up (or reset) after test

**Maßeinheit:**
- **Setup time:** < 30 seconds per test execution
- **Isolation:** Tests don't affect each other
- **Coverage:** >= 1 integration test per user journey (Login, CRUD, etc.)

**Current Implementation:** ⚠️ Unclear

**Status:** ⚠️ Needs Documentation

---

## Summary: Acceptance Criteria Status

| Scenario | Category | Status | Priority |
|---|---|---|---|
| QS-001: Password Hashing | Security | ✅ Accepted | High |
| QS-002: OAuth2 Validation | Security | ⚠️ Partial | High |
| QS-003: Anti-CSRF | Security | ✅ Accepted | High |
| QS-005: Login Performance | Performance | ⚠️ Needs Test | High |
| QS-006: N+1 Prevention | Performance | ⚠️ Needs Review | Medium |
| QS-004: DB Failover | Reliability | ❌ Missing | High |
| QS-008: Unit Tests | Testability | ❌ Missing | Medium |
| QS-009: Integration Tests | Testability | ⚠️ Unclear | Medium |

---

## Related Documentation

- [[../quality_goals]] — Parent Quality Goals
- [[../runtime_view]] — Detailed sequence diagrams for scenarios
- [[../architecture_risks]] — Risks related to unmet scenarios
- [[../tests/index]] — Test documentation

---

## Navigation

[[index]] — Architektur-Übersicht
[[quality_goals]] — Quality Goals (High-level)

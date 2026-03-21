---
title: "Quality Goals"
date: 2026-03-21
type: architecture
view_type: quality
status: accepted
tags: [architecture, quality, goals, isaqb, quality/goals]
related_docs:
  - "[[../quality_scenarios]]"
  - "[[../architecture_risks]]"
  - "[[../adrs/index]]"
  - "[[../features/index]]"
source_files:
  - Source/Fitness/Program.cs
  - Source/Fitness/Config/AppSettings.cs
---

# Quality Goals

## Einführung

Quality Goals sind die wichtigsten Non-Functional-Requirements der Fitness WebApp. Sie werden nach **ISO/IEC 25010** kategorisiert und mit **Stakeholder-Perspektiven** verlinkt.

**Gesamtprioritätsmatrix:** (Impact × 2) + (Urgency × 1.5) - (Effort × 0.5)

---

## 🏆 Top 5 Quality Goals (Priorisiert)

### 1. Security — Schutz von Benutzer-Credentials & Workouts

**Category:** Security (ISO/IEC 25010: Security)  
**Priority:** 🔴 Critical  
**Impact:** 10 | Urgency:** 10 | Effort:** 8  
**Score:** (10×2) + (10×1.5) - (8×0.5) = 27  

**Beschreibung:**  
Benutzer-Autentifizierung, Passwort-Hashing und OAuth2 Flows müssen sicher implementiert sein. Daten sind PII-sensitiv (E-Mail, Workout-History).

**Stakeholder Concerns:**
- **End Users:** "Mein Passwort ist sicher gehashing?" → ✅ PBKDF2 default
- **Security Team:** "Does the app follow NIST guidelines?" → ⚠️ Rate-limiting missing
- **Compliance:** "Is user data encrypted at rest?" → ❌ GAP-003

**Related Scenarios:**
- [[../quality_scenarios#qs-001-secure-password-hashing]]
- [[../quality_scenarios#qs-002-oauth2-token-validation]]

**Acceptance Criteria:**
- All passwords hashed with IPasswordHasher<T> (PBKDF2)
- OAuth2 state parameter validation before token exchange
- HTTPS enforced for all public endpoints
- Secrets never logged or exposed in error messages

**Current Status:** ⚠️ Partial (local auth OK, OAuth retry + at-rest encryption missing)

---

### 2. Availability — System ist zugreifbar wenn es gebraucht wird

**Category:** Reliability (ISO/IEC 25010: Reliability)  
**Priority:** 🟠 High  
**Impact:** 9 | Urgency:** 8 | Effort:** 6  
**Score:** (9×2) + (8×1.5) - (6×0.5) = 25

**Beschreibung:**  
Benutzer sollen jederzeit ihre Workouts anzeigen und erstellen können. System sollte > 99% verfügbar sein (allowing planned maintenance windows).

**Stakeholder Concerns:**
- **End Users:** "Can I access my workouts during peak hours?" → Depends on autoscaling
- **Ops Team:** "What's the failover strategy?" → Missing load balancer config
- **Gym Owners:** "Do you guarantee 99.5% uptime?" → SLA not defined

**Related Scenarios:**
- [[../quality_scenarios#qs-003-concurrent-user-load]]
- [[../quality_scenarios#qs-004-database-failover]]

**Acceptance Criteria:**
- P99 latency < 2 seconds for GET requests (Dashboard, Exercise list)
- P99 latency < 3 seconds for POST requests (Create Exercise)
- Database connection pool sized for peak load
- Auto-restart on unhandled exception
- Health check endpoint `/health` responds within 500ms

**Current Status:** ⚠️ Partial (Single instance, no HA config)

---

### 3. Performance — Schnelle Response-Times

**Category:** Performance Efficiency (ISO/IEC 25010: Performance)  
**Priority:** 🟠 High  
**Impact:** 8 | Urgency:** 7 | Effort:** 5  
**Score:** (8×2) + (7×1.5) - (5×0.5) = 24

**Beschreibung:**  
Benutzer erwarten schnelle Antworten (< 1s für einfache operations). Langsame Seiten führen zu User Frustration und Drop-off.

**Stakeholder Concerns:**
- **Product Manager:** "Slow login = churn. Optimize." → OAuth timeout is not monitored
- **UX Team:** "Page load time < 2s?" → Depends on network
- **Database Team:** "Can we handle 1000 concurrent users?" → No load test data

**Related Scenarios:**
- [[../quality_scenarios#qs-005-p95-login-response-time]]
- [[../quality_scenarios#qs-006-n-plus-one-query-prevention]]

**Acceptance Criteria:**
- Dashboard page load: P95 < 1 second
- Login page: P95 < 500ms
- Exercise CRUD: P95 < 500ms
- Database query: No N+1 queries (use .Include() in EF)
- No unindexed joins on large tables

**Current Status:** ⚠️ Unknown (No performance baselines, no load testing)

---

### 4. Maintainability — Code ist lesbar & änderbar

**Category:** Maintainability (ISO/IEC 25010: Maintainability)  
**Priority:** 🟡 Medium  
**Impact:** 7 | Urgency:** 6 | Effort:** 4  
**Score:** (7×2) + (6×1.5) - (4×0.5) = 23

**Beschreibung:**  
Neuer Dev sollte in 1 Tag produktiv sein. Code sollte gut dokumentiert und navigierbar sein.

**Stakeholder Concerns:**
- **Tech Lead:** "How long to add a new feature?" → Depends on architecture clarity
- **Junior Developer:** "Can I understand the codebase?" → Docs help
- **Code Reviewer:** "Is this code consistent with standards?" → No style guide

**Related Scenarios:**
- [[../quality_scenarios#qs-007-feature-addition-time]]

**Acceptance Criteria:**
- All public classes have XML documentation
- Controller actions follow standard CRUD naming
- Consistent folder structure (Controllers/, Models/, Views/)
- Less than 100 LoC per method (excluding views)
- Clear naming: variables, methods, classes

**Current Status:** ✅ Good (Standard MVC patterns, clear structure)

---

### 5. Testability — Code ist testbar

**Category:** Maintainability (ISO/IEC 25010: Testability)  
**Priority:** 🟡 Medium  
**Impact:** 6 | Urgency:** 5 | Effort:** 7  
**Score:** (6×2) + (5×1.5) - (7×0.5) = 19

**Beschreibung:**  
Unit & Integration Tests sollen einfach geschrieben werden können. Abhängigkeits-Injection und loose coupling sind erforderlich.

**Stakeholder Concerns:**
- **QA Team:** "How do we test the app?" → Manual + some automation needed
- **Dev Team:** "How long does test suite run?" → No CI/CD pipeline visible
- **Security Team:** "Is the OAuth integration tested?" → Unclear

**Related Scenarios:**
- [[../quality_scenarios#qs-008-unit-test-coverage]]
- [[../quality_scenarios#qs-009-integration-test-database]]

**Acceptance Criteria:**
- Controllers are injectable (dependencies in constructor)
- Business logic separated from UI (services layer)
- Test database setup is automated
- Unit tests run in < 5 seconds
- Integration tests run in < 30 seconds

**Current Status:** ⚠️ Partial (No visible test project or CI/CD)

---

## 📊 Quality Goals Matrix

| Goal | Security | Reliability | Performance | Maintainability | Testability | **Overall Score** |
|---|---|---|---|---|---|---|
| **Security** | ■■■■■ | ■■■ | ■ | ■■ | ■■ | 27 |
| **Availability** | ■■ | ■■■■■ | ■■■ | ■ | ■ | 25 |
| **Performance** | ■■ | ■■■ | ■■■■■ | ■ | ■ | 24 |
| **Maintainability** | ■ | ■■ | ■ | ■■■■■ | ■■■ | 23 |
| **Testability** | ■■ | ■■ | ■ | ■■■ | ■■■■ | 19 |

---

## Stakeholder-Perspektiven

| Stakeholder | Top Priority | Concern | Link |
|---|---|---|---|
| **End Users (Gym members)** | Availability + Performance | "I need to log my workouts NOW" | [[../quality_scenarios#qs-003]] |
| **Gym Owner** | Security + Availability | "User data must be secure & accessible" | [[../quality_scenarios#qs-001]] + [[../quality_scenarios#qs-004]] |
| **Product Manager** | Performance + Testability | "Ship features fast, maintain quality" | [[../quality_scenarios#qs-005]] + [[../quality_scenarios#qs-007]] |
| **DevOps/Ops Team** | Availability + Maintainability | "Minimize incidents, easy to deploy" | [[../quality_scenarios#qs-004]] + [[../constraints]] |
| **Security Officer** | Security | "No data breaches, compliance with GDPR" | [[../quality_scenarios#qs-001]] + [[../quality_scenarios#qs-002]] |

---

## Trade-offs

| Trade-off | Option A | Option B | Decision | Reason |
|---|---|---|---|---|
| **Caching** | In-memory (fast) | Distributed Cache (Redis, slow) | Use In-Memory for now | Simplicity, no Redis infra |
| **API Versioning** | URL-based (`/api/v1/`) | Header-based | None (MVC routing only) | Current monolith doesn't need it yet |
| **Rate Limiting** | Token Bucket | Leaky Bucket | Not implemented ⚠️ | GAP-001: Must add soon |
| **Database Encryption** | TDE (SQL Server) | Application-level | Recommend TDE | Performance < Security |

---

## Related Documentation

- [[../quality_scenarios]] — Konkrete Messkriterien & Akzeptanzszenarien
- [[../architecture_risks]] — Risiken zu Quality Goals
- [[../constraints]] — Technische Randbedingungen die Goals beeinflussen
- [[../adrs/index]] — Entscheidungen die Quality Goals tragen

---

## Navigation

[[index]] — Architektur-Übersicht
[[quality_scenarios]] — Quality Scenarios (Acceptance Criteria)

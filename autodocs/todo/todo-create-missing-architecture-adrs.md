---
type: todo
created: 2026-03-21
status: open
priority: high
category: documentation
area: architecture
tags: [todo, adr, documentation, architecture, cmp/core]
estimated_effort: 24
impact: 4
urgency: 3
effort: 3
priority_score: 11
related_adrs:
  - "[[../adrs/adr-001-use-entity-framework]]"
  - "[[../templates/adr-template]]"
resolved_by: []
source_documents:
  - "[[../auditor_non_compliance#nc-maj-002-incomplete-architecture-risk-documentation]]"
  - "[[../architecture_risks]]"
---

# TODO: Create Missing Architecture Decision Records (ADRs)

## Problem/Idee

**Ausgangsituation:** Nur 2 ADRs vorhanden (ADR-001: EF, ADR-001-doc-system)  
**Architekturrrisiken ohne Dokumentation:** 4-5 Major Decisions ungeklärt  
**Ziel:** 4 neue ADRs erstellen (ADR-002 bis ADR-005)  
**Gap:** 2 ADRs → 6-7 ADRs

**Szenario:** Neuer Developer fragt "Warum nutzen wir keine Rate Limiting?" → Keine ADR, muss Audit-Report lesen

---

## Business-Value

- **Knowledge Transfer:** Neue Team-Members lernen Architektur-Rationale
- **Decision Consistency:** Verhindert sich wiederholende Debatten
- **Risk Mitigation:** Dokumentiert akzeptierte Risiken (RR-001 bis RR-006)

---

## Schritt-für-Schritt-Anleitung

### Schritt 1: ADR-002 — Unit Testing Strategy

**Datei:** `autodocs/adrs/adr-002-unit-testing-strategy.md`

```markdown
---
type: adr
created: 2026-03-21
status: accepted
decision_date: 2026-03-21
tags: [adr, testing, xunit, architecture]
---

# ADR-002: Unit Testing Strategy (xUnit + Moq + FluentAssertions)

## Context

Project started without test infrastructure. No unit tests, all QA manual. This risks production bugs and slows development velocity.

## Decision

Use **xUnit + Moq + FluentAssertions** for unit testing:
- **xUnit:** Because it's the .NET standard, integrates with Visual Studio, supports parameterized tests
- **Moq:** For mocking dependencies (lightweight, intuitive)
- **FluentAssertions:** Better assertion readability (`result.Should().BeNull()` vs `Assert.Null(result)`)

Target coverage: **≥80%** for core logic (Controllers, Services, Models)

## Rationale

### Why xUnit over NUnit?
- Better async support in xUnit
- More modern fluent API
- Better CI/CD integration (GitHub Actions, Azure DevOps)

### Why Moq?
- Lightweight (no heavy container)
- Easy to learn (1-day ramp-up for new devs)
- Sufficient for our needs (no advanced scenarios needed)

### Why FluentAssertions?
- Readable: `foo.Should().Be(expected)` vs `Assert.AreEqual(foo, expected)`
- Better error messages on failure

## Consequences

- **Positive:** Improved quality, faster feedback, fewer production bugs
- **Positive:** Better Developer confidence in refactoring
- **Negative:** ~1-2 weeks effort to set up initially
- **Negative:** Tests add ~15-20% maintenance overhead

## Implementation Path

See: [[../todo/todo-create-unit-test-infrastructure]]

## Status

Accepted

---
```

### Schritt 2: ADR-003 — Rate Limiting Strategy

```markdown
---
type: adr
created: 2026-03-21
status: accepted
---

# ADR-003: Rate Limiting Strategy

## Context

API endpoints vulnerable to DDoS and brute-force attacks (no rate limiting implemented).

## Decision

Use **AspNetCore.RateLimiting** middleware with fixed-window polic:
- 100 requests per minute per IP (standard)
- 10 requests per 10 seconds for sensitive endpoints (login/register)
- Custom 429 response with Retry-After header

## Rationale

- Built-in to .NET 7+, no external dependencies
- Simple to configure per-endpoint
- Transparent to application code

## Status

Accepted

---
```

### Schritt 3: ADR-004 — Data Encryption Policy

```markdown
---
type: adr
created: 2026-03-21
status: accepted
---

# ADR-004: At-Rest PII Encryption Policy

## Context

PII (email, names) stored in SQL Server without encryption. GDPR Article 32 requires encryption of personal data.

## Decision

Use **SQL Server Transparent Data Encryption (TDE)** for at-rest encryption:
- All database files (.mdf) encrypted by TDE
- Passwords already hashed (bcrypt/PBKDF2) — additional encryption not needed
- Backup files encrypted

## Rationale

- TDE is transparent to application (no code changes)
- Minimal performance impact (typically <3%)
- GDPR compliant
- Standard SQL Server feature

## Alternatives Considered

1. Application-level encryption (too complex, slower)
2. Column-level encryption (fine-grained but overkill)
3. No encryption (not compliant)

## Status

Accepted

---
```

### Schritt 4: ADR-005 — High Availability Strategy (Deferred)

```markdown
---
type: adr
created: 2026-03-21
status: proposed
---

# ADR-005: SQL Server High Availability Strategy (Q3 2026)

## Context

Single SQL Server instance = single point of failure. For MVP acceptable (manual restart RTO ~30min). For production scale, need HA.

## Proposed Decision

Post Q2 2026:
- Migrate to **Azure SQL Managed Instance** (automatic failover, <10s RTO) OR
- Setup **SQL Server AlwaysOn** Availability Groups (on-premises option)

Target: 99.95% uptime SLA

## Implementation Timeline

- Q2 2026: Cost analysis, pilot with non-prod replicas
- Q3 2026: Implement after reaching 1000+ DAU

## Status

Proposed (deferred)

---
```

### Schritt 5: Create ADRs via Command Line

**Command:**
```powershell
# Create adr-002, adr-003, adr-004, adr-005 files
cd C:\Repositories\fitness\autodocs\adrs

# Content for each file as shown above
```

---

## Acceptance Criteria

- [ ] ADR-002 created: UnitTesting Strategy (xUnit, Moq, FluentAssertions)
- [ ] ADR-003 created: Rate Limiting (100 req/min per IP)
- [ ] ADR-004 created: At-Rest Encryption (TDE)
- [ ] ADR-005 created: High Availability (Q3 deferred)
- [ ] All cross-referenced to risks and RRs
- [ ] All link validation passes (no broken links)

---

## Effort Estimation

| Phase | Time |
|-------|------|
| ADR-002 (Testing) | 2h |
| ADR-003 (Rate Limiting) | 1h |
| ADR-004 (Encryption) | 1h |
| ADR-005 (HA Planning) | 1h |
| Linking & Validation | 1h |
| **TOTAL** | **6 hours** |

---

## Priority Calculation

- **Impact:** 4/5 — Improves documentation & decision traceability
- **Urgency:** 3/5 — Should complete Q2 2026
- **Effort:** 3/5 — ~24 hours
- **Priority Score:** (4×2) + (3×1.5) - (3×0.5) = **11 → HIGH**

---

[[index]]
